using System.Collections.Generic;

namespace AnimalConsultant.Services.Models
{
    public class Users
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Rating { get; set; }
        public string Image { get; set; }
        public string AboutMe { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public long Id { get; set; }
        public string Role { get; set; }

        public ICollection<Questions> Questions { get; set; }
        public ICollection<Comments> Comments { get; set; }
        public ICollection<Ratings> IncomingRatings { get; set; }
        public ICollection<Ratings> OutcomingRatings { get; set; }
        public ICollection<Reactions> Reactions { get; set; }
        public ICollection<Pets> Pets { get; set; }
        public ICollection<Articles> Articles { get; set; }
    }
}
