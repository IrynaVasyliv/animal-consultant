using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemOffice.GenericCrud.Models;
using D = AnimalConsultant.DAL.Models;
using S = AnimalConsultant.Services.Models;

namespace AnimalConsultant.Generic
{
    public static class GenericEntitiesSetup
    {
        public static readonly IReadOnlyCollection<GenericSetup> Mappings = new List<GenericSetup>
        {
            new GenericSetup<S.User, D.User>(
                mappingOverride:x=>x
                .ReverseMap()
                .ForMember(dest=>dest.Role, opt=>opt.Ignore())
                .ForMember(dest=>dest.Password, opt=>opt.Ignore())),
            new GenericSetup<S.AnimalType, D.AnimalType>(),
            new GenericSetup<S.Article, D.Article>(),
            new GenericSetup<S.Category, D.Category>(),
            new GenericSetup<S.Comment, D.Comment>(),
            new GenericSetup<S.Pet, D.Pet>(),
            new GenericSetup<S.Question, D.Question>(),
            new GenericSetup<S.Rating, D.Rating>(),
            new GenericSetup<S.Reaction, D.Reaction>()
        };
    }
}
