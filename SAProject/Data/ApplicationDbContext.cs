using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SAProject.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<File> Files { get; set; }
        public override DbSet<ApplicationUser> Users { get; set; }
        public DbSet<UserFile> UserFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserFile>()
                    .HasOne(t => t.User)
                    .WithMany(t => t.UserFiles)
                    .HasForeignKey(t => t.UserId);

            modelBuilder.Entity<UserFile>()
                    .HasOne(t => t.File)
                    .WithMany(t => t.UserFiles)
                    .HasForeignKey(t => t.FileId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
