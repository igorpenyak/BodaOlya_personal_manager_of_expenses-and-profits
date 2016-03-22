USE ExpensesManager
GO

CREATE PROCEDURE spAddNewCategory
@Name NVARCHAR(50),
@Id INT OUT
AS
BEGIN TRAN
	BEGIN TRY
	DECLARE @Temp TABLE(ID int NOT NULL);

		INSERT INTO tblCategory(Name)
		OUTPUT INSERTED.Id INTO @Temp
		VALUES(@Name)
		SELECT @Id = ID FROM @Temp
		COMMIT TRAN
	END TRY
	
	BEGIN CATCH
		ROLLBACK TRAN 
	END CATCH

GO

CREATE PROCEDURE spAddNewCreditCard
@Number NUMERIC(16,0),
@Type NVARCHAR(50),
@DateOff DATETIME,
@Balance NUMERIC(18,2),
@UserId INT,
@Id INT OUT
AS
BEGIN TRAN
	BEGIN TRY
	DECLARE @Temp TABLE(ID int NOT NULL);
	DECLARE @ExistNumber NUMERIC(16,0)
	SELECT @ExistNumber = c.Number FROM tblCreditCard c
	WHERE @Number = c.Number
	IF @ExistNumber IS NULL
	BEGIN
		INSERT INTO tblCreditCard
		(
		Number,
		[Type],
		DateOf,
		Balance,
		UserId 
		)
		OUTPUT INSERTED.Id INTO @Temp
		VALUES(
		    @Number,
			@Type,
			@DateOff,
			@Balance,
			@UserId)
		SELECT @Id = ID FROM @Temp
	  END
	  ELSE 
		BEGIN
			SET @Id = -1
		END
		COMMIT TRAN
	END TRY
	
	BEGIN CATCH
		ROLLBACK TRAN 
	END CATCH

GO

CREATE PROCEDURE spSelectUserByLogin
@Login VARCHAR(20),
@Password VARCHAR(100)
AS
	BEGIN
	SELECT 
		Id, 
		FirstName, 
		LastName, 
		[Login], 
		[IsDisable] 
	FROM tblUsers
	WHERE [Login] = @Login and [Password] = @Password and [IsDisable] <> 1;

END;
GO
CREATE PROCEDURE spAddNewTransaction
@Name NVARCHAR(100),
@Cost NUMERIC(18,2),
@CreationDate DATETIME,
@Category NVARCHAR(50),
@RepetableType NVARCHAR(50),
@TransactionType NVARCHAR(50),
@CreditCard NVARCHAR(50),
@UserId INT,
@Id INT OUT

AS

	BEGIN TRAN

		BEGIN TRY
			DECLARE @TransactionName NVARCHAR(100)
			SELECT @TransactionName = t.Name FROM tblTransactions t
			WHERE t.Name = @Name;
		
			
			BEGIN
				DECLARE
				@CreditCardId INT,
				@CategoryId INT,
				@TypeId INT,
				@RepeatableTypeId INT,
				@Status BIT
				DECLARE @Temp TABLE(ID int NOT NULL);
	
		
				SELECT @CreditCardId = cc.Id FROM tblCreditCard cc
				WHERE cc.Type +'  ('+ CAST(cc.Number as NVARCHAR(16))+')' = @CreditCard

				SELECT @CategoryId = c.Id FROM tblCategory c
				WHERE c.Name = @Category

				SELECT @TypeId = tt.Id FROM tblTransactionType tt
				WHERE tt.Name = @TransactionType

				SELECT @RepeatableTypeId = rt.Id FROM tblRepeatableExpensesType rt
				WHERE rt.Name = @RepetableType
		
				IF @RepeatableTypeId = 1
					Set @Status = 0
				
				ELSE 
					Set @Status = 1
		
				INSERT INTO tblTransactions
					(
					Name,
					Cost,
					CreditCardId,
					CategoryId,
					CreationDate,
					TypeId,
					TransactionStatus,
					RepeatableTypeId,
					LastRepeatDate,
					UserId
					) 
					OUTPUT INSERTED.Id INTO @Temp
					VALUES
					(
					@Name,
					@Cost,
					@CreditCardId,
					@CategoryId,
					@CreationDate,
					@TypeId,
					@Status,
					@RepeatableTypeId,
					@CreationDate,
					@UserId
					)
					
					IF @TransactionType = 'Expense'
						BEGIN
							UPDATE tblCreditCard
							SET Balance = Balance - @Cost
							WHERE Id =@CreditCardId
						END
					IF @TransactionType = 'Income'
						BEGIN
							UPDATE tblCreditCard
							SET Balance = Balance + @Cost
							WHERE Id =@CreditCardId
						END
					SELECT @Id = ID FROM @Temp;

					
			END
			
			COMMIT TRAN
		END TRY

		BEGIN CATCH
			ROLLBACK TRAN
		END CATCH

GO

CREATE PROCEDURE spSelectAllTransactions
@TransactionType NVARCHAR(50),
@UserId INT
AS
	BEGIN TRAN
		BEGIN TRY
		DECLARE @TypeId INT
		SELECT @TypeId = tt.Id FROM tblTransactionType tt
		WHERE tt.Name = @TransactionType
			SELECT * FROM vJoinAllTable t
 				WHERE t.UserId = @UserId and t.TypeId = @TypeId
			COMMIT TRAN
		END TRY

		BEGIN CATCH
			ROLLBACK TRAN
		END CATCH

GO

CREATE PROCEDURE spSelectTransactionByCategory
@CategoryName NVARCHAR(50),
@TransactionType NVARCHAR(50)
AS
BEGIN TRAN
	BEGIN TRY
		DECLARE @CategiryId INT, @TransactionTypeID INT;
		SELECT @CategiryId = sc.Id FROM tblCategory sc WHERE sc.[Name] = @CategoryName
		SELECT @TransactionTypeID = st.Id FROM tblTransactionType st WHERE st.[Name] = @TransactionType

			IF @CategiryId <> 0 AND @CategoryName != 'All'
			BEGIN
				SELECT * FROM vJoinAllTable t
 				WHERE t.CategoryId = @CategiryId AND @TransactionTypeID = t.TypeId
			END
			ELSE
				BEGIN
	
			    SELECT * FROM vJoinAllTable t
				WHERE @TransactionTypeID = t.TypeId
			END

	COMMIT TRAN
	END TRY
	BEGIN CATCH
	ROLLBACK TRAN
	END CATCH

GO

CREATE PROCEDURE spSelectUserCreditCards
@UserId INT
AS
BEGIN TRAN
	BEGIN TRY
		SELECT Id, Number, Type, DateOf, Balance, UserId  FROM tblCreditCard
		WHERE UserId = @UserId
		COMMIT TRAN
	END TRY
	BEGIN CATCH
	ROLLBACK TRAN
	END CATCH
GO

-------------------------
-----For Job

CREATE PROCEDURE spRepetableExpences
AS
BEGIN TRANSACTION
SELECT * INTO ##TEMPTRANS FROM [DBO].[tblTransactions]
WHERE [Transactionstatus] = 1


WHILE (EXISTS(SELECT TOP 1 * FROM ##Temptrans))
BEGIN
	DECLARE @CreationDate DATE, @RepeatableTypeId INT, @DAYS DATETIME,@ID INT, 
	@card int, @sum numeric(18,2), @Ptype int
	SELECT TOP 1
	@ID =ID, 
	@CREATIONDATE = [Lastrepeatdate], 
	@REPEATABLETYPEID = [REPEATABLETYPEID],
	@card = CreditCardId,
	@sum = cost,
	@ptype = typeid
	FROM ##Temptrans

	SELECT @DAYS =
	CASE
		WHEN [NAME] = 'WEEKLY' THEN Dateadd(dd, 7, @CreationDate)
		WHEN [NAME] = 'MONTHLY' THEN DATEADD(MM, 1, @CreationDate)
		WHEN [NAME] = 'YEARLY' THEN DATEADD(YY, 1, @CreationDate)
	END
		FROM tblRepeatableExpensesType WHERE Id = @RepeatableTypeId

PRINT (@days)

IF(CAST(@days AS DATE)= CAST (GETDATE()AS DATE))
	BEGIN
	
	INSERT INTO tblTransactions
	SELECT
	Name,
	Cost,
	CreditcardId,
	CategoryId,
	CreationDate,
	TypeId,
	TransactionStatus,
	RepeatabletypeId,
	GETDATE(),
	UserId
    FROM Tbltransactions WHERE ID = @ID  

	
	UPDATE 
	tblTransactions
	SET TransactionStatus = 0
	 WHERE ID = @ID 
	 END 
	 update [dbo].[tblCreditCard] set Balance =
	 CASE
	 WHEN @ptype = 1 THEN
	 [Balance] + @sum
	 WHEN @ptype = 2 THEN
	 [Balance] - @sum
	 ELSE Balance
	 END

	DELETE FROM ##Temptrans WHERE ID = @ID
END
COMMIT TRAN
DROP TABLE ##temptrans
GO

CREATE PROCEDURE spGetTranByCardAndDate
@DateFrom DATETIME,
@DateTo DATETIME,
@CardNum NUMERIC(16, 0),
@TransactionType NVARCHAR(50)
AS
BEGIN TRAN
	BEGIN TRY
		DECLARE  @TransactionTypeID INT, @CardId Int;
		SELECT @TransactionTypeID = st.Id FROM tblTransactionType st WHERE st.[Name] = @TransactionType;
		SELECT @CardId = cc.Id FROM tblCreditCard cc WHERE cc.Number = @CardNum;
				SELECT * FROM vJoinAllTable t
 				WHERE CONVERT(DATE,t.CreationDate,101) >= CONVERT(DATE,@DateFrom,101) AND 
				CONVERT(DATE,t.CreationDate,101) <= CONVERT(DATE,@DateTo,101) AND 
				@TransactionTypeID = t.TypeId AND @CardId = t.CreditCardId 
	COMMIT TRAN
	END TRY
	BEGIN CATCH
	ROLLBACK TRAN
	END CATCH

GO


CREATE PROCEDURE spGetTransactionsByDate
@Date DATETIME,
@TransactionType NVARCHAR(50)
AS
BEGIN TRAN
	BEGIN TRY
		DECLARE  @TransactionTypeID INT;
		SELECT @TransactionTypeID = st.Id FROM tblTransactionType st WHERE st.[Name] = @TransactionType
			SELECT * FROM vJoinAllTable t
 				WHERE CONVERT(DATE,t.CreationDate,101) = CONVERT(DATE,@Date,101) AND @TransactionTypeID = t.TypeId
	COMMIT TRAN
	END TRY
	BEGIN CATCH
	ROLLBACK TRAN
	END CATCH

GO

CREATE PROCEDURE spTransactionsSum
@DateFrom DATETIME,
@DateTo DATETIME,
@TransactionType NVARCHAR(50), 
@Sum NUMERIC(20, 2) OUT
AS
BEGIN TRAN
	BEGIN TRY
	DECLARE @TransactionTypeId INT
	SELECT @TransactionTypeID = st.Id FROM tblTransactionType st WHERE st.[Name] = @TransactionType
		
		SELECT @Sum = SUM(t.Cost) FROM tblTransactions t
		WHERE @TransactionTypeId = t.TypeId AND t.CreationDate BETWEEN @DateFrom AND @DateTo

		IF @Sum IS NULL
			SET @Sum = 0;
		COMMIT TRAN
	END TRY
	
	BEGIN CATCH
		ROLLBACK TRAN 
	END CATCH
GO

CREATE PROCEDURE spUpdateTransaction
@TransactioType NVARCHAR(50),
@Id INT,
@NewName NVARCHAR(100),
@Cost NUMERIC(18,2),
@RepeatType NVARCHAR(50),
@CreditCard NVARCHAR(50),
@CategoryName NVARCHAR(50),
@Date DATETIME
AS
BEGIN TRAN
	BEGIN TRY
	DECLARE @RepeatStatusId INT, @CategoryId INT, @CreditCardId INT, @TypeId INT;
	
	
	SELECT @TypeId = t.Id FROM tblTransactionType t
	WHERE t.Name = @TransactioType;

	SELECT @CategoryId = c.Id FROM tblCategory c
	WHERE c.Name = @CategoryName;

	SELECT @RepeatStatusId = rt.Id FROM tblRepeatableExpensesType rt
	WHERE rt.Name = @RepeatType

	SELECT @CreditCardId = cc.Id FROM tblCreditCard cc
	WHERE @CreditCard = CAST(cc.Type +'  ('+ CAST(cc.Number as NVARCHAR(16))+')' AS NVARCHAR(50))

	IF @RepeatType LIKE 'None'
		BEGIN
			UPDATE tblTransactions 
			SET
			Name = @NewName,
			Cost = @Cost,
			CategoryId=@CategoryId,
			CreditCardId=@CreditCardId,
			CreationDate = @Date,
			TypeId = @TypeId,
			TransactionStatus = 0,
			RepeatableTypeId =@RepeatStatusId
			WHERE Id=@Id
			END
	ELSE
		BEGIN
			UPDATE tblTransactions 
			SET
			Name = @NewName,
			Cost = @Cost,
			CategoryId=@CategoryId,
			CreditCardId=@CreditCardId,
			CreationDate = @Date,
			TypeId = @TypeId,
			TransactionStatus = 1,
			RepeatableTypeId =@RepeatStatusId
			WHERE Id=@Id
			
		END
	
		COMMIT TRAN
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN 
	END CATCH
GO
