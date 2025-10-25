////////using System;
////////using System.Collections.Generic;
////////using System.Linq;
////////using System.Text;
////////using System.Threading.Tasks;

////////namespace SharedModels
////////{
////////    internal class Packetcs
////////    {
////////    }
////////}

////using System;

////namespace SharedModels
////{
////    [Serializable]
////    public class Packet
////    {
////        public string Command { get; set; }     // Lệnh: "REGISTER", "LOGIN", ...
////        public string Data { get; set; }        // Dữ liệu JSON (User, thông báo...)
////        public string Token { get; set; }       // Token xác thực (nếu cần)

////        public Packet() { }

////        public Packet(string command, object data, string token = null)
////        {
////            Command = command;
////            Data = data;
////            Token = token;
////        }
////    }
////}


//////using System;

//////namespace SharedModels
//////{
//////    [Serializable]
//////    public class Packet
//////    {
//////        public string Type { get; set; }      // loại gói tin (vd: "Register")
//////        public object Data { get; set; }      // dữ liệu gửi kèm (có thể là User, string, v.v.)

//////        public Packet(string type, object data)
//////        {
//////            Type = type;
//////            Data = data;
//////        }
//////    }
//////}
/////

//using System;

//namespace SharedModels
//{
//    [Serializable]
//    public class Packet
//    {
//        public string Command { get; set; }   // ✅ thay vì Type
//        public object Data { get; set; }      // ✅ cho phép gửi User hoặc string
//        public string Token { get; set; }

//        public Packet() { }

//        public Packet(string command, object data, string token = null)
//        {
//            Command = command;
//            Data = data;    // ✅ hợp lệ vì cùng kiểu object
//            Token = token;
//        }
//    }
//}


//////using System;
//////using System.Collections.Generic;
//////using System.Linq;
//////using System.Text;
//////using System.Threading.Tasks;

//////namespace SharedModels
//////{
//////    internal class Packetcs
//////    {
//////    }
//////}

//using System;

//namespace SharedModels
//{
//    [Serializable]
//    public class Packet
//    {
//        public string Command { get; set; }     // Lệnh: "REGISTER", "LOGIN", ...
//        public string Data { get; set; }        // Dữ liệu JSON (User, thông báo...)
//        public string Token { get; set; }       // Token xác thực (nếu cần)

//        public Packet() { }

//        public Packet(string command, object data, string token = null)
//        {
//            Command = command;
//            Data = data;
//            Token = token;
//        }
//    }
//}


////using System;

////namespace SharedModels
////{
////    [Serializable]
////    public class Packet
////    {
////        public string Type { get; set; }      // loại gói tin (vd: "Register")
////        public object Data { get; set; }      // dữ liệu gửi kèm (có thể là User, string, v.v.)

////        public Packet(string type, object data)
////        {
////            Type = type;
////            Data = data;
////        }
////    }
////}
///

using System;

namespace SharedModels
{
    [Serializable]
    public class Packet
    {
        public string Command { get; set; }   // ✅ thay vì Type
        public object Data { get; set; }      // ✅ cho phép gửi User hoặc string
        public string Token { get; set; }

        public Packet() { }

        public Packet(string command, object data, string token = null)
        {
            Command = command;
            Data = data;    // ✅ hợp lệ vì cùng kiểu object
            Token = token;
        }
    }
}

