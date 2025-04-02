
Use master

Go

IF EXISTS (SELECT * FROM sys.databases WHERE name = N'FameMatchDB')

BEGIN

DROP DATABASE  FameMatchDB;

END

Go
Create Database FameMatchDB
Go
use FameMatchDB
Go
CREATE LOGIN [FameMatchAdminLogin] WITH PASSWORD ='ori1geva2';

Go

-- Create a user in the FameMatchDB database for the login

CREATE USER [FameMatchAdminUser] FOR LOGIN [FameMatchAdminLogin];

Go

-- Add the user to the db_owner role to grant admin privileges

ALTER ROLE db_owner ADD MEMBER [FameMatchAdminUser];

Go

ALTER SERVER ROLE sysadmin ADD MEMBER [FameMatchAdminLogin];
Go

use master
Go
--scaffold-DbContext "Server = (localdb)\MSSQLLocalDB;Initial Catalog=FameMatchDB;User ID=FameMatchAdminLogin;Password=ori1geva2;" Microsoft.EntityFrameworkCore.SqlServer -OutPutDir Models -Context FameMatchDbContext -DataAnnotations –force

select*from Users