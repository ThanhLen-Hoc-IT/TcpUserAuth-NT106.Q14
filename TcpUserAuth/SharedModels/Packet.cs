using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels
{
    internal class Packet
    {
        public string Message { get; set; }

        public byte[] ToBytes()
        {
            var bytes = Encoding.UTF8.GetBytes(Message);
            var len = BitConverter.GetBytes(bytes.Length);
            return len.Concat(bytes).ToArray();
        }

        public static Packet FromBytes(byte[] data)
        {
            return new Packet { Message = Encoding.UTF8.GetString(data) };
        }
    }
}
