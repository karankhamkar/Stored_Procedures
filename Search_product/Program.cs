using System.Text.RegularExpressions;
using System.Xml.Linq;

ALTER PROC SEARCH_PRODUCT
(
	@PRODUCTID INT = NULL
)
AS
BEGIN
	BEGIN TRY
		SELECT 
			Distinct P.ProductName,
			P.ManufactureDate,
			P.IsActive,
			STRING_AGG( C.CATEGORYNAME, ', ')AS Categories,
			(SELECT

                DISTINCT SP.[NAME] AS '@KEY',
                 SP.[VALUE] AS '@value'

             FROM

                 Specifications SP

             WHERE

                 SP.[PRODUCTID] = P.[PRODUCTID]

             FOR XML PATH('SPECIFICATION')) AS SPECIFICATIONS
		FROM 
			Products P 
			INNER JOIN ProductCategories PC ON PC.ProductID = P.ProductID
			INNER JOIN Categories C  ON PC.CategoryID = C.CategoryID
		WHERE 
			(@PRODUCTID IS NULL OR P.ProductID = @PRODUCTID)
		GROUP BY 
			P.ProductID,
            P.ProductName,
            P.ManufactureDate,
            P.IsActive
		
	END TRY
	BEGIN CATCH
		THROW
	END CATCH
END