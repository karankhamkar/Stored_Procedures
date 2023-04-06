
using System.Collections.Generic;

ALTER PROC Raise_Complaint
(
	@CUSTOMERID INT,
    @DESCRIPTION VARCHAR(100)
)
AS
BEGIN
	DECLARE @ComplaintID INT

	IF NOT EXISTS (SELECT CUSTOMERID FROM CUSTOMERS WHERE CUSTOMERID = @CUSTOMERID )
	BEGIN
        RAISERROR('Invalid CustomerID', 16, 1)

        RETURN
    END

	INSERT INTO Complaint (CustomerID, [Description], RaiseDate)
		VALUES(
            @CUSTOMERID,
            @DESCRIPTION,
GETDATE()
        )


    SET @ComplaintID = SCOPE_IDENTITY()
    --SELECT @ComplaintID AS ComplaintID
END