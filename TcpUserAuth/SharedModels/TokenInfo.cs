using System;

namespace SharedModels
{
    public class TokenInfo
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}
