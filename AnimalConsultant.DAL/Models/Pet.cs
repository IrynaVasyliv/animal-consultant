using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalConsultant.DAL.Models
{
    public class Pet
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Breed { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public long OwnerId { get; set; }
        public User Owner { get; set; }

        public long AnimalTypeId { get; set; }
        public AnimalType AnimalType { get; set; }
    }
}
