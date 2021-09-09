using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Disc_Store.Entities;

namespace Disc_Store.Data
{
    public class ApplicationDbContext : IdentityDbContext<MyUser>
    {
        public DbSet<Band> bands { get; set; }
        
        public DbSet<Disc> discs{ get; set; }

        public DbSet<Label> labels{ get; set; }

        public DbSet<Musician> musicians { get; set; }

        public DbSet<Role> roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Musician>( )
                .HasMany<Band>(m=>m.bands).WithMany(b => b.musicians)
                .UsingEntity(j=>j.ToTable("PivotMusiciansBand"));
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
