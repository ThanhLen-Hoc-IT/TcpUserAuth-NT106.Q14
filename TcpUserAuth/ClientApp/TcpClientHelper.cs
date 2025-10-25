using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SharedModels;

namespace ClientApp
{
    internal class TcpClientHelper
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
        /// Gửi RequestMessage đến server và chờ nhận ResponseMessage.
        /// </summary>
        public async Task<ResponseMessage> SendRequestAsync(RequestMessage request)
        {
            if (_client == null || !_client.Connected)
            {
                throw new InvalidOperationException("Client chưa kết nối server!");
            }

            try
            {
                // Gửi request dạng JSON
                var json = Utilities.ToJson(request) + "\n";
                var data = Encoding.UTF8.GetBytes(json);
                await _stream.WriteAsync(data, 0, data.Length);

                // Nhận phản hồi
                var buffer = new byte[4096];
                var sb = new StringBuilder();

                while (true)
                {
                    int n = await _stream.ReadAsync(buffer, 0, buffer.Length);
                    if (n == 0) break;

                    sb.Append(Encoding.UTF8.GetString(buffer, 0, n));

                    if (sb.ToString().Contains("\n"))
                    {
                        var line = sb.ToString().Split('\n')[0];
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
        /// Đóng kết nối TCP
        /// </summary>
        public void Disconnect()
        {
            try
            {
                _stream?.Close();
                _client?.Close();
            }
            catch { }
        }
    }
}
