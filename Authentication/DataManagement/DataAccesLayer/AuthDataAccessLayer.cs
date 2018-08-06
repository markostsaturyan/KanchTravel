using System.Data.SqlClient;
using Authentication.DataManagement.DataAccesLayer.DataAccessLayerDataModel;

namespace Authentication.DataManagement.DataAccesLayer
{
    public class AuthDataAccessLayer
    {
        private string connectionString;

        public AuthDataAccessLayer(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public UserIdentifiers GetByUserName(string userName)
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

                command.Connection.Open();

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
                        user.UserGuid = (string)dataReader["UserGuid"];
                        user.IsApproved = (bool)dataReader["IsApproved"];
                    }
                }
            }
            return user;
        }

        public UserIdentifiers GetByUserId(long id)
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
                        user.IsApproved = (bool)dataReader["IsApproved"];
                    }
                }
            }
            return user;
        }

    }
}
