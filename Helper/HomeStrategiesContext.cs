using HomeStrategiesApi.Models;
using HomeStrategiesApi.Models.MongoDB;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;

namespace HomeStrategiesApi.Helper
{
    public class HomeStrategiesContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Household> Households { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillImage> BillImages { get; set; }
        public DbSet<BillCategory> BillCategories { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<RecipeFavourite> RecipeFavourites { get; set; }

        public HomeStrategiesContext(DbContextOptions<HomeStrategiesContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Household>()
               .HasOne(h => h.HouseholdCreator)
               .WithOne(u => u.AdminOfHousehold)
               .HasForeignKey<Household>(h => h.AdminId);
                
            //modelBuilder.Entity<Household>()
            //    .HasMany(h => h.householdeMember)
            //    .WithOne(u => u.household);

            //modelBuilder.Entity<User>()
            //    .HasOne(u => u.household)
            //    .WithMany(h => h.householdeMember)
            //    .HasForeignKey(u => u.householdForgeinKey);

            //modelBuilder.Entity<Bill>()
            //    .HasMany(h => h.householdeMember)
            //    .WithOne(u => u.household);
        }
    }
}
