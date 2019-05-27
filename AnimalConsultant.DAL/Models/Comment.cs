using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalConsultant.DAL.Models
{
    public class Comment
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public DateTime CreateDate { get; set; }
        public double Rating { get; set; }

        public long? QuestionId { get; set; }
        public Question Question { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }

        public ICollection<Reaction> Reactions { get; set; }
    }
}
