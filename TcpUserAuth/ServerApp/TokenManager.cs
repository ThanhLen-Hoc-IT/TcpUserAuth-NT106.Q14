using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace ServerApp
{
    public static class TokenManager
    {
        // Lưu token kèm username và thời gian hết hạn
        private static readonly ConcurrentDictionary<string, Tuple<string, DateTime>> _tokens =
            new ConcurrentDictionary<string, Tuple<string, DateTime>>();

        private static readonly Timer _cleaner;

        static TokenManager()
        {
            _cleaner = new Timer(60_000); // chạy mỗi phút
            _cleaner.Elapsed += (s, e) => Cleanup();
            _cleaner.Start();
        }

        // Cấp phát token mới cho username
        public static string Issue(string username, TimeSpan? ttl = null)
        {
            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var expire = DateTime.UtcNow.Add(ttl ?? TimeSpan.FromHours(1));
            _tokens[token] = Tuple.Create(username, expire);
            return token;
        }

        // Kiểm tra token hợp lệ và còn hạn không
        public static bool Validate(string token, out string username)
        {
            username = "";
            if (token == null) return false;

            Tuple<string, DateTime> info;
            if (!_tokens.TryGetValue(token, out info))
                return false;

            // info.Item1 = username, info.Item2 = thời điểm hết hạn
            if (info.Item2 < DateTime.UtcNow)
            {
                _tokens.TryRemove(token, out info);
                return false;
            }

            username = info.Item1;
            return true;
        }

        // Thu hồi token (logout)
        public static void Revoke(string token)
        {
            if (token != null)
            {
                Tuple<string, DateTime> tmp;
                _tokens.TryRemove(token, out tmp);
            }
        }

        // Dọn dẹp token hết hạn
        private static void Cleanup()
        {
            var expired = _tokens
                .Where(kv => kv.Value.Item2 < DateTime.UtcNow)
                .Select(kv => kv.Key)
                .ToList();

            foreach (var t in expired)
            {
                Tuple<string, DateTime> tmp;
                _tokens.TryRemove(t, out tmp);
            }
        }
    }
}
