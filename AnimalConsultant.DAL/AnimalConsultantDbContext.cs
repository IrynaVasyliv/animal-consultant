using System;
using System.Collections.Generic;
using System.Text;
using AnimalConsultant.DAL.Models;
using AnimalConsultant.DAL.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AnimalConsultant.DAL
{
    public class AnimalConsultantDbContext : IdentityDbContext<User, Role, long, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        private readonly string connectionString;

        public AnimalConsultantDbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public DbSet<AnimalType> AnimalTypes { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        
        public AnimalConsultantDbContext(DbContextOptions<AnimalConsultantDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(this.connectionString);
            }
        }
    }
}
