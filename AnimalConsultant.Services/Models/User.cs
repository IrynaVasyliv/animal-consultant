using System.Collections.Generic;

namespace AnimalConsultant.Services.Models
{
    public class User
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

        public ICollection<Question> Questions { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Rating> IncomingRatings { get; set; }
        public ICollection<Rating> OutcomingRatings { get; set; }
        public ICollection<Reaction> Reactions { get; set; }
    }
}
