namespace Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Student_System.Models;

    public class StudentSystemContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Homework> Homeworks { get; set; }
        public DbSet<License> Licenses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(@"Server=LAPTOP-22QSF26P\SQLEXPRESS;Database=TestDb;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            DatabaseBuilder db = new DatabaseBuilder(builder);

            db.AddStudentCourseKey();
            db.ManyToManyRelationStudentCourse();
            db.OneToManyRelationCourseWithResoursec();
            db.HomeworkRelations();
            db.ResourceWithLicensesRelation();
        }

    }
}
