Use master

Go

IF EXISTS (SELECT * FROM sys.databases WHERE name = N'FameMatchDB')

BEGIN

DROP DATABASE  FameMatchDB;

END

Go
Create Database FameMatchDB
Go
Use FameMatchDB
Go
Create Table Users
(
UserId int Primary Key Identity (1,1),
UserName nvarchar(50) Not Null,
UserLastName nvarchar(50) Not Null,
UserEmail nvarchar(50) Unique Not Null,
UserPassword nvarchar(50) Not Null,
IsManager bit Not Null Default 0,
UserGender nvarchar (50) Not Null,
IsReported bit Not Null Default 0,
IsBlocked bit Not Null Default 0
)

Create Table Castors
(
UserId INT PRIMARY KEY,
Foreign Key(UserId) REFERENCES Users(UserId) ,
CompanyName nvarchar (400) Not Null,
NumOfLisence int Not Null
)

Create Table Casted
(
UserId INT PRIMARY KEY,
Foreign Key(UserId) REFERENCES Users(UserId),
UserAge int Not Null,
UserLocation nvarchar(400) Not Null,
UserHigth int  Not Null,
UserHair nvarchar(50) Not Null,
UserEyes nvarchar (50) Not Null,
UserBody nvarchar(50)  Not Null,
UserSkin nvarchar(50)  Not Null,
AboutMe nvarchar (800) Not Null
)

Create Table Reporet
(
UserId  int Foreign Key References Users(UserId),
ReporetId int Primary Key Identity (1,1),
ReportedId  int Foreign Key References Users(UserId),
Content nvarchar (800) Not Null
)

Create Table Auditions
(
UserId  int Foreign Key References Castors(UserId),
AudId int Primary Key Identity (1,1),
Description nvarchar(800) Not Null,
AudAge int Not Null,
AudLocation nvarchar(400),
AudHigth int,
AudHair nvarchar(50),
AudEyes nvarchar (50),
UserBody nvarchar(50),
AudSkin nvarchar(50),
IsPublic bit Not Null Default 0
)

Create Table Message
(
SenderId  int Foreign Key References Users (UserId),
ReciverId  int Foreign Key References Users(UserId),
MessageId int Primary Key Identity (1,1),
Content nvarchar(800) Not Null,
MessageTime datetime Not Null
)

Create Table Tip
(
TipId INT PRIMARY KEY Identity(1,1),
UserId  int Foreign Key References Casted(UserId),
TipLevel int  Not Null,
Question nvarchar(800) Not Null,
Answer1 nvarchar(800) Not Null,
Answer2 nvarchar(800) Not Null,
Answer3 nvarchar(800) Not Null,
Answer4 nvarchar(800) Not Null
)

Create Table Files
(
FileId Int Primary Key Identity(1,1),
FileExt Nvarchar(50) Not Null
)

Create Table Pictures
(
UserId  int Primary Key References Users(UserId),
FileId int Foreign Key References Files(FileId)
)

CREATE LOGIN [FameMatchAdminLogin] WITH PASSWORD ='ori1geva2';

Go

-- Create a user in the FameMatchDB database for the login

CREATE USER [FameMatchAdminUser] FOR LOGIN [FameMatchAdminLogin];

Go

-- Add the user to the db_owner role to grant admin privileges

ALTER ROLE db_owner ADD MEMBER [FameMatchAdminUser];

Go

insert into Users(UserName, UserLastName, UserEmail, UserPassword,IsManager,UserGender,IsReported,IsBlocked) 
values('Ori', 'Geva', 'geva.ori1@gmail.com','ori1geva2$!',1,'male',0,0)

select*from Users
--scaffold-DbContext "Server = (localdb)\MSSQLLocalDB;Initial Catalog=FameMatchDB;User ID=FameMatchAdminLogin;Password=ori1geva2;" Microsoft.EntityFrameworkCore.SqlServer -OutPutDir Models -Context FameMatchDbContext -DataAnnotations –force