

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
