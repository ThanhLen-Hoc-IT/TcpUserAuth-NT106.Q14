using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedModels;

namespace ServerApp
{
    internal class DatabaseHelper
    {
        private static readonly string userFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users.txt");

        // Kiểm tra username đã tồn tại chưa
        public static bool UsernameExists(string username)
        {
            if (!File.Exists(userFile)) return false;
            return File.ReadAllLines(userFile)
                       .Select(line => line.Split('|')[0])
                       .Any(u => u.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        // Thêm user mới vào file
        public static void InsertUser(User user)
        {
            using (StreamWriter sw = File.AppendText(userFile))
            {
                sw.WriteLine($"{user.Username}|{user.PasswordHash}|{user.FullName}|{user.Email}");
            }
        }

        // Lấy thông tin user theo username
        public static User GetUserByUsername(string username)
        {
            if (!File.Exists(userFile)) return null;

            foreach (var line in File.ReadAllLines(userFile))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                string[] parts = line.Split('|');
                if (parts.Length >= 4 && parts[0].Equals(username, StringComparison.OrdinalIgnoreCase))
                {
                    return new User
                    {
                        Username = parts[0],
                        PasswordHash = parts[1],
                        FullName = parts[2],
                        Email = parts[3]
                    };
                }
            }
            return null;
        }
    }
}
