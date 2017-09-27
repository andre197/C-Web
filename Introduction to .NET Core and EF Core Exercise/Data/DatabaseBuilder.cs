namespace Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Student_System.Models;

    public class DatabaseBuilder
    {
        private ModelBuilder builder;

        public DatabaseBuilder(ModelBuilder builder)
        {
            this.builder = builder;
        }

        public void AddStudentCourseKey()
        {
            this.builder
                .Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });
        }

        public void ManyToManyRelationStudentCourse()
        {
            this.builder
                .Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.Courses)
                .HasForeignKey(sc => sc.StudentId);

            this.builder
                .Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.Students)
                .HasForeignKey(sc => sc.CourseId);
        }

        public void OneToManyRelationCourseWithResoursec()
        {
            this.builder
                .Entity<Resource>()
                .HasOne(r => r.Course)
                .WithMany(c => c.Resources)
                .HasForeignKey(r => r.CourseId);
        }

        public void HomeworkRelations()
        {
            this.builder
                .Entity<Homework>()
                .HasOne(h => h.Student)
                .WithMany(s => s.Homeworks)
                .HasForeignKey(h => h.StudentId);

            this.builder
                .Entity<Homework>()
                .HasOne(h => h.Course)
                .WithMany(c => c.Homeworks)
                .HasForeignKey(h => h.CourseId);
        }

        public void ResourceWithLicensesRelation()
        {
            this.builder.Entity<License>()
                .HasOne(l => l.Resource)
                .WithMany(r => r.Licenses)
                .HasForeignKey(l => l.ResourceId);
        }
    }
}
