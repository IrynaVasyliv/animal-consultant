using System;
using System.Collections.Generic;
using System.Text;
using DemOffice.GenericCrud.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace AnimalConsultant.DAL
{
    public class AnimalConsultantDbProvider : IDbContextProvider
    {
        private readonly List<AnimalConsultantDbContext> contextList = new List<AnimalConsultantDbContext>();

        public static string ConnectionString { get; set; }

        public DbContext Create()
        {
            var context = new AnimalConsultantDbContext(ConnectionString);
            contextList.Add(context);
            return context;
        }

        public void Dispose()
        {
            contextList.ForEach(c => c.Dispose());
        }
    }
}
