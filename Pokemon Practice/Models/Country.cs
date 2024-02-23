namespace CarManagment.Models
{
    public class Country
    {
        public int CountryId { get; set; }
        public string Name { get; set; }
        public ICollection<Owner>? Owners  { get; set;}
    }
}
