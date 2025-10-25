using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using SharedModels;

namespace ClientApp
{
    public partial class FrmRegister : Form
    {
        public FrmRegister()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // placeholder nếu label có event
        }

        // Nút ĐĂNG KÝ (đảm bảo Designer gán event này cho button đăng ký)
        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();
            string confirm = textBox3.Text.Trim();
            string fullname = textBox4.Text.Trim();
            string email = textBox5.Text.Trim();

            if (string.IsNullOrEmpty(username) ||
                string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(confirm) ||
                string.IsNullOrEmpty(fullname) ||
                string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confirm)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string passwordHash = ComputeSha256Hash(password);
                User user = new User(username, passwordHash, fullname, email);

                Packet packet = new Packet("REGISTER", user);

                TcpClient client = new TcpClient("127.0.0.1", 8080); // sửa port nếu server khác
                NetworkStream stream = client.GetStream();
                BinaryFormatter bf = new BinaryFormatter();

                bf.Serialize(stream, packet);

                // nhận phản hồi (server có thể trả Packet hoặc string)
                object respObj = bf.Deserialize(stream);
                string respMsg;
                if (respObj is Packet respPacket)
                    respMsg = respPacket.Data?.ToString() ?? "Không có nội dung trả về";
                else
                    respMsg = respObj?.ToString() ?? "Không có phản hồi";

                MessageBox.Show(respMsg, "Kết quả", MessageBoxButtons.OK, MessageBoxIcon.Information);

                stream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối server: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Nút HỦY (đảm bảo Designer gán event này cho button hủy)
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // HÀM BĂM SHA256 — PHẢI NẰM TRONG CLASS FrmRegister
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
