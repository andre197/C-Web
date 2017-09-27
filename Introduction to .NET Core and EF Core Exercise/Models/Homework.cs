namespace Models
{
    using System;
    using Enums;

    public class Homework
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public TypeOfHomework TypeOfHomework { get; set; }

        public DateTime SubmissionDate { get; set; } = DateTime.Now;

        public int StudentId { get; set; }

        public int CourseId { get; set; }

        public Student Student { get; set; }

        public Course Course { get; set; }
    }
}
