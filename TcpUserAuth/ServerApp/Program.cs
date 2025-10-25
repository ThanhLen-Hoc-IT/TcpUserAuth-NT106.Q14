//using System;
//using System.Configuration;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading;
//using System.Windows.Forms;

//namespace ServerApp
//{
//    internal static class Program
//    //{
//        private static TcpListener listener;

//        [STAThread]
//        static void Main()
//        {
//            Application.EnableVisualStyles();
//            Application.SetCompatibleTextRenderingDefault(false);

//            // Đọc port từ App.config
//            int port = 8080; // giá trị mặc định
//            try
//            {
//                string portValue = ConfigurationManager.AppSettings["ServerPort"];
//                if (!string.IsNullOrEmpty(portValue))
//                    port = int.Parse(portValue);
//            }
//            catch
//            {
//                MessageBox.Show("Không đọc được cổng từ App.config. Sử dụng mặc định 8080.", "Thông báo");
//            }

//            // Khởi tạo TCP server chạy nền
//            Thread serverThread = new Thread(() => StartServer(port));
//            serverThread.IsBackground = true;
//            serverThread.Start();

//            // Mở giao diện chính (Form1)
//            Application.Run(new Form1());
//        }

//        private static void StartServer(int port)
//        {
//            try
//            {
//                listener = new TcpListener(IPAddress.Any, port);
//                listener.Start();

//                Console.WriteLine($"✅ Server đang lắng nghe tại 127.0.0.1:{port}");
//                Console.WriteLine("Chờ client kết nối...");

//                while (true)
//                {
//                    TcpClient client = listener.AcceptTcpClient();
//                    Console.WriteLine("📶 Client mới đã kết nối!");
//                    ThreadPool.QueueUserWorkItem(HandleClient, client);
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi khởi động server: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        private static void HandleClient(object clientObj)
//        {
//            try
//            {
//                TcpClient client = (TcpClient)clientObj;
//                NetworkStream ns = client.GetStream();

//                byte[] lenBuffer = new byte[4];
//                ns.Read(lenBuffer, 0, 4);
//                int len = BitConverter.ToInt32(lenBuffer, 0);

//                byte[] dataBuffer = new byte[len];
//                ns.Read(dataBuffer, 0, len);
//                string json = Encoding.UTF8.GetString(dataBuffer);

//                Console.WriteLine("📦 Nhận từ client: " + json);

//                // Gửi phản hồi đơn giản (giả lập xử lý đăng ký)
//                string responseJson = "{\"Command\":\"REGISTER_SUCCESS\",\"Data\":\"Đăng ký thành công!\"}";
//                byte[] respData = Encoding.UTF8.GetBytes(responseJson);
//                byte[] respLen = BitConverter.GetBytes(respData.Length);
//                ns.Write(respLen, 0, 4);
//                ns.Write(respData, 0, respData.Length);

//                client.Close();
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("❌ Lỗi xử lý client: " + ex.Message);
//            }
//        }
//    }
//}

using System;
using System.Windows.Forms;

namespace ServerApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1()); // chỉ chạy Form1, không tự tạo server ở đây nữa
        }
    }
}

