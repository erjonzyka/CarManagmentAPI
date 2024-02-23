namespace CarManagment.Models
{
    public class Car
    {
        public int CarId { get; set; }
        public string Make { get; set; }
        public DateTime ProductionYear { get; set; }

        public ICollection<Review>? Reviews { get; set;}
        public ICollection<Ownership>? CarOwners { get; set; }
        public ICollection<CategoryJoin>? CarCategories { get; set; }
    }
}
