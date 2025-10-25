using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels
{
    public class ResponseMessage
    {
        // Trạng thái phản hồi
        public bool Success { get; set; }

        // Thông điệp hiển thị cho người dùng hoặc log
        public string Message { get; set; } = string.Empty;

        // Token trả về sau khi đăng nhập (nếu có)
        public string Token { get; set; }

        // Dữ liệu người dùng hoặc thông tin khác
        public object User { get; set; }

        // Dữ liệu bổ sung
        public object Data { get; set; }
    }
}
