CREATE View vJoinAllTable
AS 
		  SELECT 
				t.Id AS 'Id',
			    t.Name AS 'Name',
				t.Cost AS 'Cost',
				t.CreditCardId AS 'CreditCardId',
				t.CategoryId AS 'CategoryId',
				t.TypeId AS 'TypeId',
				t.RepeatableTypeId AS 'RepeatableTypeId',
				t.UserId AS 'UserId',
				cc.Type +'  ('+ CAST(cc.Number as NVARCHAR(16))+')'  AS 'Credit card',
				c.Name AS 'Category',
				t.CreationDate AS 'CreationDate',
				t.LastRepeatDate AS 'LastRepeatDate',
				tt.Name AS 'Transaction name',
				rt.Name AS 'Repeatable Expenses'
				FROM tblTransactions t
				LEFT JOIN tblCreditCard cc ON cc.Id = t.CreditCardId 
				LEFT JOIN tblCategory c ON  t.CategoryId = c.Id
				LEFT JOIN tblTransactionType tt ON t.TypeId = tt.Id
				LEFT JOIN tblRepeatableExpensesType rt ON rt.Id = t.RepeatableTypeId
				LEFT JOIN tblUsers u ON u.Id = t.UserId
GO