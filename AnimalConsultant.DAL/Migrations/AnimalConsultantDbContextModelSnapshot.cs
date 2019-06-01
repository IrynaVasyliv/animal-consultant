﻿// <auto-generated />
using System;
using AnimalConsultant.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AnimalConsultant.DAL.Migrations
{
    [DbContext(typeof(AnimalConsultantDbContext))]
    partial class AnimalConsultantDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AnimalConsultant.DAL.Models.AnimalType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.HasKey("Id");

                    b.ToTable("AnimalTypes");
                });

            modelBuilder.Entity("AnimalConsultant.DAL.Models.Article", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("AnimalTypeId");

                    b.Property<long?>("CategoryId");

                    b.Property<string>("Comment");

                    b.Property<DateTime>("CreateDate");

                    b.Property<int>("Dislikes");

                    b.Property<int>("Likes");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("AnimalTypeId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("AnimalConsultant.DAL.Models.Category", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("AnimalConsultant.DAL.Models.Comment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreateDate");

                    b.Property<int>("Dislikes");

                    b.Property<int>("Likes");

                    b.Property<long?>("QuestionId");

                    b.Property<double>("Rating");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("AnimalConsultant.DAL.Models.Identity.Role", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("AnimalConsultant.DAL.Models.Identity.RoleClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<long>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("AnimalConsultant.DAL.Models.Identity.UserClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("AnimalConsultant.DAL.Models.Identity.UserLogin", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<long>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("AnimalConsultant.DAL.Models.Identity.UserRole", b =>
                {
                    b.Property<long>("UserId");

                    b.Property<long>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("AnimalConsultant.DAL.Models.Identity.UserToken", b =>
                {
                    b.Property<long>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("AnimalConsultant.DAL.Models.Pet", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("AnimalTypeId");

                    b.Property<string>("Breed");

                    b.Property<DateTime?>("DateOfBirth");

                    b.Property<string>("Image");

                    b.Property<string>("Name");

                    b.Property<long>("OwnerId");

                    b.HasKey("Id");

                    b.HasIndex("AnimalTypeId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Pets");
                });

            modelBuilder.Entity("AnimalConsultant.DAL.Models.Question", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("AnimalTypeId");

                    b.Property<long?>("CategoryId");

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreateDate");

                    b.Property<int>("Dislikes");

                    b.Property<string>("Image");

                    b.Property<int>("Likes");

                    b.Property<double>("Rating");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("AnimalTypeId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("AnimalConsultant.DAL.Models.Rating", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comment");

                    b.Property<long?>("RatedUserId");

                    b.Property<int>("Stars");

                    b.Property<long?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("RatedUserId");

                    b.HasIndex("UserId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("AnimalConsultant.DAL.Models.Reaction", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("CommentId");

                    b.Property<bool>("Liked");

                    b.Property<long?>("QuestionId");

                    b.Property<double>("Rating");

                    b.Property<long?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CommentId");

                    b.HasIndex("QuestionId");

                    b.HasIndex("UserId");

                    b.ToTable("Reactions");
                });

            modelBuilder.Entity("AnimalConsultant.DAL.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AboutMe");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("Image");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<double>("Rating");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("AnimalConsultant.DAL.Models.Article", b =>
                {
                    b.HasOne("AnimalConsultant.DAL.Models.AnimalType", "AnimalType")
                        .WithMany("Articles")
                        .HasForeignKey("AnimalTypeId");

                    b.HasOne("AnimalConsultant.DAL.Models.Category", "Category")
                        .WithMany("Articles")
                        .HasForeignKey("CategoryId");

                    b.HasOne("AnimalConsultant.DAL.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AnimalConsultant.DAL.Models.Comment", b =>
                {
                    b.HasOne("AnimalConsultant.DAL.Models.Question", "Question")
                        .WithMany("Comments")
                        .HasForeignKey("QuestionId");

                    b.HasOne("AnimalConsultant.DAL.Models.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AnimalConsultant.DAL.Models.Identity.RoleClaim", b =>
                {
                    b.HasOne("AnimalConsultant.DAL.Models.Identity.Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AnimalConsultant.DAL.Models.Identity.UserClaim", b =>
                {
                    b.HasOne("AnimalConsultant.DAL.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AnimalConsultant.DAL.Models.Identity.UserLogin", b =>
                {
                    b.HasOne("AnimalConsultant.DAL.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AnimalConsultant.DAL.Models.Identity.UserRole", b =>
                {
                    b.HasOne("AnimalConsultant.DAL.Models.Identity.Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AnimalConsultant.DAL.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AnimalConsultant.DAL.Models.Identity.UserToken", b =>
                {
                    b.HasOne("AnimalConsultant.DAL.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AnimalConsultant.DAL.Models.Pet", b =>
                {
                    b.HasOne("AnimalConsultant.DAL.Models.AnimalType", "AnimalType")
                        .WithMany()
                        .HasForeignKey("AnimalTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AnimalConsultant.DAL.Models.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AnimalConsultant.DAL.Models.Question", b =>
                {
                    b.HasOne("AnimalConsultant.DAL.Models.AnimalType", "AnimalType")
                        .WithMany("Questions")
                        .HasForeignKey("AnimalTypeId");

                    b.HasOne("AnimalConsultant.DAL.Models.Category", "Category")
                        .WithMany("Questions")
                        .HasForeignKey("CategoryId");

                    b.HasOne("AnimalConsultant.DAL.Models.User", "User")
                        .WithMany("Questions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AnimalConsultant.DAL.Models.Rating", b =>
                {
                    b.HasOne("AnimalConsultant.DAL.Models.User", "RatedUser")
                        .WithMany("IncomingRatings")
                        .HasForeignKey("RatedUserId");

                    b.HasOne("AnimalConsultant.DAL.Models.User", "User")
                        .WithMany("OutcomingRatings")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("AnimalConsultant.DAL.Models.Reaction", b =>
                {
                    b.HasOne("AnimalConsultant.DAL.Models.Comment", "Comment")
                        .WithMany("Reactions")
                        .HasForeignKey("CommentId");

                    b.HasOne("AnimalConsultant.DAL.Models.Question", "Question")
                        .WithMany("Reactions")
                        .HasForeignKey("QuestionId");

                    b.HasOne("AnimalConsultant.DAL.Models.User", "User")
                        .WithMany("Reactions")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}