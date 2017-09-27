namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Student
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string PhoneNumer { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        public DateTime? BirthDate { get; set; }

        public ICollection<StudentCourse> Courses { get; set; } = new HashSet<StudentCourse>();

        public ICollection<Homework> Homeworks { get; set; } = new HashSet<Homework>();
    }
}
