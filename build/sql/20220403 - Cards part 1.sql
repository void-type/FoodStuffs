-- Migration process:
-- Run part 1
-- Export Recipes table as csv
-- Run ps1 to migrate ingredients
-- Run the generated sql script
-- Run part 2

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('dbo.Ingredient', 'U') IS NOT NULL DROP TABLE [dbo].[Ingredient]
GO

CREATE TABLE [dbo].[Ingredient]
(
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](max) NOT NULL,
    [Quantity] [int] NOT NULL,
    [Order] [int] NOT NULL,
    [IsCategory] [bit] NOT NULL,
    [CreatedBy] [nvarchar](max) NOT NULL,
    [CreatedOn] [datetime] NOT NULL,
    [ModifiedBy] [nvarchar](max) NOT NULL,
    [ModifiedOn] [datetime] NOT NULL,
    [RecipeId] [int] NOT NULL,
    CONSTRAINT [PK_Ingredient] PRIMARY KEY CLUSTERED
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Ingredient] WITH CHECK ADD CONSTRAINT [FK_Ingredient_Recipe] FOREIGN KEY([RecipeId]) REFERENCES [dbo].[Recipe] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Ingredient] CHECK CONSTRAINT [FK_Ingredient_Recipe]
GO

-- Update other constraints with cascade delete
ALTER TABLE [Blob] DROP CONSTRAINT FK_Blob_Image
ALTER TABLE [Blob] WITH CHECK ADD CONSTRAINT FK_Blob_Image FOREIGN KEY (Id) REFERENCES [Image] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [Image] DROP CONSTRAINT FK_Image_Recipe
ALTER TABLE [Image] WITH CHECK ADD CONSTRAINT FK_Image_Recipe FOREIGN KEY (RecipeId) REFERENCES [Recipe] (Id) ON DELETE CASCADE
GO

ALTER TABLE [CategoryRecipe] DROP CONSTRAINT FK_CategoryRecipe_Category
ALTER TABLE [CategoryRecipe] WITH CHECK ADD CONSTRAINT [FK_CategoryRecipe_Category] FOREIGN KEY([CategoryId]) REFERENCES [Category] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [CategoryRecipe] DROP CONSTRAINT FK_CategoryRecipe_Recipe
ALTER TABLE [CategoryRecipe] WITH CHECK ADD CONSTRAINT [FK_CategoryRecipe_Recipe] FOREIGN KEY([RecipeId]) REFERENCES [Recipe] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Recipe] ADD IsForMealPlanning [bit] NULL
GO

UPDATE [dbo].[Recipe] SET IsForMealPlanning = 0
GO

ALTER TABLE [dbo].[Recipe] ALTER COLUMN IsForMealPlanning [bit] NOT NULL
GO

SET ANSI_PADDING OFF
GO
