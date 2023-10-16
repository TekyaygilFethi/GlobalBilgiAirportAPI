using AirportDistanceCalculator.Data.POCO;
using GlobalBilgiQuiz.Helper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportDistanceCalculator.Database.DbContexts
{
    public partial class AirportDistanceCalculatorDbContext
    {
        public DbSet<User> Users { get; set; }

        private void ConfigureUserEntities(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(_ => _.Username)
                .IsUnique();

            Seed(builder);
        }

        private void Seed(ModelBuilder builder)
        {
            User u1 = new User
            {
                Id = Guid.NewGuid(),
                Username = "fethitekyaygil",
                Password = CryptographyHelper.Encode("1234", "globalbilgi")
            };

            builder.Entity<User>()
                .HasData(u1);
        }
    }
}
