using System;
using System.Collections.Concurrent;
using SharedModels;

namespace ServerApp
{
    internal class TokenManager
    {
        private static ConcurrentDictionary<string, TokenInfo> Tokens
            = new ConcurrentDictionary<string, TokenInfo>();

        public static string CreateToken(int userId, int ttlMinutes = 60)
        {
            string token = Guid.NewGuid().ToString("N"); // random 32 ký tự
            Tokens[token] = new TokenInfo
            {
                Token = token,
                UserId = userId,
                ExpiredAt = DateTime.Now.AddMinutes(ttlMinutes)
            };
            return token;
        }

        public static bool ValidateToken(string token)
        {
            if (Tokens.TryGetValue(token, out TokenInfo info))
            {
                if (info.ExpiredAt > DateTime.Now)
                    return true;
                else
                    Tokens.TryRemove(token, out _);
            }
            return false;
        }

        public static void RevokeToken(string token)
        {
            Tokens.TryRemove(token, out _);
        }

        public static void CleanupExpiredTokens()
        {
            foreach (var kv in Tokens)
            {
                if (kv.Value.ExpiredAt <= DateTime.Now)
                    Tokens.TryRemove(kv.Key, out _);
            }
        }
        public static string GenerateToken(int userId, int ttlMinutes = 60)
        {
            return CreateToken(userId, ttlMinutes);
        }

    }
}
