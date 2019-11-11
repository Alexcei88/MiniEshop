using Microsoft.EntityFrameworkCore;
using MiniEshop.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniEshop.DAL
{
    public class MiniEshopDbContext
        : DbContext
    {
        public MiniEshopDbContext(DbContextOptions<MiniEshopDbContext> option)
            : base(option)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Good>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<FileLink>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<FileLink>()
                .HasIndex(g => g.FileUrl);

            modelBuilder.Entity<Good>()
                .HasOne(a => a.FileLink).WithOne().OnDelete(DeleteBehavior.SetNull);
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Good> Goods { get; set; }

        public DbSet<FileLink> FileLinks { get; set; }

    }
}
