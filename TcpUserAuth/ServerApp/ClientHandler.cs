using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace ServerApp
{
    internal class ClientHandler
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private StreamReader _reader;
        private StreamWriter _writer;

        public ClientHandler(TcpClient client)
        {
            _client = client;
            _stream = _client.GetStream();
            _reader = new StreamReader(_stream, Encoding.UTF8);
            _writer = new StreamWriter(_stream, Encoding.UTF8) { AutoFlush = true };
        }

        public void Start()
        {
            try
            {
                while (true)
                {
                    string request = _reader.ReadLine();
                    if (string.IsNullOrEmpty(request)) break;

                    // Ví dụ: "LOGIN|alice|123"
                    string[] parts = request.Split('|');
                    string command = parts[0].ToUpper();

                    switch (command)
                    {
                        case "LOGIN":
                            HandleLogin(parts);
                            break;
                        case "REGISTER":
                            HandleRegister(parts);
                            break;
                        default:
                            _writer.WriteLine("ERROR|UnknownCommand");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ClientHandler Error: " + ex.Message);
            }
            finally
            {
                _client.Close();
            }
        }

        private void HandleLogin(string[] parts)
        {
            if (parts.Length < 3)
            {
                _writer.WriteLine("ERROR|InvalidLoginFormat");
                return;
            }

            string username = parts[1];
            string password = parts[2];

            int userId = DatabaseHelper.LoginUserAndGetUserId(username, password);

            if (userId > 0)
            {
                string token = TokenManager.GenerateToken(userId);
                _writer.WriteLine($"OK|LoginSuccess|{token}");
            }
            else
            {
                _writer.WriteLine("ERROR|InvalidCredentials");
            }
        }

        private void HandleRegister(string[] parts)
        {
            if (parts.Length < 5)
            {
                _writer.WriteLine("ERROR|InvalidRegisterFormat");
                return;
            }

            string username = parts[1];
            string password = parts[2];
            string fullname = parts[3];
            string email = parts[4];

            var user = new SharedModels.User
            {
                Username = username,
                PasswordHash = DatabaseHelper.HashPassword(password),
                FullName = fullname,
                Email = email
            };

            bool success = new DatabaseHelper().RegisterUser(user);

            if (success)
                _writer.WriteLine("OK|RegisterSuccess");
            else
                _writer.WriteLine("ERROR|RegisterFailed");
        }

    }
}

