namespace One_To_Many_Relationship
{
    using Data;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new TestContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
        }
    }
}
