using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace AnimalConsultant.Services.Models
{
    public class Questions
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public double Rating { get; set; }
        public List<string> Image { get; set; }
        public DateTime CreateDate { get; set; }

        public long? CategoryId { get; set; }
        public Categories Category { get; set; }

        public long? AnimalTypeId { get; set; }
        public AnimalTypes AnimalType { get; set; }

        public long? PetId { get; set; }
        public Pets Pet { get; set; } 

        public long UserId { get; set; }
        public Users User { get; set; }

        public ICollection<Comments> Comments { get; set; }
        public ICollection<Reactions> Reactions { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
