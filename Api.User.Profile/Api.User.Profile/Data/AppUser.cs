using System;

namespace Api.User.Profile.Data
{
    public class AppUser
    {
        public string Name { get; set; }
        public DateTime Bithday { get; set; }
        public AppLocation Location { get; set; }
        public int CSexOrientation { get; set; }
        public int Height { get; set; }
        public int CReligion { get; set; }
    }
}