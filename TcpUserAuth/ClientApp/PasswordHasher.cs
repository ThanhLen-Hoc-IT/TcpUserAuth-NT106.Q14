using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using SharedModels;


namespace ClientApp
{
    // Lớp dùng để mã hóa mật khẩu bằng SHA-256
    public static class PasswordHasher
    {
        // Hàm băm chuỗi đầu vào thành chuỗi SHA-256 (64 ký tự hex)
        public static string Sha256(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input)); // Băm chuỗi thành mảng byte
            var sb = new StringBuilder();
            foreach (var b in bytes) sb.Append(b.ToString("x2")); // Chuyển byte sang dạng hex
            return sb.ToString();
        }
    }

}
