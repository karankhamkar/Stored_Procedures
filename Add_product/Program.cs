using System.Collections.Generic;
using System.Xml.Linq;

ALTER PROCEDURE InsertProduct
(
    @productName VARCHAR(50),
    @categories VARCHAR(50),
    @manufactureDate DATE,
    @IsActive BIT,
    @specifications varchar(500)
)
AS
BEGIN
  --SET NOCOUNT ON
	BEGIN TRY
		
		INSERT INTO Products (ProductName, ManufactureDate, IsActive)
		VALUES(
            @productName,
            Convert(DATE, @manufactureDate),
            @IsActive)


        DECLARE @productID INT
		SET @productID = SCOPE_IDENTITY()
		
		DECLARE @categoryList TABLE (CategoryID INT)
		INSERT INTO @categoryList (CategoryID)
		SELECT CONVERT(INT, value) AS CategoryID FROM STRING_SPLIT(@categories, ',')

		INSERT INTO ProductCategories (ProductID, CategoryID)
		SELECT @productID, CategoryID FROM @categoryList

		DECLARE @ProductCategoryID INT
		SET @ProductCategoryID = SCOPE_IDENTITY()

		DECLARE @xml XML
		SET @xml = CAST(@specifications AS XML)

		INSERT INTO Specifications (ProductID, [Name],[Value])
		SELECT @productID,
        a.value('@NAME[1]','varchar(50)') AS[Name],
		a.value('@VALUE[1]', 'varchar(50)') AS[Value]

        FROM @xml.nodes('/SPECIFICATION') AS xmldata(a)


        DECLARE @SpecificationID INT
		SET @SpecificationID = SCOPE_IDENTITY()
	
	END TRY
	BEGIN CATCH
		THROW
	END CATCH
END