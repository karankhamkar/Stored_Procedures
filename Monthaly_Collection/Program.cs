ALTER PROC MonthlyCollectionReport
(
    @startYear INT = NULL,
    @endYear INT = NULL
)
AS
BEGIN
    BEGIN TRY
        SELECT
            CONCAT(CUST.FIRSTAME, ' ', CUST.LASTNAME) AS 'NAME OF CUSTOMER',
            AR.AREA,
            AG.AGENTNAME,
            (SELECT COUNT(*)
             FROM Complaint Comp 
             WHERE Comp.CustomerId = CUST.CustomerId) AS 'Total Raised Complaints',
            M.MONTHNAME,
            MC.YEAR,
            PKG.PackageName,
            PY.PaymentMode
        FROM
            CUSTOMERS CUST 
        JOIN AREA AR ON CUST.AREAID = AR.AREAID
        JOIN AGENTS AG ON AR.AGENTID = AG.AGENTID
        JOIN MonthlyCollection MC ON MC.PackageId = CUST.PACKAGEID
        JOIN MONTHS M ON M.MONTHID = MC.MONTHID
        JOIN PAYMENTHMODE PY ON PY.PAYMENTMODEID = MC.PAYMENTMODEID
        JOIN PACKAGES PKG ON PKG.PACKAGEID = CUST.PACKAGEID
        WHERE (@startYear IS NULL OR MC.Year >= @startYear)
          AND(@endYear IS NULL OR MC.Year <= @endYear)
        ORDER BY
            CONCAT(CUST.FIRSTAME, ' ', CUST.LASTNAME),
            MC.Year,
            M.MonthId
    END TRY
    BEGIN CATCH
        THROW;
END CATCH
END