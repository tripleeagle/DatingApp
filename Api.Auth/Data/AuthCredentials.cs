using System.ComponentModel.DataAnnotations.Schema;
using Api.Auth.Data.Enums;

namespace Api.Auth.Data
{
    public class AuthCredentials
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int RegistrationStateId { get; set; }
    
        [NotMapped]
        public RegistrationStateEnum RegistrationStateEnum
        {
            get => (RegistrationStateEnum) RegistrationStateId;
            set => RegistrationStateId = (int) value;
        }
    }
}