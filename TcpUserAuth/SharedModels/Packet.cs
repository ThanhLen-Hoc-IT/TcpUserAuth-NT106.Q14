using System;

namespace SharedModels
{
    [Serializable]
    public class Packet
    {
        public string Command { get; set; }   // loại lệnh
        public object Data { get; set; }      // dữ liệu gửi kèm
        public string Token { get; set; }     // mã xác thực

        public Packet() { }

        public Packet(string command, object data, string token = null)
        {
            Command = command;
            Data = data;
            Token = token;
        }
    }
}
