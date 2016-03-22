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
    public class SqlUsersRepository : SqlBaseRepository, IUsersRepository
    {
        public SqlUsersRepository(string connection) : base(connection)
        {
        }

        public Users SelectUserByLogin(string login, string password)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelectUserByLogin", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Login", login);
                    command.Parameters.AddWithValue("@Password", password);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Users user = null;
                        if (reader.Read())
                        {
                            user = new Users();
                            user.Id = (int)reader["Id"];
                            user.FirstName = (string)reader["FirstName"];
                            user.LastName = (string)reader["LastName"];
                            user.Login = (string)reader["Login"];
                            user.IsDisable = (bool)reader["IsDisable"];
                        }
                        return user;
                    }
                }
            }
        }
    }
}
