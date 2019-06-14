using System.Collections.Generic;

namespace AnimalConsultant.Services.Models
{
    public class Categories
    {
        public long Id { get; set; }
        public string Description { get; set; }

        public ICollection<Questions> Questions { get; set; }
        public ICollection<Articles> Articles { get; set; }
    }
}
