using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace CarManagment.Models
{
    public class MyContext : DbContext
    {
        // This line will always be here. It is what constructs our context upon initialization  
        public MyContext(DbContextOptions options) : base(options) { }
        // We need to create a new DbSet<Model> for every model in our project that is making a table
        // The name of our table in our database will be based on the name we provide here
        // This is where we provide a plural version of our model to fit table naming standards
        public DbSet<Car> Cars { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reviewer> Reviewers { get; set; }
        public DbSet<Ownership> Ownerships { get; set; }
        public DbSet<CategoryJoin> CategoryJoins { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Category> Categories { get; set; }

        //per many to many
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryJoin>()
                .HasKey(e => new { e.CategoryId, e.CarId });

            modelBuilder.Entity<CategoryJoin>()
                .HasOne(e => e.Car)
                .WithMany(p => p.CarCategories)
                .HasForeignKey(e => e.CarId);
            modelBuilder.Entity<CategoryJoin>()
                .HasOne(e => e.Category)
                .WithMany(p => p.Categories)
                .HasForeignKey(e => e.CategoryId);



            modelBuilder.Entity<Ownership>()
                .HasKey(e => new { e.OwnerId, e.CarId });

            modelBuilder.Entity<Ownership>()
                .HasOne(e => e.Car)
                .WithMany(p => p.CarOwners)
                .HasForeignKey(e => e.CarId);
            modelBuilder.Entity<Ownership>()
                .HasOne(e => e.Owner)
                .WithMany(p => p.Cars)
                .HasForeignKey(e => e.OwnerId);
        }
    }
}
