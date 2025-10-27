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

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            var client = new TcpClientHelper();
            bool connected = await client.ConnectAsync();

            if (!connected)
            {
                MessageBox.Show("Không kết nối được server!");
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
                MessageBox.Show("Đăng nhập thành công!");
                FrmMain frmMain = new FrmMain();
                frmMain.Show();
            }
            else
            {
                MessageBox.Show(response?.Message ?? "Đăng nhập thất bại!");
            }

            client.Disconnect();
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            SetPlaceholder(txtUsername, "Username");
            SetPlaceholder(txtPassword, "Password", isPassword: true);
        }

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
