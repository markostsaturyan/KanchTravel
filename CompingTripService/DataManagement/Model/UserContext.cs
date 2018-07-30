using System.Data.SqlClient;
using CampingTripService.DataManagement.Model.Users;

namespace CampingTripService.DataManagement.Model
{
    public class UserContext
    {
        public string ConnectionString { get; set; }
        public User GetUser(int id)
        {
            var user = new User();

            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetUser"
                };

                command.Parameters.AddWithValue("@userId", id);

                connection.Open();

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    if (dataReader.Read())
                    {
                        user.Id = (int)dataReader["Id"];
                        user.FirstName = (string)dataReader["FirstName"];
                        user.LastName = (string)dataReader["LastName"];
                        user.Age = (int)dataReader["Age"];
                    }
                }
            }
            return user;
        }

        public void SignUpForTheCamping(int id, string campingTripID)
        {

            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "SignUpForTheTrip"
                };

                command.Parameters.AddWithValue("@CampingTripID", campingTripID);
                command.Parameters.AddWithValue("@UserID", id);

                connection.Open();

                var dataReader = command.ExecuteReader();
            }
        }
    }
}
