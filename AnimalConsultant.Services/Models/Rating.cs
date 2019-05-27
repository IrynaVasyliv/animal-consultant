namespace AnimalConsultant.Services.Models
{
    public class Rating
    {
        public long Id { get; set; }
        public int Stars { get; set; }
        public string Comment { get; set; }
        
        public long? UserId { get; set; }
        public User User { get; set; }

        public long? RatedUserId { get; set; }
        public User RatedUser { get; set; }
    }
}
