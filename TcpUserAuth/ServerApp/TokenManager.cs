//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Timers;
//using System.Collections.Concurrent;


//namespace ServerApp
//{
//    public static class TokenManager
//    {
//        private static readonly ConcurrentDictionary<string, (string Username, DateTime Expire)> _tokens = new();
//        private static readonly System.Timers.Timer _cleaner;

//        static TokenManager()
//        {
//            _cleaner = new System.Timers.Timer(60_000); // every minute
//            _cleaner.Elapsed += (_, __) => Cleanup();
//            _cleaner.Start();
//        }

//        public static string Issue(string username, TimeSpan? ttl = null)
//        {
//            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
//            _tokens[token] = (username, DateTime.UtcNow.Add(ttl ?? TimeSpan.FromHours(1)));
//            return token;
//        }

//        public static bool Validate(string? token, out string username)
//        {
//            username = "";
//            if (token == null) return false;
//            if (!_tokens.TryGetValue(token, out var info)) return false;
//            if (info.Expire < DateTime.UtcNow) { _tokens.TryRemove(token, out _); return false; }
//            username = info.Username;
//            return true;
//        }

//        public static void Revoke(string? token)
//        {
//            if (token != null) _tokens.TryRemove(token, out _);
//        }

//        private static void Cleanup()
//        {
//            var expired = _tokens.Where(kv => kv.Value.Expire < DateTime.UtcNow).Select(kv => kv.Key).ToList();
//            foreach (var t in expired) _tokens.TryRemove(t, out _);
//        }
//    }

//}

using System;
using System.Collections.Concurrent;

namespace ServerApp
{
    // Quản lý token đăng nhập cho người dùng
    public static class TokenManager
    {
        private static readonly ConcurrentDictionary<string, string> Tokens = new();

        // Cấp phát token mới cho username
        public static string Issue(string username)
        {
            var token = Guid.NewGuid().ToString();
            Tokens[token] = username;
            return token;
        }

        // Kiểm tra token có hợp lệ không
        public static bool Validate(string token, out string username)
        {
            return Tokens.TryGetValue(token, out username);
        }

        

        //thu hồi token
        public static void Revoke(string token)
        {
            Tokens.TryRemove(token, out _);
        }
    }
}

