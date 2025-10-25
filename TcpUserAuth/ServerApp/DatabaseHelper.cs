using System;
using System.Data.SqlClient;
using SharedModels;

namespace ServerApp
{
    public class DatabaseHelper
    {
       
        private string connectionString =
            "Server=DELL-010824-01;Database=CareerAppDB;User Id=sa;Password=Kimngan17052006;";

        public bool UserExists(string username)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE Username = @username";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public bool RegisterUser(User user)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Users (Username, PasswordHash, FullName, Email) " +
                               "VALUES (@u, @p, @f, @e)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@u", user.Username);
                cmd.Parameters.AddWithValue("@p", user.PasswordHash);
                cmd.Parameters.AddWithValue("@f", user.FullName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@e", user.Email ?? (object)DBNull.Value);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool ValidateLogin(string username, string passwordHash)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE Username = @u AND PasswordHash = @p";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@p", passwordHash);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        
        public int ValidateLoginAndGetUserId(string username, string passwordHash)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Id FROM Users WHERE Username = @u AND PasswordHash = @p";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@p", passwordHash);

                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        
        public static bool LoginUser(string username, string password)
        {
            var db = new DatabaseHelper();
            string hashed = HashPassword(password);
            return db.ValidateLogin(username, hashed);
        }

       
        public static int LoginUserAndGetUserId(string username, string password)
        {
            var db = new DatabaseHelper();
            string hashed = HashPassword(password);
            return db.ValidateLoginAndGetUserId(username, hashed);
        }

        
        public static string HashPassword(string input)
        {
            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(input);
                var hashBytes = sha.ComputeHash(bytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
