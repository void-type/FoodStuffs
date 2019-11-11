-- This script will add image support and update all date columns to datetime2

ALTER TABLE [dbo].[Recipe] ALTER column CreatedOn datetime2 NOT NULL
GO
ALTER TABLE [dbo].[Recipe] ALTER column ModifiedOn datetime2 NOT NULL
GO

ALTER TABLE [dbo].[Blob] DROP CONSTRAINT FK_Blob_Image
GO
ALTER TABLE [dbo].[Image] DROP CONSTRAINT FK_Image_Recipe
GO

IF OBJECT_ID('dbo.Image', 'U') IS NOT NULL DROP TABLE [dbo].[Image]
IF OBJECT_ID('dbo.Blob', 'U') IS NOT NULL DROP TABLE [dbo].[Blob]

CREATE TABLE [dbo].[Image]
(
  [Id] [int] IDENTITY(1,1) NOT NULL,
  [CreatedBy] [nvarchar] (max) NOT NULL,
  [CreatedOn] [datetime2] NOT NULL,
  [ModifiedBy] [nvarchar] (max) NOT NULL,
  [ModifiedOn] [datetime2] NOT NULL,
  [RecipeId] [int] NOT NULL,
  CONSTRAINT [PK_Image] PRIMARY KEY CLUSTERED ([Id] ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
) ON [PRIMARY]

CREATE TABLE [dbo].[Blob]
(
  [Id] [int] NOT NULL,
  [Bytes] [varbinary](max) NOT NULL,
  CONSTRAINT [PK_Blob] PRIMARY KEY CLUSTERED ([Id] ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
) ON [PRIMARY]

ALTER TABLE [Blob] ADD CONSTRAINT FK_Blob_Image FOREIGN KEY (Id) REFERENCES [Image] (Id) ON DELETE CASCADE
ALTER TABLE [Blob] CHECK CONSTRAINT FK_Blob_Image
GO

ALTER TABLE [Image] ADD CONSTRAINT FK_Image_Recipe FOREIGN KEY (RecipeId) REFERENCES [Recipe] (Id)
ALTER TABLE [Image] CHECK CONSTRAINT FK_Image_Recipe
GO
