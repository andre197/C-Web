namespace Student_System
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using global::Models;
    using global::Models.Enums;
    using Models;

    public class StartUp
    {
        private const int totalStudents = 25;
        private const int totalCourses = 10;
        private static Random random = new Random();

        public static void Main(string[] args)
        {

            using (StudentSystemContext db = new StudentSystemContext())
            {
                //db.Database.Migrate();

                //SeedData(db);

                //Print(ListStudents(db));
                //Print(ListCoursesAndTheirResources(db));
                //Print(CoursesWithMoreThan5Resources(db));
                //Print(CoursesBeingActiveOnGivenDate(db));
                //Print(StudentDetails(db));
                //Print(CoursesWithResourcesAndLicenses(db));
                //Print(StudentsFullDetails(db));
            }
        }

        private static string ListStudents(StudentSystemContext db)
        {
            StringBuilder sb = new StringBuilder();

            var studentData = db.Students
                .Select(s => new
                {
                    s.Name,
                    HomeworkNamesAndTypes = s.Homeworks.Select(h => new
                    {
                        h.Content,
                        contentType = h.TypeOfHomework.ToString()
                    })
                        .ToList()
                })
                .ToList();

            foreach (var student in studentData)
            {
                sb.AppendLine($"{student.Name}");
                foreach (var homework in student.HomeworkNamesAndTypes)
                {
                    sb.AppendLine($"{homework.Content} -> {homework.contentType}");
                }
            }

            return sb.ToString();
        }

        private static string ListCoursesAndTheirResources(StudentSystemContext db)
        {
            StringBuilder sb = new StringBuilder();

            var coursesData = db.Courses
                .OrderBy(c => c.StartDate)
                .ThenByDescending(c => c.EndDate)
                .Select(c => new
                {
                    c.Name,
                    c.Description,
                    c.Resources
                })
                .ToList();

            foreach (var course in coursesData)
            {
                sb.AppendLine($"{course.Name}:")
                    .AppendLine($"{course.Description}");

                foreach (var resource in course.Resources)
                {
                    sb.AppendLine($"Resource: {resource.Id}:")
                        .AppendLine($"Name: {resource.Name}")
                        .AppendLine($"URL: {resource.URL}");
                }
            }

            return sb.ToString();
        }

        private static string CoursesWithMoreThan5Resources(StudentSystemContext db)
        {
            StringBuilder sb = new StringBuilder();

            var coursesData = db.Courses
                .Where(c => c.Resources.Count > 5)
                .OrderByDescending(c => c.Resources.Count)
                .ThenByDescending(c => c.StartDate)
                .Select(c => new
                {
                    c.Name,
                    ResourcesCount = c.Resources.Count
                });

            foreach (var course in coursesData)
            {
                sb.AppendLine($"{course.Name} has {course.ResourcesCount} resources");
            }

            return sb.ToString();
        }

        private static string CoursesBeingActiveOnGivenDate(StudentSystemContext db)
        {
            StringBuilder sb = new StringBuilder();

            DateTime date = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.CurrentCulture);

            var courses = db.Courses
                .Where(c => c.StartDate <= date && c.EndDate >= date)
                .Select(c => new
                {
                    c.Name,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    CourseDuration = c.EndDate.Subtract(c.StartDate),
                    StudentsEnrolled = c.Students.Count
                })
                .OrderByDescending(c => c.StudentsEnrolled)
                .ThenByDescending(c => c.CourseDuration)
                .ToList();

            foreach (var course in courses)
            {
                sb.AppendLine($"Name: {course.Name}")
                    .AppendLine($"Start Date: {course.StartDate}")
                    .AppendLine($"End Date: {course.EndDate}")
                    .AppendLine($"Duration: {course.CourseDuration.Days}")
                    .AppendLine($"Students enrolled: {course.StudentsEnrolled}");
            }

            return sb.ToString();
        }

        private static string StudentDetails(StudentSystemContext db)
        {
            StringBuilder sb = new StringBuilder();

            var data = db.Students
                .Where(s => s.Courses.Any())
                .Select(s => new
                {
                    s.Name,
                    CoursesCount = s.Courses.Count,
                    TotalCoursePrice = s.Courses.Sum(c => c.Course.Price),
                    AverageCoursePrice = s.Courses.Average(c => c.Course.Price)
                })
                .OrderByDescending(d => d.TotalCoursePrice)
                .ThenByDescending(d => d.CoursesCount)
                .ThenBy(d => d.Name)
                .ToList();

            foreach (var student in data)
            {
                sb.AppendLine($"Name: {student.Name}")
                    .AppendLine($"Number of Courses: {student.CoursesCount}")
                    .AppendLine($"Total courses price: {student.TotalCoursePrice:f2}")
                    .AppendLine($"Average courses price: {student.AverageCoursePrice:f2}");
            }

            return sb.ToString();
        }

        private static string CoursesWithResourcesAndLicenses(StudentSystemContext db)
        {
            StringBuilder sb = new StringBuilder();

            var data = db.Courses
                .OrderByDescending(d => d.Resources.Count)
                .ThenBy(d => d.Name)
                .Select(c => new
                {
                    c.Name,
                    Resources = c.Resources
                        .OrderByDescending(r => r.Licenses.Count)
                        .ThenBy(r => r.Name)
                        .Select(r => new
                        {
                            ResourceName = r.Name,
                            Licenses = r.Licenses.Select(l => l.Name)
                        })
                })
                .ToList();

            foreach (var course in data)
            {
                sb.AppendLine($"Course name: {course.Name}")
                    .AppendLine($"Resources:");

                foreach (var resource in course.Resources)
                {
                    sb.AppendLine(
                        $"Resource {resource.ResourceName} has licenses: {string.Join(", ", resource.Licenses)}");
                }

            }

            return sb.ToString();
        }

        private static string StudentsFullDetails(StudentSystemContext db)
        {
            StringBuilder sb = new StringBuilder();

            var data = db.Students
                .Where(s => s.Courses.Any())
                .Select(s => new
                {
                    s.Name,
                    Courses = s.Courses.Count,
                    Resources = s.Courses.Sum(c => c.Course.Resources.Count),
                    Licenses = s.Courses.Sum(c => c.Course.Resources.Sum(r => r.Licenses.Count))
                })
                .OrderByDescending(d => d.Courses)
                .ThenByDescending(d => d.Resources)
                .ThenBy(d => d.Name)
                .ToList();

            foreach (var student in data)
            {
                sb.AppendLine(
                    $"Student {student.Name} is enrolled in {student.Courses} " +
                    $"courses which have {student.Resources} " +
                    $"resources with {student.Licenses} licenses");
            }

            return sb.ToString();
        }

        private static void Print(string output)
        {
            Console.WriteLine(output);
        }

        private static void SeedData(StudentSystemContext db)
        {
            random = new Random();

            AddStudents(db);
            AddCourses(db);

            var studentIds = db.Students.Select(s => s.Id).ToList();
            var courses = db.Courses.ToList();

            AddStudentsAndResources(db, studentIds, courses);
            AddHomework(db, courses);
            AddLicense(db);
        }

        private static void AddStudentsAndResources(StudentSystemContext db, List<int> studentIds, List<Course> courses)
        {
            Console.WriteLine("Adding students and resources in courses");

            for (int i = 0; i < totalCourses; i++)
            {
                var currentCourse = courses[i];

                var studentsInCourse = random.Next(2, totalStudents / 2);

                // Adding students in courses
                for (int j = 0; j < studentsInCourse; j++)
                {
                    int studentId = studentIds[random.Next(0, totalStudents)];

                    if (!currentCourse.Students.Any(s => s.StudentId == studentId))
                    {
                        currentCourse.Students.Add(new StudentCourse
                        {
                            StudentId = studentId
                        });
                    }
                    else
                    {
                        j--;
                    }
                }

                db.SaveChanges();

                var resourcesCount = random.Next(2, 20);
                var typesOfResource = new[] { 0, 1, 2, int.MaxValue };

                // Adding resources in courses
                for (int j = 0; j < resourcesCount; j++)
                {
                    Resource resource = new Resource()
                    {
                        Name = $"Resourc {i}.{j}",
                        TypeOfCourse = (TypeOfCourse)typesOfResource[random.Next(0, typesOfResource.Length)],
                        URL = $"Lorem Ipsum URL {i}.{j}"
                    };

                    currentCourse.Resources.Add(resource);
                }

                db.SaveChanges();
            }

            Console.WriteLine("students and resources added in courses");
        }

        private static void AddStudents(StudentSystemContext db)
        {
            Console.WriteLine("Adding students");

            for (int i = 0; i < totalStudents; i++)
            {
                Student student = new Student()
                {
                    Name = $"Student {i + 1}",
                    PhoneNumer = $"0123456{i}",
                    RegistrationDate = DateTime.Now.AddMonths(-2 * i)
                };

                db.Students.Add(student);
            }

            db.SaveChanges();

            Console.WriteLine("Students added");
        }

        private static void AddCourses(StudentSystemContext db)
        {
            Console.WriteLine("Adding courses");

            for (int i = 0; i < totalCourses; i++)
            {
                Course course = new Course()
                {
                    Name = $"Course {i}",
                    Description = $"Lorem ipsum {i}",
                    StartDate = DateTime.Now.AddDays(-5 * (i + 1)),
                    EndDate = DateTime.Now.AddDays(5 * (i + 1)),
                    Price = 100 * (i + 1)
                };

                db.Courses.Add(course);
            }

            db.SaveChanges();

            Console.WriteLine("Courses added");
        }

        private static void AddHomework(StudentSystemContext db, List<Course> courses)
        {
            Console.WriteLine("Adding homeworks");

            for (int i = 0; i < courses.Count; i++)
            {
                var currentCourse = courses[i];
                var studentsInCoursesIds = currentCourse.Students
                    .Select(s => s.StudentId)
                    .ToList();

                for (int j = 0; j < studentsInCoursesIds.Count; j++)
                {
                    var totalHomeworks = random.Next(2, 5);

                    for (int k = 0; k < totalHomeworks; k++)
                    {
                        var homework = new Homework()
                        {
                            Content = $"Bla Bla x{(i * j * k) + 1}",
                            SubmissionDate = DateTime.Now.AddDays((k * 1) + 1),
                            TypeOfHomework = TypeOfHomework.ZIP,
                            StudentId = studentsInCoursesIds[j],
                            CourseId = currentCourse.Id
                        };

                        db.Homeworks.Add(homework);
                    }
                }

            }

            db.SaveChanges();

            Console.WriteLine("Homeworks added");
        }

        private static void AddLicense(StudentSystemContext db)
        {
            Console.WriteLine("Adding Licenses");
            var resources = db.Resources;

            foreach (var resource in resources)
            {
                int licensesCount = random.Next(2, 6);

                for (int i = 0; i < licensesCount; i++)
                {
                    resource.Licenses.Add(new License()
                    {
                        Name = $"License Number {DateTime.Now.TimeOfDay}.{(i * 2) + 1}"
                    });
                }
            }

            db.SaveChanges();

            Console.WriteLine("Licenses Added");
        }
    }
}
