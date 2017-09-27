namespace Student_System.Models
{
    using System.ComponentModel.DataAnnotations;
    using global::Models;

    public class License
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int ResourceId { get; set; }

        public Resource Resource { get; set; }
    }
}
