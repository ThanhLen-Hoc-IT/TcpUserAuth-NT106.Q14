using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace ClientApp
{
    internal class Utilities
    {
        /// <summary>
        /// Chuyển đối tượng bất kỳ sang chuỗi JSON.
        /// </summary>
        public static string ToJson(object obj)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = false, // gọn nhẹ khi truyền qua socket
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return JsonSerializer.Serialize(obj, options);
        }

        /// <summary>
        /// Giải mã chuỗi JSON thành đối tượng kiểu T.
        /// </summary>
        public static T FromJson<T>(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ JSON parse error: {ex.Message}");
                return default;
            }
        }
    }
}
