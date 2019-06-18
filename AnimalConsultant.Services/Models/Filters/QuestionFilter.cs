using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalConsultant.Services.Models.Filters
{
    public class QuestionFilter : Filter
    {
        public long?[] CategoryId { get; set; }
        public long?[] AnimalTypeId { get; set; }
        public long? UserId { get; set; }
    }
}
