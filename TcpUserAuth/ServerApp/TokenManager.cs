using System;
using System.Collections.Concurrent;
using SharedModels;

namespace ServerApp
{
    /// <summary>
    /// Quản lý token đăng nhập (issue, validate, revoke).
    /// Dạng đơn giản, lưu trong bộ nhớ (chưa dùng DB hay Redis).
    /// </summary>
    public static class TokenManager
    {
        // Key: token string, Value: thông tin user + thời hạn
        private static readonly ConcurrentDictionary<string, TokenInfo> _tokens =
    new ConcurrentDictionary<string, TokenInfo>();


        // Thời gian sống của token (ví dụ: 30 phút)
        private static readonly TimeSpan TokenLifetime = TimeSpan.FromMinutes(30);

        /// <summary>
        /// Tạo token mới cho user.
        /// </summary>
        public static string Issue(string username)
        {
            // Tạo token ngẫu nhiên dạng GUID
            var token = Guid.NewGuid().ToString("N");

            // Lưu token vào bộ nhớ
            var info = new TokenInfo
            {
                Username = username,
                Expiration = DateTime.UtcNow.Add(TokenLifetime)
            };

            _tokens[token] = info;

            Logger.Info($"✅ Token issued for user {username}");
            return token;
        }

        /// <summary>
        /// Kiểm tra token có hợp lệ hay không.
        /// </summary>
        public static bool Validate(string token, out string username)
        {
            username = null;
            if (string.IsNullOrWhiteSpace(token))
                return false;

            if (_tokens.TryGetValue(token, out var info))
            {
                // Hết hạn?
                if (DateTime.UtcNow > info.Expiration)
                {
                    _tokens.TryRemove(token, out _);
                    Logger.Info($"⚠️ Token expired for user {info.Username}");
                    return false;
                }

                username = info.Username;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Thu hồi (revoke) token khi user logout.
        /// </summary>
        public static void Revoke(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return;

            if (_tokens.TryRemove(token, out var info))
            {
                Logger.Info($"🚪 Token revoked for user {info.Username}");
            }
        }

        /// <summary>
        /// Dọn dẹp token hết hạn (tùy chọn, có thể chạy định kỳ).
        /// </summary>
        public static void CleanupExpired()
        {
            foreach (var kv in _tokens)
            {
                if (DateTime.UtcNow > kv.Value.Expiration)
                    _tokens.TryRemove(kv.Key, out _);
            }
        }

        // Struct chứa thông tin token
        private class TokenInfo
        {
            public string Username { get; set; }
            public DateTime Expiration { get; set; }
        }
    }
}
