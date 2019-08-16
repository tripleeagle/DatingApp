using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Auth.Data
{
    [Table("Jwt_Refresh_Token")]
    public class JwtRefreshToken
    {   
        [Column("Email")] public string Email { get; set; }
        [Column("Jwt_Token")] public string JwtToken { get; set; }
        [Column("Valid_To")] public DateTime ValidTo { get; set; }
    }
}