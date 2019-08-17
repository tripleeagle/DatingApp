namespace Api.Auth.Models
{
    public class JwtOptions
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public int JwtExpireMinutes { get; set; }
        public string Audience { get; set; }
        public int RefreshTokenExpireMonth { get; set; }
    }
}