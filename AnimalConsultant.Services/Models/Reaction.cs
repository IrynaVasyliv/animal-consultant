namespace AnimalConsultant.Services.Models
{
    public class Reactions
    {
        public long Id { get; set; }
        public bool Liked { get; set; }
        public double Rating { get; set; }
        
        public long? CommentId { get; set; }
        public Comments Comment { get; set; }

        public long? QuestionId { get; set; }
        public Questions Question { get; set; }

        public long? UserId { get; set; }
        public Users User { get; set; }
    }
}
