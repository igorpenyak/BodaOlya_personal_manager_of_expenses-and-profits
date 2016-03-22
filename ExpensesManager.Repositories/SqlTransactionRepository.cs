using ExpensesManager.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesManager.Repositories
{
    public class SqlTransactionRepository : SqlBaseRepository, ITransactionRepository
    {
        public SqlTransactionRepository(string connection) : base(connection)
        {
        }

        public decimal CalculeteSumTransactions(DateTime dateFrom, DateTime dateTo, string transactionType)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                decimal sum = 0;
                using (var command = new SqlCommand("spTransactionsSum", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@TransactionType", SqlDbType.NChar).Value = transactionType;
                    command.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = dateFrom;
                    command.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = dateTo;
                    command.Parameters.Add("@Sum", SqlDbType.Decimal).Direction = ParameterDirection.Output;

                    command.ExecuteNonQuery();
                    return sum = (decimal)command.Parameters["@Sum"].Value;



                }
            }

        }

        public int InsertNewTransaction(Transactions transaction)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("spAddNewTransaction", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Name", SqlDbType.NChar).Value = transaction.Name;
                    command.Parameters.Add("@Cost", SqlDbType.Decimal).Value = transaction.Cost;
                    command.Parameters.Add("@CreationDate", SqlDbType.DateTime).Value = transaction.CreationDate;
                    command.Parameters.Add("@Category", SqlDbType.NChar).Value = transaction.CategoryName;
                    command.Parameters.Add("@RepetableType", SqlDbType.NChar).Value = transaction.RepeatableType;
                    command.Parameters.Add("@CreditCard", SqlDbType.NChar).Value = transaction.CreditCard;
                    command.Parameters.Add("@TransactionType", SqlDbType.NChar).Value = transaction.TypeOfTransaction;
                    command.Parameters.Add("@UserId", SqlDbType.Int).Value = transaction.UserId;
                    command.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;

                    command.ExecuteNonQuery();
                    return transaction.Id = (int)command.Parameters["@Id"].Value;

                }
            }
        }

        public IEnumerable<Transactions> SelectAllTransactions(string transactionType, int userId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();


                using (var command = new SqlCommand("spSelectAllTransactions", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@TransactionType", SqlDbType.NChar).Value = transactionType;
                    command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

                    var transactionList = new List<Transactions>();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            transactionList.Add(new Transactions
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"],
                                Cost = (decimal)reader["Cost"],
                                CategoryId = (int)reader["CategoryId"],
                                CreditCardId = (int)reader["CreditCardId"],
                                TypeId = (int)reader["TypeId"],
                                RepeatableTypeId = (int)reader["RepeatableTypeId"],
                                UserId = (int)reader["UserId"],
                                CategoryName = (string)reader["Category"],
                                TypeOfTransaction = (string)reader["Transaction name"],
                                CreditCard = (string)reader["Credit card"],
                                CreationDate = (DateTime)reader["CreationDate"],
                                RepeatableType = (string)reader["Repeatable Expenses"]
                            });
                        }

                    }
                    return transactionList;
                }
            }
        }

        public IEnumerable<Transactions> SelectTransactionsByDateCard(long number, DateTime dateFrom, DateTime dateTo, string transactionType)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("spGetTranByCardAndDate", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@TransactionType", SqlDbType.NChar).Value = transactionType;
                    command.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = dateFrom;
                    command.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = dateTo;
                    command.Parameters.Add("@CardNum", SqlDbType.BigInt).Value = number;

                    var transactionList = new List<Transactions>();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            transactionList.Add(new Transactions
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"],
                                Cost = (decimal)reader["Cost"],
                                CategoryId = (int)reader["CategoryId"],
                                CreditCardId = (int)reader["CreditCardId"],
                                TypeId = (int)reader["TypeId"],
                                RepeatableTypeId = (int)reader["RepeatableTypeId"],
                                UserId = (int)reader["UserId"],
                                CategoryName = (string)reader["Category"],
                                TypeOfTransaction = (string)reader["Transaction name"],
                                CreditCard = (string)reader["Credit card"],
                                CreationDate = (DateTime)reader["CreationDate"],
                                RepeatableType = (string)reader["Repeatable Expenses"]
                            });
                        }

                    }
                    return transactionList;

                }
            }

        }

        public IEnumerable<Transactions> SortTransactionsByCategory(string categoryName, string transactionType)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("spSelectTransactionByCategory", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@TransactionType", SqlDbType.NChar).Value = transactionType;
                    command.Parameters.Add("@CategoryName", SqlDbType.NChar).Value = categoryName;

                    var transactionList = new List<Transactions>();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            transactionList.Add(new Transactions
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"],
                                Cost = (decimal)reader["Cost"],
                                CategoryId = (int)reader["CategoryId"],
                                CreditCardId = (int)reader["CreditCardId"],
                                TypeId = (int)reader["TypeId"],
                                RepeatableTypeId = (int)reader["RepeatableTypeId"],
                                UserId = (int)reader["UserId"],
                                CategoryName = (string)reader["Category"],
                                TypeOfTransaction = (string)reader["Transaction name"],
                                CreditCard = (string)reader["Credit card"],
                                CreationDate = (DateTime)reader["CreationDate"],
                                RepeatableType = (string)reader["Repeatable Expenses"]
                            });
                        }

                    }
                    return transactionList;

                }
            }
        }

        public IEnumerable<Transactions> SortTransactionsByDate(DateTime date, string transactionType)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("spGetTransactionsByDate", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@TransactionType", SqlDbType.NChar).Value = transactionType;
                    command.Parameters.Add("@Date", SqlDbType.DateTime).Value = date;

                    var transactionList = new List<Transactions>();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            transactionList.Add(new Transactions
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"],
                                Cost = (decimal)reader["Cost"],
                                CategoryId = (int)reader["CategoryId"],
                                CreditCardId = (int)reader["CreditCardId"],
                                TypeId = (int)reader["TypeId"],
                                RepeatableTypeId = (int)reader["RepeatableTypeId"],
                                UserId = (int)reader["UserId"],
                                CategoryName = (string)reader["Category"],
                                TypeOfTransaction = (string)reader["Transaction name"],
                                CreditCard = (string)reader["Credit card"],
                                CreationDate = (DateTime)reader["CreationDate"],
                                RepeatableType = (string)reader["Repeatable Expenses"]
                            });
                        }

                    }
                    return transactionList;

                }
            }
        }

        public int UpdateTransaction(Transactions transaction, string newName, string transactionType)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("spUpdateTransaction", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = transaction.Id;
                    command.Parameters.Add("@NewName", SqlDbType.NChar).Value = newName;
                    command.Parameters.Add("@Cost", SqlDbType.Decimal).Value = transaction.Cost;
                    command.Parameters.Add("@Date", SqlDbType.DateTime).Value = transaction.CreationDate;
                    command.Parameters.Add("@CategoryName", SqlDbType.NChar).Value = transaction.CategoryName;
                    command.Parameters.Add("@RepeatType", SqlDbType.NChar).Value = transaction.RepeatableType;
                    command.Parameters.Add("@CreditCard", SqlDbType.NChar).Value = transaction.CreditCard;
                    command.Parameters.Add("@TransactioType", SqlDbType.NChar).Value = transactionType;

                    command.ExecuteNonQuery();

                    return transaction.Id;
                }
            }
        }
    }
}
