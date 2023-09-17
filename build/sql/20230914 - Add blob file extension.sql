-- Infer file extension from magic bytes.
-- https://en.wikipedia.org/wiki/List_of_file_signatures
UPDATE [Image] SET
    [FileName] = LOWER(CAST(NEWID() AS nvarchar(36))) + CASE SUBSTRING(Bytes, 0, 3)
        WHEN 0xFFD8 THEN '.jpg'
        WHEN 0x8950 THEN '.png'
        WHEN 0x5249 THEN '.webp'
    END
FROM [Image] i
    INNER JOIN [Blob] b ON i.Id = b.Id

SELECT TOP(100)
    *
FROM [Image]
ORDER BY [FileName]
