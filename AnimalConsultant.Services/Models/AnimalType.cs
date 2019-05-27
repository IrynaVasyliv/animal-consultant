using System.Collections.Generic;

namespace AnimalConsultant.Services.Models
{
    public class AnimalType 
    {
        public long Id { get; set; }
        public string Description { get; set; }

        public ICollection<Question> Questions { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}
