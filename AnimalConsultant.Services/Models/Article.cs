using System;

namespace AnimalConsultant.Services.Models
{
    public class Articles
    {
        public long Id { get; set; }
        public string Comment { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public DateTime CreateDate { get; set; }

        public long UserId { get; set; }
        public Users User { get; set; }

        public long? CategoryId { get; set; }
        public Categories Category { get; set; }

        public long? AnimalTypeId { get; set; }
        public AnimalTypes AnimalType { get; set; }
    }
}
