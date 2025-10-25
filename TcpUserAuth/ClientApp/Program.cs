//////using System;
//////using System.Collections.Generic;
//////using System.Linq;
//////using System.Threading.Tasks;
//////using System.Windows.Forms;

//////namespace ClientApp
//////{
//////    internal static class Program
//////    {
//////        /// <summary>
//////        /// The main entry point for the application.
//////        /// </summary>
//////        [STAThread]
//////        static void Main()
//////        {
//////            Application.EnableVisualStyles();
//////            Application.SetCompatibleTextRenderingDefault(false);
//////            Application.Run(new FrmRegister());
//////        }
//////    }
//////}

////using System;
////using System.Windows.Forms;

////// 🔹 Thêm dòng này nếu FrmRegister nằm trong cùng project ClientApp
////// Nếu FrmRegister có namespace khác, ví dụ ClientApp.Forms,
////// thì sửa dòng này tương ứng.
////using ClientApp;

////namespace ClientApp
////{
////    internal static class Program
////    {
////        /// <summary>
////        /// Điểm khởi động chính của ứng dụng.
////        /// </summary>
////        [STAThread]
////        static void Main()
////        {
////            Application.EnableVisualStyles();
////            Application.SetCompatibleTextRenderingDefault(false);

////            // 👇 Chạy form đăng ký thay vì Form1
////            Application.Run(new FrmRegister());
////        }
////    }
////}


////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Threading.Tasks;
////using System.Windows.Forms;

////namespace ClientApp
////{
////    internal static class Program
////    {
////        /// <summary>
////        /// The main entry point for the application.
////        /// </summary>
////        [STAThread]
////        static void Main()
////        {
////            Application.EnableVisualStyles();
////            Application.SetCompatibleTextRenderingDefault(false);
////            Application.Run(new FrmRegister());
////        }
////    }
////}

//using System;
//using System.Windows.Forms;

//// 🔹 Thêm dòng này nếu FrmRegister nằm trong cùng project ClientApp
//// Nếu FrmRegister có namespace khác, ví dụ ClientApp.Forms,
//// thì sửa dòng này tương ứng.
//using ClientApp;

//namespace ClientApp
//{
//    internal static class Program
//    {
//        /// <summary>
//        /// Điểm khởi động chính của ứng dụng.
//        /// </summary>
//        [STAThread]
//        static void Main()
//        {
//            Application.EnableVisualStyles();
//            Application.SetCompatibleTextRenderingDefault(false);

//            // 👇 Chạy form đăng ký thay vì Form1
//            Application.Run(new FrmRegister());
//        }
//    }
//}


////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Threading.Tasks;
////using System.Windows.Forms;

////namespace ClientApp
////{
////    internal static class Program
////    {
////        /// <summary>
////        /// The main entry point for the application.
////        /// </summary>
////        [STAThread]
////        static void Main()
////        {
////            Application.EnableVisualStyles();
////            Application.SetCompatibleTextRenderingDefault(false);
////            Application.Run(new FrmRegister());
////        }
////    }
////}

//using System;
//using System.Windows.Forms;

//// 🔹 Thêm dòng này nếu FrmRegister nằm trong cùng project ClientApp
//// Nếu FrmRegister có namespace khác, ví dụ ClientApp.Forms,
//// thì sửa dòng này tương ứng.
//using ClientApp;

//namespace ClientApp
//{
//    internal static class Program
//    {
//        /// <summary>
//        /// Điểm khởi động chính của ứng dụng.
//        /// </summary>
//        [STAThread]
//        static void Main()
//        {
//            Application.EnableVisualStyles();
//            Application.SetCompatibleTextRenderingDefault(false);

//            // 👇 Chạy form đăng ký thay vì Form1
//            Application.Run(new FrmRegister());
//        }
//    }
//}


//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace ClientApp
//{
//    internal static class Program
//    {
//        /// <summary>
//        /// The main entry point for the application.
//        /// </summary>
//        [STAThread]
//        static void Main()
//        {
//            Application.EnableVisualStyles();
//            Application.SetCompatibleTextRenderingDefault(false);
//            Application.Run(new FrmRegister());
//        }
//    }
//}

using System;
using System.Windows.Forms;

// 🔹 Thêm dòng này nếu FrmRegister nằm trong cùng project ClientApp
// Nếu FrmRegister có namespace khác, ví dụ ClientApp.Forms,
// thì sửa dòng này tương ứng.
using ClientApp;
using SharedModels;

namespace ClientApp
{
    internal static class Program
    {
        /// <summary>
        /// Điểm khởi động chính của ứng dụng.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 👇 Chạy form đăng ký thay vì Form1
            Application.Run(new FrmRegister());
        }
    }
}
