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
    public class SqlCreditCardRepository : SqlBaseRepository, ICreditCardRepository
    {
        public SqlCreditCardRepository(string connection) : base(connection)
        {
        }

        public int InsertNewCreditCard(CreditCard card)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("spAddNewCreditCard", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Number", SqlDbType.BigInt).Value = card.Number;
                    command.Parameters.Add("@Type", SqlDbType.NChar).Value = card.Type;
                    command.Parameters.Add("@DateOff", SqlDbType.DateTime).Value = card.DateOf;
                    command.Parameters.Add("Balance", SqlDbType.Decimal).Value = card.Balance;
                    command.Parameters.Add("UserId", SqlDbType.Int).Value = card.UserId;
                    command.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;

                    command.ExecuteNonQuery();

                    return card.Id = (int)command.Parameters["@Id"].Value;

                }

            }
        }

        public IEnumerable<CreditCard> SelectAll(int userId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("spSelectUserCreditCards", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

                    var creditCardList = new List<CreditCard>();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            creditCardList.Add(new CreditCard
                            {

                                Id = (int)reader["Id"],
                                Number = long.Parse(reader["Number"].ToString()),
                                Type = (string)reader["Type"],
                                DateOf = (DateTime)reader["DateOf"],
                                Balance = (decimal)reader["Balance"],
                                UserId = (int)reader["UserId"]

                            });
                        }

                    }
                    return creditCardList;
                }
            }
        }
    }
}
