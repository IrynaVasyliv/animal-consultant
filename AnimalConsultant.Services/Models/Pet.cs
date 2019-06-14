using System;

namespace AnimalConsultant.Services.Models
{
    public class Pets
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Breed { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public long OwnerId { get; set; }
        public Users Owner { get; set; }

        public long AnimalTypeId { get; set; }
        public AnimalTypes AnimalType { get; set; }
    }
}
