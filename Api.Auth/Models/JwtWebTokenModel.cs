using System;

namespace Api.Auth.Data
{
    public class JwtWebTokenModel
    {
        public string AccessToken { get; set; }
        public JwtRefreshToken JwtRefreshToken { get; set; }
    }
}