using System;
using System.Data;
using System.Data.SqlClient;


namespace WPFSQLL.Model
{
    public class DatabaseLogic
    {
        private string connectionString = "Data Source=localhost;Initial Catalog=WPFS;Integrated Security=True";

        public DatabaseLogic(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void AddUser(User user)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO [dbo].[User] (Nickname, Password) VALUES (@Nickname, @Password)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nickname", user.Nickname);
                    command.Parameters.AddWithValue("@Password", user.Password);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateUser(int id, string nickname, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE [dbo].[User] SET Nickname = @Nickname, Password = @Password WHERE ID = @ID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    command.Parameters.AddWithValue("@Nickname", nickname);
                    command.Parameters.AddWithValue("@Password", password);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteUser(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "DELETE FROM [dbo].[User] WHERE ID = @ID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public DataTable GetUserData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM [dbo].[User]";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
        }

        public bool UserExists(User user)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM [dbo].[User] WHERE Nickname = @Nickname AND Password = @Password";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nickname", user.Nickname);
                    command.Parameters.AddWithValue("@Password", user.Password);

                    connection.Open();
                    int userCount = (int)command.ExecuteScalar();

                    return userCount > 0;
                }
            }
        }

        public List<User> GetUsers()
        {
            List<User> users = new List<User>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT ID, Nickname, Password FROM [dbo].[User]";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                User user = new User
                                {
                                    ID = Convert.ToInt32(reader["ID"]),
                                    Nickname = reader["Nickname"].ToString(),
                                    Password = reader["Password"].ToString()
                                };

                                users.Add(user);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок, например, логирование или отображение сообщения
                Console.WriteLine($"Ошибка при загрузке данных из БД: {ex.Message}");
            }

            return users;
        }
    }
}
