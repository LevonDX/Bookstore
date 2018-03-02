using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {

        }

        public ApplicationUser(string userID) : base(userID)
        {

        }
    }
}
