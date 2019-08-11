namespace Api.Auth.Models
{
    public class AuthConfig
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public int JwtExpireMinutes { get; set; }
        public string Audience { get; set; }
    }
}