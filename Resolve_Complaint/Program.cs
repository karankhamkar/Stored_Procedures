using System.Collections.Generic;
using System;

ALTER PROCEDURE Resolved_Complaint_With_ExpenseDetails
(
	@ComplaintId INT,
    @CustomerId INT,
    @ResolvedByAgent INT,
    @Description varchar(50),
	@Expense XML
)
AS
BEGIN
	BEGIN TRY
		IF NOT EXISTS(SELECT CustomerId FROM Complaint WHERE CustomerId = @CustomerId)
		BEGIN
            RAISERROR('CUSTOMER WHICH WAS NOT RAISED ANY COMPLAINT',16,1)

            RETURN
        END	

		INSERT INTO  [dbo].[Complaint] ([ResolvedByAgentID],[ResolvedDate])

        VALUES(
             @ResolvedByAgent,
Convert(Datetime, GETDATE())
            )


        INSERT INTO[dbo].[DetailExpenses]
(ComplaintId,[Description], ExpenseId, Amount)

        SELECT
            @ComplaintId,
            @Description,
			expense.value('@ExpenseID', 'VARCHAR(50)') AS ExpenseID,
            CONVERT(DECIMAL(10, 2), expense.value('(./text())[1]', 'VARCHAR(50)')) AS Amount
		FROM @Expense.nodes('//Expenses/Expense') AS xmldata(expense)


    END TRY 
	BEGIN CATCH
		THROW
	END CATCH
END

DECLARE @xml xml
SET @xml = '<Expenses>
<Expense ExpenseID ="3">1800</Expense>
<Expense ExpenseID ="4">800</Expense>
</Expenses>'
EXEC Resolved_Complaint_With_ExpenseDetails 2006,2002,2,'Manpower and tools', @xml

