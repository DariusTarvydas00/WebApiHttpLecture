
namespace DataLoader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            LoadUsers();
        }

        private static void LoadUsers()
        {
            //string path = @"C:\Users\gzms\Downloads\BX-Users.csv";
            //DbContextOptionsBuilder<MainDbContext> optionsBuilder = new DbContextOptionsBuilder<MainDbContext>();
            //optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=WebApiDb3;Trusted_Connection=True;");
            //DbContextOptions<MainDbContext> options = optionsBuilder.Options;

            //List<User> users = new List<User>();
            //using (var context = new MainDbContext(options))
            //{
            //    using (StreamReader sr = new StreamReader(path))
            //    {
            //        string line = sr.ReadLine();
            //        for (int i = 0; i < 100; i++)
            //        {
            //            line = sr.ReadLine();
            //            string[] columns = line.Split(';');
            //            User user = new User()
            //            {
            //                Id = int.Parse(columns[0]),
            //                Username = columns[1],
            //                Location = columns[2],
            //                Age = columns[3]
            //            };
            //            users.Add(user);
            //        }
            //    }
            //    context.Users.AddRange(users);
            //    context.SaveChanges();
            //}
        }
    }
}
