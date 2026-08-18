using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.RestoreBackup.DTOs;
using Npgsql;

namespace CleanArchitecture.Blazor.Infrastructure.Services.RestoreBackupStrategies;

public class PostgresBackupRestoreStrategy : IDatabaseBackupRestoreStrategy
{
    public string BackupFileExtension => ".backup";

    public async Task<bool> CreateBackupAsync(string connectionString, string backupPath, string backupName)
    {
        var backupFile = Path.Combine(backupPath, $"{backupName}{BackupFileExtension}");

        // Ensure the directory exists
        Directory.CreateDirectory(Path.GetDirectoryName(backupFile) ?? string.Empty);

        var builder = new NpgsqlConnectionStringBuilder(connectionString);

        // Use pg_dump for PostgreSQL backup
        var pgDumpPath = FindExecutable("pg_dump");
        if (string.IsNullOrEmpty(pgDumpPath))
            throw new InvalidOperationException("pg_dump utility not found. Please ensure PostgreSQL client tools are installed.");

        // Log the version being used
        var version = await GetToolVersion(pgDumpPath);
        Console.WriteLine($"Using pg_dump version: {version} from {pgDumpPath}");

        //// Build arguments safely - Use custom format (Fc) which is version-specific
        //var arguments = new StringBuilder();
        //arguments.Append($"-h {EscapeArgument(builder.Host)} ");
        //arguments.Append($"-p {builder.Port} ");
        //arguments.Append($"-U {EscapeArgument(builder.Username)} ");
        
        //// Use directory format for better compatibility across versions
        //// or stick with custom format but note version compatibility
        //arguments.Append("-F d -j 4 -v "); // Directory format with 4 parallel jobs
        //// Alternative: arguments.Append("-F c -b -v "); // Custom format
        
        // arguments.Append($"-f {EscapeArgument(backupFile)} ");
        // arguments.Append(EscapeArgument(builder.Database));

        // Build arguments for custom format (single file)
        
var arguments = new StringBuilder();
arguments.Append($"-h {EscapeArgument(builder.Host)} ");
arguments.Append($"-p {builder.Port} ");
arguments.Append($"-U {EscapeArgument(builder.Username)} ");
arguments.Append("-F c -b -v ");   // Custom format, include blobs, verbose
arguments.Append($"-f {EscapeArgument(backupFile)} ");
arguments.Append(EscapeArgument(builder.Database));

        var startInfo = new ProcessStartInfo
        {
            FileName = pgDumpPath,
            Arguments = arguments.ToString(),
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            CreateNoWindow = true,
            WorkingDirectory = Path.GetTempPath()
        };

        // Set password environment variable
        startInfo.EnvironmentVariables["PGPASSWORD"] = builder.Password;

        using var process = new Process { StartInfo = startInfo };
        
        var outputBuilder = new StringBuilder();
        var errorBuilder = new StringBuilder();
        
        process.OutputDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
                outputBuilder.AppendLine(e.Data);
        };
        
        process.ErrorDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
                errorBuilder.AppendLine(e.Data);
        };

        try
        {
            if (!process.Start())
                throw new InvalidOperationException("Failed to start pg_dump process.");

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            
            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                var error = errorBuilder.ToString();
                throw new Exception($"PostgreSQL backup failed with exit code {process.ExitCode}: {error}");
            }

            return true;
        }
        catch (Exception ex) when (ex is not InvalidOperationException)
        {
            throw new Exception($"Failed to execute pg_dump: {ex.Message}", ex);
        }
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

        // Check if backup is in directory format or custom format
        bool isDirectoryFormat = Directory.Exists(backupFile) || 
                                 (File.GetAttributes(backupFile) & FileAttributes.Directory) == FileAttributes.Directory;

        // Use pg_restore for PostgreSQL restore
        var pgRestorePath = FindExecutable("pg_restore");
        if (string.IsNullOrEmpty(pgRestorePath))
            throw new InvalidOperationException("pg_restore utility not found. Please ensure PostgreSQL client tools are installed.");

        // Log the version being used
        var restoreVersion = await GetToolVersion(pgRestorePath);
        Console.WriteLine($"Using pg_restore version: {restoreVersion} from {pgRestorePath}");

        // First, terminate all connections to the database (except ours)
        await TerminateConnections(connectionString, builder.Database);

        // Wait a moment for connections to be terminated
        await Task.Delay(2000);

        // Truncate all tables if requested
        if (truncateTablesFirst)
        {
            Console.WriteLine("Truncating tables before restore...");
            
            // Create a new connection for truncating tables
            var truncateConnectionString = new NpgsqlConnectionStringBuilder(connectionString)
            {
                Pooling = false,
                ConnectionLifetime = 0
            }.ToString();
            
            await TruncateAllTables(truncateConnectionString, builder.Database);
        }

        try
        {
            // Build arguments safely
            var arguments = new StringBuilder();
            
            // Common restore options
            arguments.Append("--no-owner --no-privileges --clean --if-exists ");
            
            // Add connection parameters
            arguments.Append($"-h {EscapeArgument(builder.Host)} ");
            arguments.Append($"-p {builder.Port} ");
            arguments.Append($"-U {EscapeArgument(builder.Username)} ");
            arguments.Append($"-d {EscapeArgument(builder.Database)} ");
            
            // Add performance and compatibility options
            arguments.Append("--disable-triggers --verbose ");
            
            // For directory format, we need to specify the format
            if (isDirectoryFormat)
            {
                arguments.Append("-F d ");
            }
            
            // Add parallel jobs for better performance (if directory format)
            if (isDirectoryFormat)
            {
                arguments.Append("-j 4 ");
            }
            
            // Add the backup file/directory path
            arguments.Append(EscapeArgument(backupFile));

            Console.WriteLine($"Restore command: {pgRestorePath} {arguments}");

            var startInfo = new ProcessStartInfo
            {
                FileName = pgRestorePath,
                Arguments = arguments.ToString(),
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                WorkingDirectory = Path.GetDirectoryName(backupFile) ?? Path.GetTempPath()
            };

            // Set password environment variable
            if (!string.IsNullOrEmpty(builder.Password))
            {
                startInfo.EnvironmentVariables["PGPASSWORD"] = builder.Password;
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
                // Check for version incompatibility
                if (error.Contains("unsupported version") || error.Contains("version"))
                {
                    throw new Exception($"PostgreSQL version incompatibility: The backup file was created with a different version of pg_dump than the pg_restore being used. " +
                        $"Please ensure both backup and restore use the same major version of PostgreSQL tools.\n" +
                        $"Error: {error}");
                }
                
                // Check if it's just constraint violation warnings
                if (error.Contains("warning: errors ignored on restore") ||
                    error.Contains("violates foreign key constraint") ||
                    error.Contains("WARNING:"))
                {
                    Console.WriteLine("Warning: Some foreign key violations occurred but were ignored due to --disable-triggers");
                    Console.WriteLine("Data restore completed with warnings.");
                    Console.WriteLine($"Warnings: {error}");
                    return true;
                }

                throw new Exception($"PostgreSQL restore failed with exit code {process.ExitCode}: {error}\nOutput: {output}");
            }

            Console.WriteLine("Data restore completed successfully.");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Restore failed: {ex.Message}");
            throw;
        }
    }

    private async Task<string> GetToolVersion(string toolPath)
    {
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = toolPath,
                Arguments = "--version",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using var process = new Process { StartInfo = startInfo };
            process.Start();
            string output = await process.StandardOutput.ReadToEndAsync();
            await process.WaitForExitAsync();
            return output.Trim();
        }
        catch
        {
            return "Unknown version";
        }
    }

    public async Task<List<RestoreBackupFileDto>> GetBackupsAsync(string backupPath)
    {
        if (!Directory.Exists(backupPath))
            Directory.CreateDirectory(backupPath);

        var backupFiles = new List<RestoreBackupFileDto>();

        // Look for both files and directories (directory format backups create directories)
        foreach (var fileSystemEntry in Directory.GetFileSystemEntries(backupPath))
        {
            var isDirectory = (File.GetAttributes(fileSystemEntry) & FileAttributes.Directory) == FileAttributes.Directory;
            
            // Check if it's a backup file or directory
            if ((!isDirectory && fileSystemEntry.EndsWith(BackupFileExtension)) || 
                (isDirectory && Directory.Exists(Path.Combine(fileSystemEntry, "toc.dat")))) // Directory format backup
            {
                var fileInfo = new FileInfo(fileSystemEntry);
                backupFiles.Add(new RestoreBackupFileDto
                {
                    Name = Path.GetFileName(fileSystemEntry),
                    Path = fileSystemEntry,
                    Size = isDirectory ? GetDirectorySize(fileSystemEntry) : fileInfo.Length,
                    Created = fileInfo.CreationTime,
                    IsDirectory = isDirectory
                });
            }
        }

        return backupFiles.OrderByDescending(f => f.Created).ToList();
    }

    private long GetDirectorySize(string directoryPath)
    {
        try
        {
            return new DirectoryInfo(directoryPath).GetFiles("*", SearchOption.AllDirectories).Sum(fi => fi.Length);
        }
        catch
        {
            return 0;
        }
    }

    // Rest of the methods remain the same...
    private async Task TruncateAllTables(string connectionString, string databaseName, bool preserveIdentitySequences = false)
    {
        // Use a new connection with retry logic
        int maxRetries = 3;
        int retryDelay = 2000; // 2 seconds
        
        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
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

                // Query to get all tables (excluding system tables)
                var getTablesCommand = new NpgsqlCommand(@"
                    SELECT 
                        quote_ident(nspname) AS schemaname,
                        quote_ident(relname) AS tablename
                    FROM pg_catalog.pg_class c
                    JOIN pg_catalog.pg_namespace n ON n.oid = c.relnamespace
                    WHERE c.relkind IN ('r', 'p')
                    AND n.nspname NOT IN ('pg_catalog', 'information_schema')
                    AND n.nspname NOT LIKE 'pg_toast%'
                    AND n.nspname NOT LIKE 'pg_temp%'
                    AND c.relpersistence = 'p'
                    AND c.relname NOT LIKE 'pg_%'
                    ORDER BY nspname, relname;", connection);

                await using (var reader = await getTablesCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var schemaName = reader.GetString(0);
                        var tableName = reader.GetString(1);
                        tables.Add($"{schemaName}.{tableName}");
                    }
                }

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
                
                // If we get here, truncation was successful
                return;
            }
            catch (PostgresException ex) when (ex.SqlState == "57P01") // terminating connection due to administrator command
            {
                Console.WriteLine($"Attempt {attempt} failed due to connection termination. Retrying in {retryDelay}ms...");
                if (attempt == maxRetries)
                {
                    throw new Exception($"Failed to truncate tables after {maxRetries} attempts due to connection issues.", ex);
                }
                await Task.Delay(retryDelay);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error truncating tables: {ex.Message}");
                throw;
            }
        }
    }

    private async Task TerminateConnections(string connectionString, string databaseName)
    {
        // Connect to template1 or postgres database
        var masterBuilder = new NpgsqlConnectionStringBuilder(connectionString)
        {
            Database = "template1",
            Pooling = false,
            ConnectionLifetime = 0
        };

        await using var connection = new NpgsqlConnection(masterBuilder.ToString());
        await connection.OpenAsync();

        // Terminate all connections to the target database except our own
        var terminateCommandText = @"
            SELECT pg_terminate_backend(pid) 
            FROM pg_stat_activity 
            WHERE datname = @databaseName 
            AND pid <> pg_backend_pid();";

        await using var command = new NpgsqlCommand(terminateCommandText, connection);
        command.Parameters.AddWithValue("@databaseName", databaseName);

        var terminatedCount = await command.ExecuteNonQueryAsync();
        Console.WriteLine($"Terminated {terminatedCount} connections to database {databaseName}");
    }

    private string EscapeArgument(string argument)
    {
        if (string.IsNullOrEmpty(argument))
            return argument;

        // For Windows, we need different escaping
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // Windows command line escaping
            if (argument.Contains(" ") || argument.Contains("\""))
            {
                argument = argument.Replace("\"", "\\\"");
                return $"\"{argument}\"";
            }
            return argument;
        }
        else
        {
            // Linux/Unix escaping
            if (argument.Contains(" ") || argument.Contains("\"") || argument.Contains("'"))
            {
                argument = argument.Replace("'", "'\\''");
                return $"'{argument}'";
            }
            return argument;
        }
    }

    private string? FindExecutable(string executableName)
    {
        // Remove any extension for consistency
        string baseExecutableName = Path.GetFileNameWithoutExtension(executableName);
        
        // Platform-specific executable extensions
        bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        
        // First, check PATH
        var pathDirs = Environment.GetEnvironmentVariable("PATH")?.Split(Path.PathSeparator);
        
        if (pathDirs != null)
        {
            var extensions = isWindows
                ? new[] { ".exe", ".cmd", ".bat", "" }
                : new[] { "" };

            foreach (var dir in pathDirs)
            {
                if (string.IsNullOrWhiteSpace(dir)) continue;
                
                foreach (var ext in extensions)
                {
                    try
                    {
                        var potentialPath = Path.Combine(dir.Trim(), baseExecutableName + ext);
                        if (File.Exists(potentialPath))
                            return potentialPath;
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }

        // Platform-specific locations
        if (isWindows)
        {
            return FindExecutableOnWindows(baseExecutableName);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return FindExecutableOnLinux(baseExecutableName);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return FindExecutableOnMacOS(baseExecutableName);
        }

        return null;
    }

    private string? FindExecutableOnWindows(string baseExecutableName)
    {
        var programFilesPaths = new[]
        {
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
        };

        var versions = new[] { "18", "17", "16", "15", "14", "13", "12", "11", "10" };

        foreach (var programFiles in programFilesPaths)
        {
            if (string.IsNullOrEmpty(programFiles)) continue;
            
            foreach (var version in versions)
            {
                var paths = new[]
                {
                    Path.Combine(programFiles, "PostgreSQL", version, "bin", baseExecutableName + ".exe"),
                    Path.Combine(programFiles, "edb", "postgresql" + version, "bin", baseExecutableName + ".exe")
                };

                foreach (var path in paths)
                {
                    if (File.Exists(path))
                        return path;
                }
            }
        }

        return null;
    }

    private string? FindExecutableOnLinux(string baseExecutableName)
    {
        var linuxPaths = new[]
        {
            $"/usr/bin/{baseExecutableName}",
            $"/usr/local/bin/{baseExecutableName}",
            $"/usr/lib/postgresql/16/bin/{baseExecutableName}",
            $"/usr/lib/postgresql/15/bin/{baseExecutableName}",
            $"/usr/lib/postgresql/14/bin/{baseExecutableName}",
            $"/usr/lib/postgresql/13/bin/{baseExecutableName}",
            $"/usr/lib/postgresql/12/bin/{baseExecutableName}",
            $"/usr/pgsql-16/bin/{baseExecutableName}",
            $"/usr/pgsql-15/bin/{baseExecutableName}",
            $"/usr/pgsql-14/bin/{baseExecutableName}",
            $"/opt/postgresql/bin/{baseExecutableName}"
        };

        foreach (var path in linuxPaths)
        {
            if (File.Exists(path))
                return path;
        }

        // Try using 'which' command
        try
        {
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "which",
                    Arguments = baseExecutableName,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            string output = process.StandardOutput.ReadToEnd().Trim();
            process.WaitForExit(1000);
            
            if (process.ExitCode == 0 && !string.IsNullOrEmpty(output) && File.Exists(output))
                return output;
        }
        catch
        {
            // Ignore errors
        }

        return null;
    }

    private string? FindExecutableOnMacOS(string baseExecutableName)
    {
        var macPaths = new[]
        {
            $"/usr/local/bin/{baseExecutableName}",
            $"/opt/homebrew/bin/{baseExecutableName}",
            $"/Applications/Postgres.app/Contents/Versions/latest/bin/{baseExecutableName}",
            $"/Library/PostgreSQL/16/bin/{baseExecutableName}",
            $"/Library/PostgreSQL/15/bin/{baseExecutableName}",
            $"/Library/PostgreSQL/14/bin/{baseExecutableName}"
        };

        foreach (var path in macPaths)
        {
            if (File.Exists(path))
                return path;
        }

        // Try 'which' on macOS
        try
        {
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "which",
                    Arguments = baseExecutableName,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            string output = process.StandardOutput.ReadToEnd().Trim();
            process.WaitForExit(1000);
            
            if (process.ExitCode == 0 && !string.IsNullOrEmpty(output) && File.Exists(output))
                return output;
        }
        catch
        {
            // Ignore errors
        }

        return null;
    }
}

// Update the DTO to include IsDirectory property
