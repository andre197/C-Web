namespace TheRestExercisesIncludedInTheLab
{
    using System.Collections.Generic;

    public class Item
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public ICollection<ItemOrder> Orders { get; set; } = new HashSet<ItemOrder>();

        public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
    }
}
