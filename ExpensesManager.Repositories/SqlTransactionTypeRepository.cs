using ExpensesManager.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesManager.Repositories
{
    public class SqlTransactionTypeRepository : SqlBaseRepository, ITransactionTypeRepository
    {
        public SqlTransactionTypeRepository(string connection) : base(connection)
        {

        }
        public IEnumerable<TransactionType> SelectAllType()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT Id, Name FROM tblTransactionType", connection))
                {
                    var typeList = new List<TransactionType>();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            typeList.Add(new TransactionType
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"]
                            });
                        }

                    }
                    return typeList;
                }

            }
        }
    }
}
