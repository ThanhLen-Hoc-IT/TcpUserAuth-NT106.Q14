using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels
{
    public class RequestMessage
    {
        // Hành động mà client yêu cầu, ví dụ: "Register", "Login", "GetProfile", "Logout"
        public string Action { get; set; } = string.Empty;

        // Dữ liệu người dùng gửi kèm
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        // Token để xác thực (khi đã đăng nhập)
        public string Token { get; set; }

        // Dữ liệu bổ sung (nếu có)
        public object Data { get; set; }
    }
}
