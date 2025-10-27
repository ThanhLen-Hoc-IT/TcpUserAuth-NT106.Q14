<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Linq;
=======
﻿using SharedModels;
using System;
>>>>>>> 823bd52597aa9b2f0146871a60a6b9c7a4edf517
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

<<<<<<< HEAD
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
=======
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
>>>>>>> 823bd52597aa9b2f0146871a60a6b9c7a4edf517
        {
            try
            {
                _stream?.Close();
                _client?.Close();
            }
            catch { }
        }
<<<<<<< HEAD
=======

        public void Close()
        {
            Dispose();
        }
>>>>>>> 823bd52597aa9b2f0146871a60a6b9c7a4edf517
    }
}
