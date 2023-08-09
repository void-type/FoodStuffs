-- This script will clean up duplicate categories created prior to v9.1

UPDATE CategoryRecipe SET CategoryID = (
    SELECT TOP(1)
    nc.Id
FROM Category nc
WHERE nc.Name = c.Name )
FROM CategoryRecipe cr
    INNER JOIN Category c ON cr.CategoryId = c.Id

DELETE
FROM Category
WHERE NOT EXISTS (
    SELECT Id
    FROM CategoryRecipe cr
    WHERE cr.CategoryId = Id
)
