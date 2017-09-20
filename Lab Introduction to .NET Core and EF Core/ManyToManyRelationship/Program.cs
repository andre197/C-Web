namespace ManyToManyRelationship
{
    public class Program
    {
        public static void Main()
        {
            using (var db = new UniversityContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
        }
    }
}
