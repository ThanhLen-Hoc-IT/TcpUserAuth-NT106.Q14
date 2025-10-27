using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    /// <summary>
    /// Lớp tĩnh lưu trữ thông tin phiên đăng nhập hiện tại.
    /// (Dùng để giữ Token và Username trong suốt thời gian chạy chương trình)
    /// </summary>
    public static class SessionManager
    {
        /// <summary>
        /// Token được server trả về sau khi đăng nhập thành công.
        /// </summary>
        public static string Token { get; set; }

        /// <summary>
        /// Tên người dùng hiện tại.
        /// </summary>
        public static string Username { get; set; }

        /// <summary>
        /// Xóa thông tin phiên (khi logout hoặc đóng app).
        /// </summary>
        public static void Clear()
        {
            Token = null;
            Username = null;
        }
    }
}
