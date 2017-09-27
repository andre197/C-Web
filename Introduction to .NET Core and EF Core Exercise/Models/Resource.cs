namespace Models
{
    using System.Collections.Generic;
    using Enums;
    using Student_System.Models;

    public class Resource
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public TypeOfCourse TypeOfCourse { get; set; }

        public string URL { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public ICollection<License> Licenses { get; set; } = new HashSet<License>();
    }
}
