

************************

*: In case of using sqlLite:

Navigate to Migrators.SqLite project path

D:\TrdBx\src\Migrators\Migrators.SqLite

cd D:\TrdBx\src\Migrators\Migrators.SqLite

Create Initial Migration

dotnet ef --startup-project D:/TrdBx/src/Server.UI/ migrations add DatabaseUpdate-001 --context ApplicationDbContext -o D:\TrdBx\src\Migrators\Migrators.SqLite\Migrations


dotnet tool update --global dotnet-ef
dotnet tool install --global dotnet-ef
dotnet tool update dotnet-ef
dotnet ef --version
dotnet tool update dotnet-ef


*: In case of using PostgreSQL:

Navigate to Migrators.PostgreSQL project path

D:\TrdBx\src\Migrators\Migrators.PostgreSQL

cd D:\TrdBx\src\Migrators\Migrators.PostgreSQL

Create Initial Migration

dotnet ef --startup-project D:/TrdBx/src/Server.UI/ migrations add DatabaseUpdate-001 --context ApplicationDbContext -o D:\TrdBx\src\Migrators\Migrators.PostgreSQL\Migrations



mohammed@MyThinkBook:~/Projects/TrdBx/src/Migrators/Migrators.PostgreSQL$ dotnet ef migrations add DescUpdated   --project /home/mohammed/Projects/TrdBx/src/Migrators/Migrators.PostgreSQL   --startup-project /home/mohammed/Projects/TrdBx/src/Server.UI   --context ApplicationDbContext   --output-dir Migrations


List existing migrations:
dotnet ef migrations list --project /home/mohammed/Projects/TrdBx/src/Migrators/Migrators.PostgreSQL --startup-project /home/mohammed/Projects/TrdBx/src/Server.UI --context ApplicationDbContext

Generate SQL script (without applying):
dotnet ef migrations script --project /home/mohammed/Projects/TrdBx/src/Migrators/Migrators.PostgreSQL --startup-project /home/mohammed/Projects/TrdBx/src/Server.UI --context ApplicationDbContext

Remove last migration (if needed):
dotnet ef migrations remove --project /home/mohammed/Projects/TrdBx/src/Migrators/Migrators.PostgreSQL --startup-project /home/mohammed/Projects/TrdBx/src/Server.UI --context ApplicationDbContext




Step 1: Backup your database (important!)

pg_dump -U your_username -h localhost your_database_name > backup_before_migration_reset.sql
pg_dump -U postgres -h localhost TrdBxPostgres > backup_before_migration_resetx.sql


Step 2: Remove all old migration files from your project

# Navigate to your migrations folder
cd /home/mohammed/Projects/TrdBx/src/Migrators/Migrators.PostgreSQL/Migrations

# Delete all migration files except the MigrationDesignerSnapshot file
rm 202*.cs

Step 3: Remove the migration history from the database

-- Connect to your database and run:
DELETE FROM "__EFMigrationsHistory";

Or via command line:
psql -U your_username -d your_database_name -c "DELETE FROM \"__EFMigrationsHistory\";"

Step 4: Create a new baseline migration

dotnet ef migrations add InitialCreate --project /home/mohammed/Projects/TrdBx/src/Migrators/Migrators.PostgreSQL --startup-project /home/mohammed/Projects/TrdBx/src/Server.UI --context ApplicationDbContext --output-dir Migrations

Step 5: Generate the SQL script (to see what will happen)

dotnet ef migrations script --project /home/mohammed/Projects/TrdBx/src/Migrators/Migrators.PostgreSQL --startup-project /home/mohammed/Projects/TrdBx/src/Server.UI --context ApplicationDbContext

Step 6: Apply the migration to the database (without making actual schema changes)

Since your database already has the schema, you need to add the migration record without applying schema changes:

Option A - If the migration contains only the changes you want:
bash

dotnet ef database update --project /home/mohammed/Projects/TrdBx/src/Migrators/Migrators.PostgreSQL --startup-project /home/mohammed/Projects/TrdBx/src/Server.UI --context ApplicationDbContext

Option B - If the migration tries to create existing tables (most likely):
Add the migration to history without executing SQL:
bash

# Add a dummy migration record
psql -U your_username -d your_database_name -c "INSERT INTO \"__EFMigrationsHistory\" (\"MigrationId\", \"ProductVersion\") VALUES ('20260603120000_InitialCreate', '10.0.0');"

Step 7: Verify the setup

# List migrations to confirm only one exists
dotnet ef migrations list --project /home/mohammed/Projects/TrdBx/src/Migrators/Migrators.PostgreSQL --startup-project /home/mohammed/Projects/TrdBx/src/Server.UI --context ApplicationDbContext

# Get current migration
dotnet ef migrations list --project /home/mohammed/Projects/TrdBx/src/Migrators/Migrators.PostgreSQL --startup-project /home/mohammed/Projects/TrdBx/src/Server.UI --context ApplicationDbContext --connection "your_connection_string"