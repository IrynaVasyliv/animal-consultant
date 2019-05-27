namespace AnimalConsultant.Services.Models
{
    public class Reaction
    {
        public long Id { get; set; }
        public bool Liked { get; set; }
        public double Rating { get; set; }
        
        public long? CommentId { get; set; }
        public Comment Comment { get; set; }

        public long? QuestionId { get; set; }
        public Question Question { get; set; }

        public long? UserId { get; set; }
        public User User { get; set; }
    }
}
