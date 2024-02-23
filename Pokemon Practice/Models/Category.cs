namespace CarManagment.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public ICollection<CategoryJoin>? Categories { get; set;}
    }
}
