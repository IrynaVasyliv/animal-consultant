namespace AnimalConsultant.Services.Models
{
    public class Ratings
    {
        public long Id { get; set; }
        public int Stars { get; set; }
        public string Comment { get; set; }
        
        public long? UserId { get; set; }
        public Users User { get; set; }

        public long? RatedUserId { get; set; }
        public Users RatedUser { get; set; }
    }
}
