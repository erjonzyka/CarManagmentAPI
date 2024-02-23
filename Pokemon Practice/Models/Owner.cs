namespace CarManagment.Models
{
    public class Owner
    {
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public string Garage { get; set; }
        public int? CountryId { get; set; }
        public Country? Country { get; set; }
        public ICollection<Ownership>? Cars { get; set;}
    }
}
