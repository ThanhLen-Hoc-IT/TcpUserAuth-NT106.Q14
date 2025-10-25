using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;
using SharedModels;

namespace ServerApp
{
    //public partial class Form1 : Form
    //{
    //    private TcpListener server;
    //    private Thread listenThread;
    //    private readonly string userFile = "users.txt";
    //    private volatile bool running = false;

    public partial class Form1 : Form
    {
        private readonly string userFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users.txt");

        private TcpListener server;
        private Thread listenThread;
        private bool running = false;

     

        public Form1()
        {
            InitializeComponent();
            // đảm bảo form load event được đăng ký nếu designer chưa làm
            this.Load -= Form1_Load; // an toàn: bỏ trước để tránh double subscription
            this.Load += Form1_Load;
            this.FormClosing -= Form1_FormClosing;
            this.FormClosing += Form1_FormClosing;
        }

        //private void Form1_Load(object sender, EventArgs e)
        //{
        //    StartServer();
        //}

        //private void StartServer()

        //{
        //    MessageBox.Show("Server thật sự đã start!");

        //    try
        //    {
        //        if (running) return;
        //        server = new TcpListener(IPAddress.Any, 8080);
        //        server.Start();

        //        running = true;
        //        listenThread = new Thread(ListenForClients);
        //        listenThread.IsBackground = true;
        //        listenThread.Start();
        //        // cập nhật tiêu đề form an toàn với UI thread
        //        UpdateTitle("Server đang chạy trên cổng 8080...");
        //        Log("Server started on port 8080");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Lỗi khởi động server: " + ex.Message);
        //    }
        //}

        private void Form1_Load(object sender, EventArgs e)
        {
            StartServer();
        }

        private void StartServer()

        {
            MessageBox.Show("Server thật sự đã start!");

            try
            {
                if (running) return;
                server = new TcpListener(IPAddress.Any, 8080);
                server.Start();

                running = true;
                listenThread = new Thread(ListenForClients);
                listenThread.IsBackground = true;
                listenThread.Start();
                // cập nhật tiêu đề form an toàn với UI thread
                UpdateTitle("Server đang chạy trên cổng 8080...");
                Log("Server started on port 8080");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi động server: " + ex.Message);
            }
        }

        private void ListenForClients()
        {
            try
            {
                while (running)
                {
                    TcpClient client = server.AcceptTcpClient(); // blocking
                    Thread clientThread = new Thread(HandleClientComm);
                    clientThread.IsBackground = true;
                    clientThread.Start(client);
                }
            }
            catch (SocketException se)
            {
                // socket closed khi dừng server
                Log("Listener stopped: " + se.Message);
            }
            catch (Exception ex)
            {
                Log("ListenForClients exception: " + ex.Message);
            }
        }

        private void HandleClientComm(object clientObj)
        {
            TcpClient client = clientObj as TcpClient;
            if (client == null) return;

            string clientEndPoint = client.Client.RemoteEndPoint?.ToString() ?? "unknown";
            Log($"Client connected: {clientEndPoint}");

            using (NetworkStream stream = client.GetStream())
            {
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    var obj = bf.Deserialize(stream);
                    if (obj is Packet packet)
                    {
                        if (packet.Command.Equals("REGISTER", StringComparison.OrdinalIgnoreCase))

                        {
                            // packet.Data was serialized as User object
                            if (packet.Data is User newUser)
                            {
                                string result = RegisterUser(newUser);
                                bf.Serialize(stream, result);
                                Log($"Register attempt: {newUser.Username} -> {result}");
                            }
                            else
                            {
                                // try to handle if Data was stringified JSON - fallback
                                bf.Serialize(stream, "Invalid user data");
                            }
                        }
                        else
                        {
                            Packet resp = new Packet("ERROR", "Unknown packet type");
                            bf.Serialize(stream, resp);

                        }
                    }
                    else
                    {
                        // if client sent something else
                        bf.Serialize(stream, "Invalid packet");
                    }
                }
                catch (Exception ex)
                {
                    Log("HandleClientComm exception: " + ex.Message);
                }
                finally
                {
                    try { client.Close(); } catch { }
                }
            }
        }

        private string RegisterUser(User user)
        {
            try
            {
                // đảm bảo file path là ở folder chạy của server (bin\Debug)
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, userFile);
                var existing = new List<string>();
                if (File.Exists(path))
                    existing.AddRange(File.ReadAllLines(path));

                foreach (string line in existing)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    string[] parts = line.Split('|');
                    if (parts.Length > 0 && parts[0].Equals(user.Username, StringComparison.OrdinalIgnoreCase))
                        return "Tên đăng nhập đã tồn tại!";
                }

                using (StreamWriter sw = File.AppendText(path))
                {
                    // Lưu password thẳng (vì client gửi plaintext theo thiết kế hiện tại),
                    // nếu bạn muốn mã hóa hãy đổi ở client và server tương ứng.
                    sw.WriteLine($"{user.Username}|{user.PasswordHash}|{user.FullName}|{user.Email}");
                }

                return "Đăng ký thành công!";
            }
            catch (Exception ex)
            {
                Log("RegisterUser exception: " + ex.Message);
                return "Lỗi khi lưu người dùng";
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopServer();
        }

        private void StopServer()
        {
            try
            {
                running = false;
                if (server != null)
                {
                    server.Stop();
                    server = null;
                }
                if (listenThread != null && listenThread.IsAlive)
                {
                    try { listenThread.Join(500); } catch { }
                    listenThread = null;
                }
                UpdateTitle("Server đã dừng");
                Log("Server stopped");
            }
            catch (Exception ex)
            {
                Log("StopServer exception: " + ex.Message);
            }
        }

        // Helper: cập nhật UI an toàn
        private void UpdateTitle(string text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => this.Text = text));
            }
            else
            {
                this.Text = text;
            }
        }

        // Helper: ghi log vào ListBox nếu bạn có control lstLog, nếu không sẽ bỏ qua
        private void Log(string message)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => Log(message)));
                    return;
                }

                // nếu bạn có ListBox tên lstLog trên Form1, thêm vào
                var lb = this.Controls["lstLog"] as ListBox;
                if (lb != null)
                {
                    lb.Items.Add($"{DateTime.Now:HH:mm:ss} - {message}");
                }
                else
                {
                    // nếu không có control, ta vẫn show vào Debug output
                    System.Diagnostics.Debug.WriteLine(message);
                }
            }
            catch { }
        }
    }
}
