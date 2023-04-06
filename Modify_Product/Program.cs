using System.Collections.Generic;
using System.Xml.Linq;

ALTER PROC Update_Product
(	
	@ProductID INT,
    @ProductName VARCHAR(50),
	@ManufactureDate DATE,
    @IsActive BIT,
    @CategoryID VARCHAR(50),
	@Specification XML
)
AS
BEGIN
	BEGIN TRY
		UPDATE
			Products 
		SET
			ProductName = @ProductName,
            ManufactureDate = @ManufactureDate,
            IsActive = @IsActive
		WHERE
			ProductID = @ProductID
---------------------------------------------------------------------------
		DELETE FROM [dbo].[PRoductCategories]
WHERE
            PRODUCTID = @PRODUCTID
		
		DECLARE @categoryList TABLE (CategoryID INT)
		INSERT INTO @categoryList (CategoryID)
		SELECT CONVERT(INT, value) AS CategoryID FROM STRING_SPLIT(@CategoryID, ',')

		INSERT INTO[PRoductCategories] (ProductID, CategoryID)
		SELECT
            @PRODUCTID, CategoryID
		FROM 
			@categoryList
-----------------------------------------------------------------------
		DELETE FROM SPECIFICATIONS 
		WHERE 
			PRODUCTID = @PRODUCTID

		INSERT INTO Specifications (ProductID, [Name],[Value])
		SELECT
            @productID,
			a.value('@NAME[1]','varchar(50)') AS[Name],
			a.value('@VALUE[1]', 'varchar(50)') AS[Value]

        FROM
            @Specification.nodes('/SPECIFICATION') AS xmldata(a)


    END TRY
	BEGIN CATCH
		THROW
	END CATCH
END

