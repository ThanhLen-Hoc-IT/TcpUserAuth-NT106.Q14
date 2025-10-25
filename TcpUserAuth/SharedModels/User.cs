//////////using System;
//////////using System.Collections.Generic;
//////////using System.Linq;
//////////using System.Text;
//////////using System.Threading.Tasks;

//////////namespace SharedModels
//////////{
//////////    internal class User
//////////    {
//////////    }
//////////}

////////using System;

////////namespace SharedModels
////////{
////////    [Serializable]
////////    public class User
////////    {
////////        public string Username { get; set; }
////////        public string PasswordHash { get; set; }    // Mật khẩu đã băm SHA256
////////        public string FullName { get; set; }
////////        public string Email { get; set; }

////////        public User() { }

////////        public User(string username, string passwordHash, string fullName, string email)
////////        {
////////            Username = username;
////////            PasswordHash = passwordHash;
////////            FullName = fullName;
////////            Email = email;
////////        }

////////        public override string ToString()
////////        {
////////            return $"{FullName} ({Username}) - {Email}";
////////        }
////////    }
////////}

//////using System;

//////namespace SharedModels
//////{
//////    [Serializable]
//////    public class User
//////    {
//////        public string Username { get; set; }
//////        public string Password { get; set; }   // ✅ đây là dòng bị thiếu
//////        public string FullName { get; set; }
//////        public string Email { get; set; }

//////        public User(string username, string password, string fullName, string email)
//////        {
//////            Username = username;
//////            Password = password;
//////            FullName = fullName;
//////            Email = email;
//////        }

//////        public User() { } // thêm constructor rỗng cho an toàn khi deserialize
//////    }
//////}



////using System;

////namespace SharedModels
////{
////    [Serializable]
////    public class User
////    {
////        public string Username { get; set; }
////        public string PasswordHash { get; set; }    // Mật khẩu đã băm SHA256
////        public string FullName { get; set; }
////        public string Email { get; set; }

////        public User() { }

////        public User(string username, string passwordHash, string fullName, string email)
////        {
////            Username = username;
////            PasswordHash = passwordHash;
////            FullName = fullName;
////            Email = email;
////        }

////        public override string ToString()
////        {
////            return $"{FullName} ({Username}) - {Email}";
////        }
////    }
////}


//using System;

//namespace SharedModels
//{
//    [Serializable]
//    public class User
//    {
//        public string Username { get; set; }
//        public string PasswordHash { get; set; }   // hoặc 'Password' nếu bạn không băm
//        public string FullName { get; set; }
//        public string Email { get; set; }

//        public User() { }

//        public User(string username, string passwordHash, string fullName, string email)
//        {
//            Username = username;
//            PasswordHash = passwordHash;
//            FullName = fullName;
//            Email = email;
//        }
//    }
//}


using System;

namespace SharedModels
{
    [Serializable]
    public class User
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }   // ✅ dùng PasswordHash, KHÔNG dùng Password
        public string FullName { get; set; }
        public string Email { get; set; }

        public User() { }

        public User(string username, string passwordHash, string fullName, string email)
        {
            Username = username;
            PasswordHash = passwordHash;
            FullName = fullName;
            Email = email;
        }
    }
}

