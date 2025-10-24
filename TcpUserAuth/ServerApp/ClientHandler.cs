using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Text.Json;
using SharedModels;

namespace ServerApp
{
    public class ClientHandler
    {
        private readonly TcpClient _client;

        public ClientHandler(TcpClient client)
        {
            _client = client;
        }

        public async Task RunAsync()
        {
            using var c = _client;
            using var stream = c.GetStream();
            var buffer = new byte[4096];
            var sb = new StringBuilder();

            try
            {
                while (true)
                {
                    int n = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (n == 0) break;
                    sb.Append(Encoding.UTF8.GetString(buffer, 0, n));
                    if (!sb.ToString().Contains("\n")) continue;

                    var line = sb.ToString();
                    var idx = line.IndexOf('\n');
                    var one = line[..idx];
                    sb.Clear();
                    if (idx + 1 < line.Length) sb.Append(line[(idx + 1)..]);

                    var req = Utilities.FromJson<RequestMessage>(one);
                    if (req == null)
                    {
                        await SendAsync(stream, new ResponseMessage { Success = false, Message = "Yêu cầu không hợp lệ." });
                        continue;
                    }

                    var resp = Handle(req);
                    await SendAsync(stream, resp);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Client error: " + ex.Message);
            }
        }

        private static Task SendAsync(NetworkStream stream, ResponseMessage resp)
        {
            var json = Utilities.ToJson(resp);
            var data = Encoding.UTF8.GetBytes(json + "\n");
            return stream.WriteAsync(data, 0, data.Length);
        }

        private static ResponseMessage Handle(RequestMessage req)
        {
            return req.Action switch
            {
                "Register" => DoRegister(req),
                "Login" => DoLogin(req),
                "GetProfile" => DoGetProfile(req),
                "Logout" => DoLogout(req),
                _ => new ResponseMessage { Success = false, Message = "Action không được hỗ trợ." }
            };
        }

        private static ResponseMessage DoRegister(RequestMessage req)
        {
            if (string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.PasswordHash))
                return new ResponseMessage { Success = false, Message = "Thiếu username hoặc password." };
            try
            {
                if (DatabaseHelper.UsernameExists(req.Username))
                    return new ResponseMessage { Success = false, Message = "Username đã tồn tại." };
                DatabaseHelper.InsertUser(req);
                return new ResponseMessage { Success = true, Message = "Đăng ký thành công!" };
            }
            catch (Exception ex)
            {
                Logger.Error("Register error: " + ex.Message);
                return new ResponseMessage { Success = false, Message = "Lỗi hệ thống khi đăng ký." };
            }
        }

        private static ResponseMessage DoLogin(RequestMessage req)
        {
            try
            {
                var got = DatabaseHelper.GetUserByUsername(req.Username);
                if (!got.ok || got.hash == null) return new ResponseMessage { Success = false, Message = "Sai username hoặc password." };
                if (!string.Equals(got.hash, req.PasswordHash, StringComparison.OrdinalIgnoreCase))
                    return new ResponseMessage { Success = false, Message = "Sai username hoặc password." };

                var token = TokenManager.Issue(req.Username);
                return new ResponseMessage { Success = true, Message = "Đăng nhập thành công.", User = got.user, Token = token };
            }
            catch (Exception ex)
            {
                Logger.Error("Login error: " + ex.Message);
                return new ResponseMessage { Success = false, Message = "Lỗi hệ thống khi đăng nhập." };
            }
        }

        private static ResponseMessage DoGetProfile(RequestMessage req)
        {
            if (!TokenManager.Validate(req.Token, out var username))
                return new ResponseMessage { Success = false, Message = "Token không hợp lệ hoặc hết hạn." };

            var got = DatabaseHelper.GetUserByUsername(username);
            if (!got.ok || got.user == null) return new ResponseMessage { Success = false, Message = "Không tìm thấy user." };
            return new ResponseMessage { Success = true, User = got.user, Message = "OK" };
        }

        private static ResponseMessage DoLogout(RequestMessage req)
        {
            TokenManager.Revoke(req.Token);
            return new ResponseMessage { Success = true, Message = "Đã đăng xuất." };
        }
    }

}
