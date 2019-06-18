﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AnimalConsultant.Services;
using AnimalConsultant.Services.Models.Filters;
using DemOffice.GenericCrud.Models;
using Microsoft.EntityFrameworkCore;
using D = AnimalConsultant.DAL.Models;
using S = AnimalConsultant.Services.Models;
using F = AnimalConsultant.Services.Models.Filters;

namespace AnimalConsultant.Generic
{
    public static class GenericEntitiesSetup
    {
        public static readonly IReadOnlyCollection<GenericSetup> Mappings = new List<GenericSetup>
        {
            
            new GenericSetup<S.AnimalTypes, D.AnimalType>(),

            new GenericSetup<S.Articles, D.Article>(),

            new GenericSetup<S.Categories, D.Category>(),

            new GenericSetup<S.Comments, D.Comment>(
                includes: new List<string>
                {
                    nameof(D.Comment.User),
                    nameof(D.Comment.Reactions)
                }),

            new GenericSetup<S.Pets, D.Pet>(),

            new FilteredGenericSetup<S.Questions, D.Question, F.QuestionFilter>(
                (filter, q, context) =>
            {
                var filters = new List<Expression<Func<D.Question, bool>>>();

                if (filter.AnimalTypeId != null && filter.AnimalTypeId.Any())
                {
                    filters.Add(x=>filter.AnimalTypeId.Contains(x.AnimalTypeId));
                }

                if (filter.CategoryId != null && filter.CategoryId.Any())
                {
                    filters.Add(x=>filter.CategoryId.Contains(x.CategoryId));
                }

                if (filter.UserId != null)
                {
                    filters.Add(x=>x.UserId == filter.UserId);
                }

                if (!string.IsNullOrEmpty(q))
                {
                    filters.Add(x=>x.Content.ToLower().Contains(q.ToLower()));
                }

                return filters;
            },
                mappingOverride: x=>x
                .ForMember(s=>s.Image, opt=>opt.MapFrom(d=>string.Join(";", d.Image)))
                .ReverseMap()
                .ForMember(s=>s.Image, opt => opt.MapFrom(d => d.Image.Split(';', StringSplitOptions.None).ToList())),
                getManyIncludes: new List<string>()
                {
                    nameof(D.Question.User),
                    nameof(D.Question.AnimalType),
                    nameof(D.Question.Category),
                    nameof(D.Question.Reactions),
                    nameof(D.Question.Comments) + "." + nameof(D.Question.User),

                },
                includes: new List<string>()
                {
                    nameof(D.Question.User),
                    nameof(D.Question.AnimalType),
                    nameof(D.Question.Category),
                    nameof(D.Question.Reactions),
                    nameof(D.Question.Comments) + "." + nameof(D.Question.User),
                    nameof(D.Question.Comments) + "." + nameof(D.Question.Reactions),

                }),

            new GenericSetup<S.Ratings, D.Rating>(
                ),

            new GenericSetup<S.Reactions, D.Reaction>(specificRepository: typeof(ReactionRepostory)),

            new GenericSetup<S.Users, D.User>(
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
