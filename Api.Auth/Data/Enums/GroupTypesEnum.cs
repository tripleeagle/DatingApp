using System.ComponentModel;

namespace Api.Auth.Data.Enums
{
    public enum GroupTypesEnum
    { 
        [Description("User")] User = 10,
        [Description("Admin")] Admin = 20,
        [Description("SuperUser")] SuperUser = 30
    }
}