-- Requirements: An target database and sql server login.
-- This script will create the necessary tables and map the login to the db user.
-- This script will destroy any data within the database and reset the databse to a new condition.

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- Set the database name here
USE [FoodStuffsTest]
GO

-- Set the login here
DECLARE	@login varchar(100) = 'FoodStuffsTestUser';

EXEC sp_dropuser @login;
EXEC sp_adduser @login, @login, 'db_datareader';
EXEC sp_addrolemember 'db_datawriter', @login;
GO

ALTER TABLE [dbo].[CategoryRecipe] DROP CONSTRAINT [FK_CategoryRecipe_Recipe]
GO

ALTER TABLE [dbo].[CategoryRecipe] DROP CONSTRAINT [FK_CategoryRecipe_Category]
GO

ALTER TABLE [dbo].[Recipe] DROP CONSTRAINT [FK_Recipe_UserModified]
GO

ALTER TABLE [dbo].[Recipe] DROP CONSTRAINT [FK_Recipe_UserCreated]
GO

IF OBJECT_ID('dbo.Category', 'U') IS NOT NULL DROP TABLE [dbo].[Category]
IF OBJECT_ID('dbo.CategoryRecipe', 'U') IS NOT NULL DROP TABLE [dbo].[CategoryRecipe]
IF OBJECT_ID('dbo.Recipe', 'U') IS NOT NULL DROP TABLE [dbo].[Recipe]
IF OBJECT_ID('dbo.User', 'U') IS NOT NULL DROP TABLE [dbo].[User]
GO

CREATE TABLE [dbo].[CategoryRecipe](
    [RecipeId] [int] NOT NULL,
    [CategoryId] [int] NOT NULL,
 CONSTRAINT [PK_CategoryRecipe] PRIMARY KEY CLUSTERED 
(
    [RecipeId] ASC,
    [CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Recipe](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](max) NOT NULL,
    [Directions] [nvarchar](max) NOT NULL,
    [Ingredients] [nvarchar](max) NOT NULL,
    [PrepTimeMinutes] [int] NULL,
    [CookTimeMinutes] [int] NULL,
    [CreatedByUserId] [int] NOT NULL,
    [ModifiedByUserId] [int] NOT NULL,
    [CreatedOn] [datetime] NOT NULL,
    [ModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_Recipe] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[Category](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[User](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [UserName] [nvarchar](max) NOT NULL,
    [Password] [char](128) NOT NULL,
    [IsAdmin] [bit] NOT NULL,
    [Salt] [nvarchar](max) NOT NULL,
    [FirstName] [nvarchar](max) NOT NULL,
    [LastName] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_AppUser] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

-- TODO: Remove this once users are implemented
INSERT INTO [dbo].[User]
           ([UserName]
           ,[Password]
           ,[IsAdmin]
           ,[Salt]
           ,[FirstName]
           ,[LastName])
     VALUES
           ('FirstUser'
           ,'80c9cc7840dc718ed2e85018daf13d7596a83df289baeec61748ade36b54cf93'
           ,1
           ,'d55c0a75cd34955bced4210f203dfe77f5b296892c2e786c091325d329c2482e'
           ,'First'
           ,'User')
GO

ALTER TABLE [dbo].[CategoryRecipe]  WITH CHECK ADD  CONSTRAINT [FK_CategoryRecipe_Category] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([Id])
GO

ALTER TABLE [dbo].[CategoryRecipe] CHECK CONSTRAINT [FK_CategoryRecipe_Category]
GO

ALTER TABLE [dbo].[CategoryRecipe]  WITH CHECK ADD  CONSTRAINT [FK_CategoryRecipe_Recipe] FOREIGN KEY([RecipeId])
REFERENCES [dbo].[Recipe] ([Id])
GO

ALTER TABLE [dbo].[CategoryRecipe] CHECK CONSTRAINT [FK_CategoryRecipe_Recipe]
GO

ALTER TABLE [dbo].[Recipe]  WITH CHECK ADD  CONSTRAINT [FK_Recipe_UserCreated] FOREIGN KEY([CreatedByUserId])
REFERENCES [dbo].[User] ([Id])
GO

ALTER TABLE [dbo].[Recipe] CHECK CONSTRAINT [FK_Recipe_UserCreated]
GO

ALTER TABLE [dbo].[Recipe]  WITH CHECK ADD  CONSTRAINT [FK_Recipe_UserModified] FOREIGN KEY([ModifiedByUserId])
REFERENCES [dbo].[User] ([Id])
GO

ALTER TABLE [dbo].[Recipe] CHECK CONSTRAINT [FK_Recipe_UserModified]
GO

SET ANSI_PADDING OFF
GO