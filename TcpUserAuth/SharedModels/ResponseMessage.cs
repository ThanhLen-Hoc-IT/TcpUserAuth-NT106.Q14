using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels
{
    /// <summary>
    /// Gói tin mà Server trả về cho Client.
    /// </summary>
    public class ResponseMessage
    {
        // Kết quả xử lý (true = thành công, false = thất bại)
        public bool Success { get; set; }

        // Thông báo cho người dùng hoặc client
        public string Message { get; set; }

        // Token trả về khi login thành công
        public string Token { get; set; }

        // Thông tin user (nếu cần)
        public User User { get; set; }

        // Dữ liệu bổ sung
        public object Data { get; set; }
    }
}
