using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;


namespace ServerApp
{
    internal class Utilities
    {
        /// <summary>
        /// Chuyển object sang JSON string (để gửi qua mạng).
        /// </summary>
        public static string ToJson(object obj)
        {
            try
            {
                return JsonSerializer.Serialize(obj);
            }
            catch (Exception ex)
            {
                Logger.Error("JSON serialize error: " + ex.Message);
                return "{}";
            }
        }

        /// <summary>
        /// Chuyển JSON string thành object (ví dụ RequestMessage).
        /// </summary>
        public static T FromJson<T>(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                Logger.Error("JSON parse error: " + ex.Message);
                return default(T);
            }
        }

        /// <summary>
        /// Sinh SHA-256 hash cho password (có thể dùng khi client gửi plain text).
        /// </summary>
        public static string Sha256(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sb = new StringBuilder();
                foreach (var b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
    }
}
