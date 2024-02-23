using CarManagment.Models;


namespace CarManagment
{
    public class Seed
    {
        private readonly MyContext dataContext;
        public Seed(MyContext context)
        {
            this.dataContext = context;
        }
        public void SeedDataContext()
        {
            if (!dataContext.Ownerships.Any())
            {
                var carOwners = new List<Ownership>()
                {
                    new Ownership()
                    {
                        Car = new Car()
                        {
                            Make = "Mercedes Benz",
                            ProductionYear = new DateTime(2018,1,1),
                            CarCategories = new List<CategoryJoin>()
                            {
                                new CategoryJoin { Category = new Category() { Name = "Electric"}}
                            },
                            Reviews = new List<Review>()
                            {
                                new Review { Title="Mercedes",Text = "Mercedes is the best car, because it is electric", Rating = 5,
                                Reviewer = new Reviewer(){ FirstName = "Teddy", LastName = "Smith" } },
                                new Review { Title="Mercedes", Text = "Mercedes is the best at off roading", Rating = 5,
                                Reviewer = new Reviewer(){ FirstName = "Taylor", LastName = "Jones" } },
                                new Review { Title="Mercedes",Text = "Low quality interior!", Rating = 1,
                                Reviewer = new Reviewer(){ FirstName = "Jessica", LastName = "McGregor" } },
                            }
                        },
                        Owner = new Owner()
                        {
                            Name = "Jack",
                            Garage = "Electric Cars Albania",
                            Country = new Country()
                            {
                                Name = "Albania"
                            }
                        }
                    },
                };
                dataContext.Ownerships.AddRange(carOwners); 
                dataContext.SaveChanges();
            }
        }
    }
}