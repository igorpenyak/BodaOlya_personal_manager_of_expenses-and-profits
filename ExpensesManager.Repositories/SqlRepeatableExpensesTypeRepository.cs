using ExpensesManager.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesManager.Repositories
{
    public class SqlRepeatableExpensesTypeRepository : SqlBaseRepository, IRepeatableExpensesType
    {
        public SqlRepeatableExpensesTypeRepository(string connection) : base(connection)
        {
        }

        public IEnumerable<RepeatableExpensesType> SelectAll()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT Id, Name FROM tblRepeatableExpensesType", connection))
                {
                    var repeatableTypes = new List<RepeatableExpensesType>();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            repeatableTypes.Add(new RepeatableExpensesType
                            {
                                Name = (string)reader["Name"],
                                Id = (int)reader["Id"]
                            });
                        }

                    }
                    return repeatableTypes;

                }
            }
        }
    }
}
