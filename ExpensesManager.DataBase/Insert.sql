USE ExpensesManager 
GO

SET IDENTITY_INSERT tblUsers ON
INSERT INTO tblUsers (Id, FirstName, LastName,[Login], [Password], IsDisable)
	VALUES
	(1, 'Olha', 'Boda', 'Olya' , '827ccb0eea8a706c4c34a16891f84e7b', 0),
	(2, 'Halyna', 'Leshchyshyn', 'Halynka', '1e01ba3e07ac48cbdab2d3284d1dd0fa',0)
SET IDENTITY_INSERT tblUsers OFF
GO

SET IDENTITY_INSERT tblCategory ON
INSERT INTO tblCategory (Id, Name)
	VALUES
	(1, 'All'),
	(2, 'Food'),
	(3, 'Home'),
	(4, 'Transport expenses'),
	(5, 'Entertainment'),
	(6, 'Education'),
	(7, 'Salary'),
	(8, 'Other income'),
	(9, 'Healthy & beauty'),
    (10, 'Stipend')
SET IDENTITY_INSERT tblCategory OFF
GO

SET IDENTITY_INSERT tblCreditCard ON
INSERT INTO tblCreditCard (Id, Number, [Type], DateOf, Balance, UserId)
	VALUES
	(1, 4243231426655567, 'KDV VISA', '2017-4-4', 1232.50, 1),
	(2, 4543231426657565, 'KDV VISA', '2018-9-4',2004.56, 1),
	(3, 7843231426655590, 'HK MASTER', '2017-4-4',5420.10, 2),
	(4, 5643231426655590, 'HK MASTER', '2017-9-4',5420.10, 2)
SET IDENTITY_INSERT tblCreditCard OFF
GO

SET IDENTITY_INSERT tblRepeatableExpensesType ON
INSERT INTO tblRepeatableExpensesType (Id, Name)
	VALUES
	(1, 'None'),
	(2, 'Weekly'),
	(3, 'Monthly'),
	(4, 'Yearly')
SET IDENTITY_INSERT tblRepeatableExpensesType OFF
GO

SET IDENTITY_INSERT tblTransactionType ON
INSERT INTO tblTransactionType (Id, Name)
	VALUES
	(1, 'Income'),
	(2, 'Expense'),
	(3, 'Planing Expense')
SET IDENTITY_INSERT tblTransactionType OFF
GO

SET IDENTITY_INSERT tblTransactions ON
INSERT INTO tblTransactions (Id, Name, Cost, CreditCardId, CategoryId, CreationDate, TypeId, TransactionStatus, RepeatableTypeId, LastRepeatDate, UserId)
	VALUES
	(1, 'Milk and orange',35.00,1, 2, '2016-03-16', 2, 0, 1, '2016-03-16', 1),
	(2, 'Gym',350.00, 2, 9, '2016-03-2', 2, 1, 3, '2016-03-2', 2),
    (3, 'Parfum',950.50, 1, 9, '2016-03-20', 2, 0, 1, '2016-03-20', 1),
    (4, 'Ticket for concert of Muse',1200.00, 2, 5, '2016-02-15', 2, 0, 1, '2016-02-15', 1),
    (5, 'English course',2350.00, 1, 6, '2016-02-3', 2, 1, 4, '2016-02-3', 1),
    (6, 'Armchair',745.00, 2, 3, '2016-01-16', 2, 0, 1, '2016-01-16', 1),
    (7, 'Stipend NULP',925.15, 1, 10, '2016-03-18', 1, 1, 3, '2016-03-18', 2),
    (8, 'From parents',240.00, 2, 8, '2016-03-16', 1, 0, 1, '2016-03-16', 2),
	(9, 'Refund train ticket',12.50, 1, 8, '2016-01-20', 1, 0, 1, '2016-01-20', 1),
	(10, 'Booking hotel',800.00, 2, 1, '2016-03-30', 3, 0, 1, '2016-03-30', 1),
	(11, 'MP3 player',769.00, 1, 1, '2016-02-5', 3, 0, 1, '2016-02-5', 1),
	(12, 'Sneakers',1500.00, 2, 1, '2016-03-1', 3, 0, 1, '2016-03-1', 2),
	(13, 'Train tickets to Kyiv',100.00, 1, 4, '2016-07-7', 3, 0, 1, '2016-07-7', 1),
	(14, 'Salary NULP',1450.50, 2, 7, '2016-03-21', 1, 1, 3, '2016-03-21', 2),
	(15, 'Chinese course',2450.00, 1, 6, '2016-05-10', 3, 0, 1, '2016-05-10', 1)
	SET IDENTITY_INSERT tblTransactionType OFF
GO

