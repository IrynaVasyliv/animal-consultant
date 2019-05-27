using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace AnimalConsultant.DAL.Models
{
    public class User : IdentityUser<long>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Rating { get; set; }
        public string Image { get; set; }
        public string AboutMe { get; set; }

        public ICollection<Question> Questions { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Rating> IncomingRatings { get; set; }
        public ICollection<Rating> OutcomingRatings { get; set; }
        public ICollection<Reaction> Reactions { get; set; }
    }
}
