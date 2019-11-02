using Microsoft.EntityFrameworkCore;
using MiniEshop.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniEshop.DAL
{
    public class EshopDbContext
        : DbContext
    {
        public EshopDbContext(DbContextOptions<EshopDbContext> option)
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
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Good> Goods { get; set; }

    }
}
