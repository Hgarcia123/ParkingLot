using System;
using System.Collections.Generic;
using System.Text;

using Domain;
using Application.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure
{
    public class ParkingContext : IdentityDbContext<IdentityUser>, IParkingContext
    {
        public DbSet<Park> Parks { get; private set; }
        public DbSet<ParkSpots> ParkSpots { get; private set; }
        public DbSet<Spot> Spots { get; private set; }
        public DbSet<Entry> Entries { get; private set; }


        public string DbPath { get; private set; }


        public ParkingContext(DbContextOptions<ParkingContext> options) : base(options)
        {
            //var folder = Environment.SpecialFolder.LocalApplicationData;
            //var path = Environment.GetFolderPath(folder);
            //DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}parking.db";
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }

        public void SeedSuperUser(ModelBuilder modelBuilder)
        {
            //SEED SUPERUSER
            string USERID = "17029c13-4ccd-482e-9301-a01885987481";
            string ALLACCESSROLEID = "b0677b29-afc5-4e40-bae3-4f839de95421";
            string GUESTROLEID = "d9af4456-234f-4326-82d8-d0f98156caec";


            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = ALLACCESSROLEID,
                Name = "AllAccess",
                NormalizedName = "ALLACCESS"
            });

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = GUESTROLEID,
                Name = "Guest",
                NormalizedName = "GUEST"
            });

            var superUser = new IdentityUser
            {
                Id = USERID,
                UserName = "SuperUser",
                NormalizedUserName = "SUPERUSER",
            };

            PasswordHasher<IdentityUser> pass = new PasswordHasher<IdentityUser>();
            superUser.PasswordHash = pass.HashPassword(superUser, "Manager_2019");

            modelBuilder.Entity<IdentityUser>().HasData(superUser);

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ALLACCESSROLEID,
                UserId = USERID
            });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            SeedSuperUser(modelBuilder);
        }

        

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //=> options.UseSqlServer(@"Server=HGARCIAPC;Database=myParkingDB;Trusted_Connection=True;");


    }
}