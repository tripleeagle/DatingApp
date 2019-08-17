using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Api.Auth.Data.Enums;

namespace Api.Auth.Data
{
    [Table("Auth_Credentials")]
    public class AuthCredentials
    {
        public AuthCredentials(string email, string password)
        {
            Email = email;
            Password = password;
            RegistrationStateEnum = RegistrationStateEnum.Unconfirmed;
        }
        
        [Column("Email")]
        public string Email { get; set; }
        
        [Column("Password")] 
        public string Password { get; set; }
        
        [Column("Relation_State_Id")] 
        public int RegistrationStateId { get; set; }

        [NotMapped]
        public RegistrationStateEnum RegistrationStateEnum
        {
            get => (RegistrationStateEnum) RegistrationStateId;
            set => RegistrationStateId = (int) value;
        }
    }
}