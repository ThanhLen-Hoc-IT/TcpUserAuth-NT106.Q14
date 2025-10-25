using SharedModels;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    public class TcpClientHelper : IDisposable
    {
        private TcpClient _client;
        private NetworkStream _stream;

        public bool IsConnected => _client?.Connected ?? false;

        public async Task ConnectAsync(string ip, int port)
        {
            if (IsConnected) return;

            _client = new TcpClient();
            await _client.ConnectAsync(ip, port);
            _stream = _client.GetStream();
        }

        public async Task SendPacketAsync(RequestMessage req)
        {
            if (!IsConnected)
                throw new InvalidOperationException("Client chưa kết nối server!");

            string json = Utilities.ToJson(req) + "\n";
            byte[] data = Encoding.UTF8.GetBytes(json);
            await _stream.WriteAsync(data, 0, data.Length);
        }

        public async Task<ResponseMessage> ReceivePacketAsync()
        {
            if (!IsConnected)
                throw new InvalidOperationException("Client chưa kết nối server!");

            byte[] buffer = new byte[4096];
            int n = await _stream.ReadAsync(buffer, 0, buffer.Length);
            string json = Encoding.UTF8.GetString(buffer, 0, n);

            // Một số server có thể gửi nhiều gói => chỉ lấy phần đầu tiên trước dấu xuống dòng
            if (json.Contains("\n"))
                json = json.Substring(0, json.IndexOf('\n'));

            var resp = Utilities.FromJson<ResponseMessage>(json);
            return resp;
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

        public void Close()
        {
            Dispose();
        }
    }
}
