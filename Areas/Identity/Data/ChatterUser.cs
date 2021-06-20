using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Chatter.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the ChatterUser class
    public class ChatterUser : IdentityUser
    {
        [PersonalData]
        public string ChatterName { get; set; }
        [PersonalData]
        public string Avatar { get; set; }

        [PersonalData]
        public string AboutMe { get; set; }

        [PersonalData]
        public DateTime DayOfBirth  { get; set; }

    }
}
