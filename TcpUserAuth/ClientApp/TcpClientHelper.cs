using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SharedModels;

namespace ClientApp
{
    public class TcpClientHelper : IDisposable
    {
        private TcpClient _client;
        private NetworkStream _stream;

        public bool IsConnected => _client?.Connected ?? false;

        /// <summary>
        /// Kết nối tới server (mặc định 127.0.0.1:8888)
        /// </summary>
        public async Task<bool> ConnectAsync(string host = "127.0.0.1", int port = 8888)
        {
            try
            {
                if (IsConnected) return true;

                _client = new TcpClient();
                await _client.ConnectAsync(host, port);
                _stream = _client.GetStream();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Connect error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Gửi RequestMessage và chờ nhận ResponseMessage (dạng JSON, kết thúc bằng '\n')
        /// </summary>
        public async Task<ResponseMessage> SendRequestAsync(RequestMessage request)
        {
            if (!IsConnected)
                throw new InvalidOperationException("Client chưa kết nối server!");

            try
            {
                // Gửi request
                string json = Utilities.ToJson(request) + "\n";
                byte[] data = Encoding.UTF8.GetBytes(json);
                await _stream.WriteAsync(data, 0, data.Length);

                // Nhận phản hồi
                byte[] buffer = new byte[4096];
                StringBuilder sb = new StringBuilder();

                while (true)
                {
                    int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break; // Server đóng kết nối

                    sb.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));

                    if (sb.ToString().Contains("\n"))
                    {
                        string line = sb.ToString().Split('\n')[0];
                        var resp = Utilities.FromJson<ResponseMessage>(line);
                        return resp;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Send/Receive error: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Gửi gói tin mà không cần chờ phản hồi
        /// </summary>
        public async Task SendPacketAsync(RequestMessage req)
        {
            if (!IsConnected)
                throw new InvalidOperationException("Client chưa kết nối server!");

            string json = Utilities.ToJson(req) + "\n";
            byte[] data = Encoding.UTF8.GetBytes(json);
            await _stream.WriteAsync(data, 0, data.Length);
        }

        /// <summary>
        /// Nhận phản hồi (nếu server chủ động gửi về)
        /// </summary>
        public async Task<ResponseMessage> ReceivePacketAsync()
        {
            if (!IsConnected)
                throw new InvalidOperationException("Client chưa kết nối server!");

            byte[] buffer = new byte[4096];
            int n = await _stream.ReadAsync(buffer, 0, buffer.Length);
            if (n == 0) return null;

            string json = Encoding.UTF8.GetString(buffer, 0, n);
            if (json.Contains("\n"))
                json = json.Substring(0, json.IndexOf('\n'));

            return Utilities.FromJson<ResponseMessage>(json);
        }

        /// <summary>
        /// Đóng kết nối TCP
        /// </summary>
        public void Disconnect()
        {
            Dispose();
        }

        public void Dispose()
        {
            try
            {
                _stream?.Close();
                _client?.Close();
            }
            catch { }
        }

        public void Close() => Dispose();
    }
}
