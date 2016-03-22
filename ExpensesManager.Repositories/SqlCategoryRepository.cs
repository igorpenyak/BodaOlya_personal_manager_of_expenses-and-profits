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
    public class SqlCategoryRepository : SqlBaseRepository, ICategoryRepository
    {

        public SqlCategoryRepository(string connection) : base(connection)
        {
        }

        public IEnumerable<Category> SelectAll()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT Id, Name FROM tblCategory", connection))
                {
                    var categoryList = new List<Category>();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categoryList.Add(new Category
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"]
                            });
                        }

                    }
                    return categoryList;
                }

            }

        }

        public int InsertNewCategory(Category obj)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("spAddNewCategory", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = obj.Name;
                    command.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    command.ExecuteNonQuery();
                    return obj.Id = (int)command.Parameters["@Id"].Value;

                }

            }


        }
    }
}
