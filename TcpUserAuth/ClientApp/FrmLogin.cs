using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharedModels;

namespace ClientApp
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        // ✅ Bắt sự kiện nhấn nút Login (đã hỗ trợ await)
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // Kiểm tra nhập trống
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) ||
                username == "Username" || password == "Password")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Username và Password!", "Thiếu thông tin",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnLogin.Enabled = false; // Tạm khóa nút tránh nhấn liên tục

            try
            {
                var client = new TcpClientHelper();
                bool connected = await client.ConnectAsync();

                if (!connected)
                {
                    MessageBox.Show("Không thể kết nối đến server.", "Lỗi mạng",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var request = new RequestMessage
                {
                    Action = "Login",
                    Data = new { Username = username, Password = password }
                };

                var response = await client.SendRequestAsync(request);

                if (response != null && response.Success)
                {
                    // ✅ Lưu thông tin session
                    SessionManager.Username = username;
                    SessionManager.Token = response.Token;

                    MessageBox.Show("Đăng nhập thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Mở form chính và ẩn form login
                    FrmMain frmMain = new FrmMain();
                    frmMain.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show(response?.Message ?? "Đăng nhập thất bại!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                client.Disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi đăng nhập",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLogin.Enabled = true; // Mở lại nút
            }
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            SetPlaceholder(txtUsername, "Username");
            SetPlaceholder(txtPassword, "Password", isPassword: true);
        }

        // ✅ Hàm tạo placeholder cho textbox
        private void SetPlaceholder(TextBox textBox, string placeholder, bool isPassword = false)
        {
            textBox.Text = placeholder;
            textBox.ForeColor = Color.Gray;
            textBox.UseSystemPasswordChar = false;

            textBox.GotFocus += (s, e) =>
            {
                if (textBox.Text == placeholder)
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.Black;
                    if (isPassword)
                        textBox.UseSystemPasswordChar = true;
                }
            };

            textBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.UseSystemPasswordChar = false;
                    textBox.Text = placeholder;
                    textBox.ForeColor = Color.Gray;
                }
            };
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmRegister registerForm = new FrmRegister();
            registerForm.ShowDialog();
        }
    }
}
