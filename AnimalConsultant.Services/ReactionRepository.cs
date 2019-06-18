using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimalConsultant.DAL.Models;
using AnimalConsultant.Services.Models;
using AutoMapper;
using DemOffice.GenericCrud.DataAccess;
using DemOffice.GenericCrud.Repositories;
using DemOffice.GenericCrud.Services;
using Microsoft.EntityFrameworkCore;

namespace AnimalConsultant.Services
{
    public class ReactionRepostory : GenericRepository<Reaction>
    {
        public ReactionRepostory(IDbContextProvider dbContextProvider, IAuthContext context) : base(dbContextProvider, context)
        {
        }

        public override async Task<Reaction> Create(Reaction reaction)
        {
            var db = DbContextProvider.Create();
 
            db.Add(reaction);
            await db.SaveChangesAsync();
            return reaction;
        }

        public override async Task<Reaction> Update(Reaction reaction)
        {
            var db = DbContextProvider.Create();
            db.Update(reaction);
            await db.SaveChangesAsync();
            return reaction;
        }
    } 
}
