using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalConsultant.DAL.Models
{
    public class Category
    {
        public long Id { get; set; }
        public string Description { get; set; }

        public ICollection<Question> Questions { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}
