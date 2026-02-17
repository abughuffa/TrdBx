using System.Diagnostics;
using System.Text;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.RestoreBackup.DTOs;
using DocumentFormat.OpenXml.InkML;
using Npgsql;

namespace CleanArchitecture.Blazor.Infrastructure.Services.RestoreBackupStrategies;
public class PostgresBackupRestoreStrategy : IDatabaseBackupRestoreStrategy
{
    public string BackupFileExtension => ".backup";

    public async Task<bool> CreateBackupAsync(string connectionString, string backupPath, string backupName)
    {
        var backupFile = Path.Combine(backupPath, $"{backupName}{BackupFileExtension}");

        // Ensure the directory exists
        Directory.CreateDirectory(Path.GetDirectoryName(backupFile));

        var builder = new NpgsqlConnectionStringBuilder(connectionString);

        // Use pg_dump for PostgreSQL backup
        var pgDumpPath = FindExecutable("pg_dump");
        if (string.IsNullOrEmpty(pgDumpPath))
            throw new InvalidOperationException("pg_dump utility not found. Please ensure PostgreSQL client tools are installed.");

        var arguments = $"-h {builder.Host} -p {builder.Port} -U {builder.Username} -F c -b -v -f \"{backupFile}\" {builder.Database}";

        var startInfo = new ProcessStartInfo
        {
            FileName = pgDumpPath,
            Arguments = arguments,
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            CreateNoWindow = true
        };

        startInfo.Environment["PGPASSWORD"] = builder.Password;

        using var process = new Process { StartInfo = startInfo };
        process.Start();

        var error = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
            throw new Exception($"PostgreSQL backup failed: {error}");

        return await Task.FromResult(true);
    }
    public async Task<bool> RestoreBackupAsync(string connectionString, string backupPath, string backupName)
    {
        bool truncateTablesFirst = true;
        var backupFile = Path.Combine(backupPath, backupName);

        // Validate backup file exists
        if (!File.Exists(backupFile))
        {
            throw new FileNotFoundException($"Backup file not found: {backupFile}");
        }

        var builder = new NpgsqlConnectionStringBuilder(connectionString);

        // Use pg_restore for PostgreSQL restore
        var pgRestorePath = FindExecutable("pg_restore");
        if (string.IsNullOrEmpty(pgRestorePath))
            throw new InvalidOperationException("pg_restore utility not found. Please ensure PostgreSQL client tools are installed.");

        try
        {
            // Terminate all connections to the database
            await TerminateConnections(connectionString, builder.Database);

            // Truncate all tables if requested
            if (truncateTablesFirst)
            {
                Console.WriteLine("Truncating tables before restore...");
                await TruncateAllTables(connectionString, builder.Database);
            }

            // Build arguments - use --no-owner to avoid permission issues
            var arguments = $"--no-owner --no-privileges --clean --if-exists " +
                           $"-h {EscapeArgument(builder.Host)} " +
                           $"-p {builder.Port} " +
                           $"-U {EscapeArgument(builder.Username)} " +
                           $"-d {EscapeArgument(builder.Database)} " +
                           $"--disable-triggers --single-transaction --verbose " +
                           $"\"{EscapeArgument(backupFile)}\"";

            var startInfo = new ProcessStartInfo
            {
                FileName = pgRestorePath,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                WorkingDirectory = Path.GetDirectoryName(backupFile) // Set working directory to backup location
            };

            // Set password environment variable
            if (!string.IsNullOrEmpty(builder.Password))
            {
                startInfo.Environment["PGPASSWORD"] = builder.Password;
            }

            using var process = new Process { StartInfo = startInfo };

            var outputBuilder = new StringBuilder();
            var errorBuilder = new StringBuilder();

            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    outputBuilder.AppendLine(e.Data);
                    Console.WriteLine($"pg_restore output: {e.Data}");
                }
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    errorBuilder.AppendLine(e.Data);
                    Console.WriteLine($"pg_restore error: {e.Data}");
                }
            };

            Console.WriteLine($"Starting restore from {backupFile} to database {builder.Database}");

            if (!process.Start())
            {
                throw new Exception("Failed to start pg_restore process.");
            }

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await process.WaitForExitAsync();

            var output = outputBuilder.ToString();
            var error = errorBuilder.ToString();

            if (process.ExitCode != 0)
            {
                // Check if it's just constraint violation warnings
                if (error.Contains("warning: errors ignored on restore") ||
                    error.Contains("violates foreign key constraint") ||
                    error.Contains("WARNING:"))
                {
                    Console.WriteLine("Warning: Some foreign key violations occurred but were ignored due to --disable-triggers");
                    Console.WriteLine("Data restore completed with warnings.");
                    Console.WriteLine($"Warnings: {error}");
                    return await Task.FromResult(true);
                }

                throw new Exception($"PostgreSQL restore failed with exit code {process.ExitCode}: {error}\nOutput: {output}");
            }

            Console.WriteLine("Data restore completed successfully.");
            return await Task.FromResult(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Restore failed: {ex.Message}");
            throw;
        }
    }

    public async Task<List<RestoreBackupFileDto>> GetBackupsAsync(string backupPath)
    {
        if (!Directory.Exists(backupPath))
            Directory.CreateDirectory(backupPath);

        var backupFiles = new List<RestoreBackupFileDto>();

        foreach (var file in Directory.GetFiles(backupPath).Where(f => f.EndsWith(BackupFileExtension)))
        {
            var fileInfo = new FileInfo(file);
            backupFiles.Add(new RestoreBackupFileDto
            {
                Name = Path.GetFileName(file),
                Path = file,
                Size = fileInfo.Length,
                Created = fileInfo.CreationTime
            });
        }

        return await Task.FromResult(backupFiles.OrderByDescending(f => f.Created).ToList());
    }

    private async Task TruncateAllTables(string connectionString, string databaseName, bool preserveIdentitySequences = false)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();

        // Disable foreign key constraints
        await using var disableConstraintsCmd = new NpgsqlCommand(
            "SET CONSTRAINTS ALL DEFERRED;",
            connection
        );
        await disableConstraintsCmd.ExecuteNonQueryAsync();

        List<string> tables = new List<string>();

        try
        {
            // Query to get all tables (excluding system tables)
            var getTablesCommand = new NpgsqlCommand(@"
            SELECT 
                quote_ident(nspname) AS schemaname,
                quote_ident(relname) AS tablename
            FROM pg_catalog.pg_class c
            JOIN pg_catalog.pg_namespace n ON n.oid = c.relnamespace
            WHERE c.relkind IN ('r', 'p')  -- regular tables and partitioned tables
            AND n.nspname NOT IN ('pg_catalog', 'information_schema')
            AND n.nspname NOT LIKE 'pg_toast%'
            AND n.nspname NOT LIKE 'pg_temp%'
            AND c.relpersistence = 'p'  -- permanent tables only
            AND c.relname NOT LIKE 'pg_%'
            ORDER BY nspname, relname;
        ", connection);

            // FIXED: Properly dispose the reader before executing another command
            await using (var reader = await getTablesCommand.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var schemaName = reader.GetString(0);
                    var tableName = reader.GetString(1);
                    tables.Add($"{schemaName}.{tableName}");
                }
            } // Reader is automatically disposed here

            if (tables.Count == 0)
            {
                Console.WriteLine("No tables found to truncate.");
                return;
            }

            Console.WriteLine($"Found {tables.Count} tables to truncate.");

            // Build and execute truncate command
            var truncateCommand = $"TRUNCATE TABLE {string.Join(", ", tables)}";

            if (!preserveIdentitySequences)
            {
                truncateCommand += " RESTART IDENTITY";
            }

            truncateCommand += " CASCADE;";

            await using var truncateCmd = new NpgsqlCommand(truncateCommand, connection);
            await truncateCmd.ExecuteNonQueryAsync();

            Console.WriteLine("All tables truncated successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error truncating tables: {ex.Message}");
            throw;
        }
        finally
        {
            // Re-enable foreign key constraints
            await using var enableConstraintsCmd = new NpgsqlCommand(
                "SET CONSTRAINTS ALL IMMEDIATE;",
                connection
            );
            await enableConstraintsCmd.ExecuteNonQueryAsync();
        }
    }

    private async Task TerminateConnections(string connectionString, string databaseName)
    {
        // Connect to template1 or postgres database since we can't connect to the target DB
        var masterBuilder = new NpgsqlConnectionStringBuilder(connectionString)
        {
            Database = "template1" // Use template1 as it always exists
        };

        await using var connection = new NpgsqlConnection(masterBuilder.ToString());
        await connection.OpenAsync();

        // Use parameterized query to prevent SQL injection
        var terminateCommandText = @"
        SELECT pg_terminate_backend(pg_stat_activity.pid) 
        FROM pg_stat_activity 
        WHERE pg_stat_activity.datname = @databaseName 
        AND pid <> pg_backend_pid()
        AND state <> 'idle'";

        await using var command = new NpgsqlCommand(terminateCommandText, connection);
        command.Parameters.AddWithValue("@databaseName", databaseName);

        await command.ExecuteNonQueryAsync();

        // Give connections time to close
        await Task.Delay(1000);
    }

    private string FindExecutable(string executableName)
    {
        // Check PATH first (most reliable)
        var pathDirs = Environment.GetEnvironmentVariable("PATH")?.Split(Path.PathSeparator);
        if (pathDirs != null)
        {
            var extensions = OperatingSystem.IsWindows()
                ? new[] { ".exe", ".cmd", ".bat", "" }
                : new[] { "", ".sh" };

            foreach (var dir in pathDirs)
            {
                if (!string.IsNullOrWhiteSpace(dir))
                {
                    foreach (var ext in extensions)
                    {
                        var potentialPath = Path.Combine(dir, executableName + ext);
                        if (File.Exists(potentialPath))
                            return potentialPath;
                    }
                }
            }
        }

        // Manual search in common PostgreSQL paths for Windows
        if (OperatingSystem.IsWindows())
        {
            var programFilesPaths = new[]
            {
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
        };

            // Check multiple PostgreSQL versions
            var versions = new[] { "18", "17", "16", "15", "14", "13", "12", "11", "10" };

            foreach (var programFiles in programFilesPaths)
            {
                foreach (var version in versions)
                {
                    var fullPath = Path.Combine(programFiles, "PostgreSQL", version, "bin", executableName + ".exe");
                    if (File.Exists(fullPath))
                        return fullPath;
                }
            }
        }

        return null;
    }

    private string EscapeArgument(string argument)
    {
        if (string.IsNullOrEmpty(argument))
            return argument;

        // Escape quotes and spaces for command line arguments
        return argument.Contains(" ") ? $"\"{argument.Replace("\"", "\\\"")}\"" : argument;
    }







}
