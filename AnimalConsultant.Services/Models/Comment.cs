using System;
using System.Collections.Generic;

namespace AnimalConsultant.Services.Models
{
    public class Comments
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public DateTime CreateDate { get; set; }
        public double Rating { get; set; }

        public long? QuestionId { get; set; }
        public Questions Question { get; set; }

        public long UserId { get; set; }
        public Users User { get; set; }

        public ICollection<Reactions> Reactions { get; set; }
    }
}
