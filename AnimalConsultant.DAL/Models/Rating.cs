using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AnimalConsultant.DAL.Models
{
    public class Rating
    {
        public long Id { get; set; }
        public int Stars { get; set; }
        public string Comment { get; set; }
        
        public long? UserId { get; set; }
        [InverseProperty("OutcomingRatings")]
        public User User { get; set; }

        public long? RatedUserId { get; set; }
        [InverseProperty("IncomingRatings")]
        public User RatedUser { get; set; }
    }
}
