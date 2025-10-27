using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels
{
    /// <summary>
    /// Gói tin mà Client gửi lên Server.
    /// </summary>
    public class RequestMessage
    {
        // Hành động mà client yêu cầu, ví dụ: "Register", "Login", "GetProfile", "Logout"
        public string Action { get; set; } = string.Empty;

        // Dữ liệu người dùng gửi kèm
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        // Token để xác thực (dùng khi đã đăng nhập)
        public string Token { get; set; }

        // Dữ liệu bổ sung (tùy loại yêu cầu)
        public string FullName { get; set; }
        public string Email { get; set; }
        public object Data { get; set; }
    }
}

