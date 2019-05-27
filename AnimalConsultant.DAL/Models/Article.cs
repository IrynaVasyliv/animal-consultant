using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalConsultant.DAL.Models
{
    public class Article
    {
        public long Id { get; set; }
        public string Comment { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public DateTime CreateDate { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }

        public long? CategoryId { get; set; }
        public Category Category { get; set; }

        public long? AnimalTypeId { get; set; }
        public AnimalType AnimalType { get; set; }
    }
}
