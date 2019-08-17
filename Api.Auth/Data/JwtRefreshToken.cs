using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Auth.Data
{
    [Table("Jwt_Refresh_Token")]
    public class JwtRefreshToken
    {   
        [Column("Email")] 
        public string Email { get; set; }
        
        [Column("Key")] 
        public string Key { get; set; }
        
        [Column("Valid_To")] 
        public DateTime ValidTo { get; set; }
    }
}