using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace AnimalConsultant.DAL.Models.Identity
{
    public class Role : IdentityRole<long>
    {
        public Role(string name)
            : base(name)
        {
        }
    }
}
