namespace CarManagment.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public int? ReviewerId { get; set; }
        public Reviewer? Reviewer { get; set; }
        public Car? Car { get; set; }
    }
}
