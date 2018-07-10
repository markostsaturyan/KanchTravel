using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;
using Authentication.DataManagement.DataAccesLayer.DataAccessLayerDataModel;

namespace Authentication.DataManagement.DataAccesLayer
{
    public static class AuthDataAccessLayer
    {
        private static string connectionString = @"Data Source=DESKTOP-7S9MC1D\SQLEXPRESS;Initial Catalog=UsersDB;Integrated Security=True;";
            /*new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json").Build()["ConnectionStrings:currentConnectionString"];*/

        public static UserIdentifiers GetByUserName(string userName)
        {
            var user = new UserIdentifiers();
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetByUserName"
                };

                command.Parameters.AddWithValue("@userName", userName);

                connection.Open();

                

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    if (dataReader.Read())
                    {
                        user.Id = (int)dataReader["Id"];
                        user.UserName = (string)dataReader["UserName"];
                        user.Password = (string)dataReader["Password"];
                        user.Role = (string)dataReader["RoleName"];
                        user.IsActive = (bool)dataReader["IsActive"];
                    }
                }
            }
            return user;
        }

        public static UserIdentifiers GetByUserId(long id)
        {
            var user = new UserIdentifiers();

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetByUserId"
                };

                command.Parameters.AddWithValue("@id", id);

                connection.Open();

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    if (dataReader.Read())
                    {
                        user.Id = (int)dataReader["Id"];
                        user.UserName = (string)dataReader["UserName"];
                        user.Password = (string)dataReader["Password"];
                        user.Role = (string)dataReader["RoleName"];
                        user.IsActive = (bool)dataReader["IsActive"];
                    }
                }
            }
            return user;
        }

    }
}
