namespace Models
{
    using System;
    using System.Collections.Generic;

    public class Course
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal Price { get; set; }

        public ICollection<StudentCourse> Students { get; set; } = new HashSet<StudentCourse>();

        public ICollection<Resource> Resources { get; set; } = new HashSet<Resource>();

        public ICollection<Homework> Homeworks { get; set; } = new HashSet<Homework>();
    }
}
