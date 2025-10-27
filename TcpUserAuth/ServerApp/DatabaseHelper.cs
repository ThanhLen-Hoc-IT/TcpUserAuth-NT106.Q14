using System;
using System.Data.SqlClient;
using SharedModels;

namespace ServerApp
{
    public static class DatabaseHelper
    {
        // ⚙️ Chuỗi kết nối SQL Server bằng tài khoản sa
        private static readonly string connectionString =
            "Server=DELL-010824-01;Database=LoginDB;User Id=sa;Password=Kimngan17052006;";

        // ✅ Kiểm tra username đã tồn tại
        public static bool UsernameExists(string username)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM Users WHERE LOWER(Username) = LOWER(@u)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        // ✅ Thêm user mới vào bảng
        public static void InsertUser(RequestMessage req)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO Users (Username, PasswordHash, FullName, Email)
                               VALUES (@u, @p, @f, @e)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@u", req.Username);
                    cmd.Parameters.AddWithValue("@p", req.PasswordHash);
                    cmd.Parameters.AddWithValue("@f", req.FullName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@e", req.Email ?? (object)DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ✅ Lấy user theo username
        public static (bool ok, string hash, User user) GetUserByUsername(string username)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT Id, Username, PasswordHash, FullName, Email 
                               FROM Users 
                               WHERE LOWER(Username) = LOWER(@u)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            var user = new User
                            {
                                Id = rd.GetInt32(0),
                                Username = rd.GetString(1),
                                PasswordHash = rd.GetString(2),
                                FullName = rd.IsDBNull(3) ? "" : rd.GetString(3),
                                Email = rd.IsDBNull(4) ? "" : rd.GetString(4)
                            };
                            return (true, user.PasswordHash, user);
                        }
                    }
                }
            }
            return (false, null, null);
        }
    }
}
