//using SharedModels;
//using System;
//using System.IO;
//using System.Net.Sockets;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.Security.Cryptography;
//using System.Text;
//using System.Windows.Forms;
//using SharedModels;

//using static System.Runtime.InteropServices.JavaScript.JSType;

//namespace ClientApp
//{
//    public partial class FrmRegister : Form
//    {
//        public FrmRegister()
//        {
//            InitializeComponent();
//        }

//        private void label1_Click(object sender, EventArgs e)
//        {
//            // placeholder nếu label có event
//        }

//        // Nút ĐĂNG KÝ (đảm bảo Designer gán event này cho button đăng ký)

//        private void button1_Click(object sender, EventArgs e)
//        {
//            string username = textBox1.Text.Trim();
//            string password = textBox2.Text.Trim();
//            string confirm = textBox3.Text.Trim();
//            string fullname = textBox4.Text.Trim();
//            string email = textBox5.Text.Trim();

//            if (string.IsNullOrEmpty(username) ||
//                string.IsNullOrEmpty(password) ||
//                string.IsNullOrEmpty(confirm) ||
//                string.IsNullOrEmpty(fullname) ||
//                string.IsNullOrEmpty(email))
//            {
//                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }

//            if (password != confirm)
//            {
//                MessageBox.Show("Mật khẩu xác nhận không khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }

//            try
//            {
//                // Băm mật khẩu
//                string passwordHash = ComputeSha256Hash(password);

//                // Tạo request đăng ký
//                RequestMessage req = new RequestMessage
//                {
//                    Action = "Register",
//                    Username = username,
//                    PasswordHash = passwordHash,
//                    FullName = fullname,
//                    Email = email
//                };

//                // Serialize sang JSON + thêm newline để server nhận dạng gói
//                string json = Utilities.ToJson(req) + "\n";
//                byte[] data = Encoding.UTF8.GetBytes(json);

//                using (TcpClient client = new TcpClient("127.0.0.1", 8080))
//                using (NetworkStream stream = client.GetStream())
//                {
//                    // Gửi gói đăng ký
//                    stream.Write(data, 0, data.Length);

//                    // Nhận phản hồi (ResponseMessage)
//                    byte[] buffer = new byte[4096];
//                    int n = stream.Read(buffer, 0, buffer.Length);
//                    string respJson = Encoding.UTF8.GetString(buffer, 0, n);
//                    ResponseMessage resp = Utilities.FromJson<ResponseMessage>(respJson);

//                    if (resp != null && resp.Success)
//                    {
//                        MessageBox.Show(resp.Message ?? "Đăng ký thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                        this.Close();
//                    }
//                    else
//                    {
//                        MessageBox.Show(resp?.Message ?? "Đăng ký thất bại!", "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi kết nối server: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }


//        // Nút HỦY (đảm bảo Designer gán event này cho button hủy)
//        private void button2_Click(object sender, EventArgs e)
//        {
//            this.Close();
//        }

//        // HÀM BĂM SHA256 — PHẢI NẰM TRONG CLASS FrmRegister
//        private string ComputeSha256Hash(string rawData)
//        {
//            using (SHA256 sha = SHA256.Create())
//            {
//                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(rawData));
//                StringBuilder sb = new StringBuilder();
//                foreach (byte b in bytes)
//                    sb.Append(b.ToString("x2"));
//                return sb.ToString();
//            }
//        }
//    }
//}



using SharedModels;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientApp
{
    public partial class FrmRegister : Form
    {
        public FrmRegister()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();
            string confirm = textBox3.Text.Trim();
            string fullname = textBox4.Text.Trim();
            string email = textBox5.Text.Trim();

            // Kiểm tra đầu vào
            if (string.IsNullOrEmpty(username) ||
                string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(confirm) ||
                string.IsNullOrEmpty(fullname) ||
                string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confirm)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string passwordHash = ComputeSha256Hash(password);

                // Tạo yêu cầu đăng ký
                var req = new RequestMessage
                {
                    Action = "Register",
                    Username = username,
                    PasswordHash = passwordHash,
                    FullName = fullname,
                    Email = email
                };

                MessageBox.Show(Utilities.ToJson(req), "JSON gửi lên Server");

                using (var tcp = new TcpClientHelper())
                {
                    // Kết nối đến server (chỉnh port nếu server khác)
                    await tcp.ConnectAsync("127.0.0.1", 8080);

                    // Gửi gói JSON
                    await tcp.SendPacketAsync(req);

                    // Nhận phản hồi
                    var resp = await tcp.ReceivePacketAsync();

                    if (resp != null && resp.Success)
                    {
                        MessageBox.Show(resp.Message ?? "Đăng ký thành công!",
                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(resp?.Message ?? "Đăng ký thất bại!",
                            "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối server: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Nút HỦY
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Hàm băm SHA256
        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
    }
}
