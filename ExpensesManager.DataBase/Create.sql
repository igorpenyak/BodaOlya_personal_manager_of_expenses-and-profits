CREATE DATABASE ExpensesManager
GO

USE ExpensesManager
GO
CREATE TABLE tblCategory
(
	Id INT NOT NULL IDENTITY(1, 1),
	Name NVARCHAR(50) NOT NULL,
	CONSTRAINT PK_tblCategory_ID PRIMARY KEY(Id)
)

CREATE TABLE tblTransactionType
(
	Id INT NOT NULL IDENTITY(1, 1),
	Name NVARCHAR(50) NOT NULL,
	CONSTRAINT PK_tblTransactionType_ID PRIMARY KEY(Id)
)
CREATE TABLE tblUsers
(
	Id INT NOT NULL IDENTITY(1,1),
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	[Login] VARCHAR(20) NOT NULL,
	[Password] VARCHAR(100) NOT NULL,
	IsDisable BIT NOT NULL,
	CONSTRAINT PF_tblUsers_ID PRIMARY KEY(Id),
	CONSTRAINT UQ_tblUsers_Login UNIQUE([Login])
)
GO

CREATE TABLE tblRepeatableExpensesType
(
	Id INT NOT NULL IDENTITY(1, 1),
	Name NVARCHAR(50) NOT NULL,
	CONSTRAINT PK_tblRepeatableExpensesType_ID PRIMARY KEY(Id)
)

CREATE TABLE tblCreditCard
(
	Id INT NOT NULL IDENTITY(1, 1),
	Number NUMERIC(16) NOT NULL,
	[Type] NVARCHAR(50), 
	DateOf DATETIME NOT NULL,
	Balance NUMERIC(18,2) NOT NULL, 
	UserId INT NOT NULL,
	CONSTRAINT PF_tblCreditCard_ID PRIMARY KEY(Id), 
	CONSTRAINT UQ_tblCreditCard_CardNumber UNIQUE(Number),
	CONSTRAINT FK_tblCreditCard_UserId_tblUsers_Id FOREIGN KEY (UserId) REFERENCES tblUsers(Id)
)
GO

CREATE TABLE tblTransactions
(
	Id INT NOT NULL IDENTITY(1, 1),
	Name NVARCHAR(100) NOT NULL,
	Cost NUMERIC(18,2) NOT NULL, 
	CreditCardId INT,
	CategoryId INT NOT NULL,
	CreationDate DATETIME NOT NULL,
	TypeId INT NOT NULL,
	TransactionStatus BIT NOT NULL,
	RepeatableTypeId INT NOT NULL,
	LastRepeatDate DATETIME, 
	UserId INT NOT NULL,
	CONSTRAINT PF_tblTransactions_ID PRIMARY KEY(Id),
	CONSTRAINT FK_tblTransactions_CreditCardId_tblCreditCard_Id FOREIGN KEY (CreditCardId) REFERENCES tblCreditCard(Id),
    CONSTRAINT FK_tblTransactions_CategoryId_tblCategory_Id FOREIGN KEY (CategoryId) REFERENCES tblCategory(Id),
	CONSTRAINT FK_tblTransactions_TypeId_tblTransactionType_Id FOREIGN KEY (TypeId) REFERENCES tblTransactionType(Id),
	CONSTRAINT FK_tblTransactions_RepeatableTypeId_tblRepeatableExpensesType_Id FOREIGN KEY (RepeatableTypeId) REFERENCES tblRepeatableExpensesType(Id),
	CONSTRAINT FK_tblTransactions_UserId_tblUsers_Id FOREIGN KEY (UserId) REFERENCES tblUsers(Id)
)
GO

