namespace CarManagment.Models
{
    public class Ownership
    {
        public int CarId { get; set; }
        public int OwnerId { get; set; }
        public Car? Car { get; set; }
        public Owner? Owner { get; set; }
    }
}
