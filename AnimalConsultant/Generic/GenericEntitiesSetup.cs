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
            
            new GenericSetup<S.AnimalType, D.AnimalType>(),
            new GenericSetup<S.Article, D.Article>(),
            new GenericSetup<S.Category, D.Category>(),
            new GenericSetup<S.Comment, D.Comment>(),
            new GenericSetup<S.Pet, D.Pet>(),
            new GenericSetup<S.Question, D.Question>(),
            new GenericSetup<S.Rating, D.Rating>(),
            new GenericSetup<S.Reaction, D.Reaction>(),
            new GenericSetup<S.User, D.User>(
                validate: (entity, type, db) =>
                {
                    var result = new List<string>();
                    var users = db.Set<D.User>();

                    if (type == OperationType.Create)
                    {
                        if (users.Any(x => x.Email == entity.Email))
                        {
                            result.Add("User with wthis email already exists.");
                        }
                    }

                    return result;
                },
                includes: new List<string>
                {
                    nameof(D.User.IncomingRatings),
                    nameof(D.User.Questions),
                    nameof(D.User.Pets)
                },
                getManyIncludes: new List<string>
                {
                    nameof(D.User.IncomingRatings)
                },
                mappingOverride:x=>x
                    .ReverseMap()
                    .ForMember(dest=>dest.Role, opt=>opt.Ignore())
                    .ForMember(dest=>dest.Password, opt=>opt.Ignore())),
        };
    }
}
