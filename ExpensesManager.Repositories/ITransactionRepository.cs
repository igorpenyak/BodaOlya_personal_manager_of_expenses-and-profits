using ExpensesManager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesManager.Repositories
{
    public interface ITransactionRepository
    {
        IEnumerable<Transactions> SelectAllTransactions(string transactionType, int userId);
        int InsertNewTransaction(Transactions transaction);
        IEnumerable<Transactions> SortTransactionsByCategory(string categoryName, string transactionType);
        IEnumerable<Transactions> SortTransactionsByDate(DateTime date, string transactionType);
        IEnumerable<Transactions> SelectTransactionsByDateCard(long number, DateTime dateFrom, DateTime dateTo, string transactionType);
        decimal CalculeteSumTransactions(DateTime dateFrom, DateTime dateTo, string transactionType);
        int UpdateTransaction(Transactions transaction, string newName, string transactionType);

    }

}
