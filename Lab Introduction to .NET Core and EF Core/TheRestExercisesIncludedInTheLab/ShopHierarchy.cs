namespace TheRestExercisesIncludedInTheLab
{
    using System;
    using Data;
    using System.Linq;

    class ShopHierarchy
    {

        static void Main(string[] args)
        {
            using (ShopContext db = new ShopContext())
            {
                InitializeDatabase(db);
                AddSalesmans(db);
                AddItems(db);
                ReadCommands(db);
                // PrintSalesmansAndTheirCustomers(db);
                // PrintCustomersWithOrdersAndReviewsCount(db);
                // PrintDetailsForCustomerWithId(db);
                // PrintDetailsForCustomerAndHisSalesmanByCustomerId(db);
                PrintOrdersWithMoreThanOneItemCount(db);
            }

        }

        private static void InitializeDatabase(ShopContext db)
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }

        private static void AddSalesmans(ShopContext db)
        {
            var salesmans = Console.ReadLine().Split(';');
            foreach (var salesman in salesmans)
            {
                var toBeAdded = new Salesman { Name = salesman };
                db.Salesmans.Add(toBeAdded);
            }

            db.SaveChanges();
        }

        private static void ReadCommands(ShopContext db)
        {
            while (true)
            {
                var input = Console.ReadLine().Split('-');
                var command = input[0];
                if (command == "END")
                {
                    break;
                }
                var args = input[1];
                switch (command)
                {
                    case "register":
                        RegisterCustomers(db, args);
                        continue;
                    case "order":
                        AddOrder(db, args);
                        continue;
                    case "review":
                        AddReview(db, args);
                        continue;
                }
            }
        }

        private static void RegisterCustomers(ShopContext db, string args)
        {
            var tokens = args.Split(';');
            var name = tokens[0];
            var salesmanId = int.Parse(tokens[1]);
            var customer = new Customer
            {
                Name = name,
                SalesmanId = salesmanId
            };

            db.Customers.Add(customer);
            db.SaveChanges();
        }

        private static void PrintSalesmansAndTheirCustomers(ShopContext db)
        {
            var salesmanWithCustomersCount = db
                .Salesmans
                .Select(x => new
                {
                    Name = x.Name,
                    CustomersCount = x.Customers.Count
                })
                .OrderByDescending(x => x.CustomersCount)
                .ThenBy(x => x.Name)
                .ToList();

            foreach (var salesman in salesmanWithCustomersCount)
            {
                Console.WriteLine($"{salesman.Name}: {salesman.CustomersCount} customers");
            }
        }

        private static void AddOrder(ShopContext db, string args)
        {
            var tokens = args.Split(';');

            var customerId = int.Parse(tokens[0]);

            Order order = new Order();

            foreach (var token in tokens.Skip(1))
            {
                order.Items.Add(new ItemOrder
                {
                    ItemId = int.Parse(token)
                });
            }

            db.Customers.FirstOrDefault(c => c.Id == customerId).Orders.Add(order);

            db.SaveChanges();
        }

        private static void AddReview(ShopContext db, string args)
        {
            var tokens = args.Split(';');
            var customerId = int.Parse(tokens[0]);

            Review review = new Review
            {
                ItemId = int.Parse(tokens[1])
            };

            db.Customers.FirstOrDefault(c => c.Id == customerId).Reviews.Add(review);

            db.SaveChanges();
        }

        private static void PrintCustomersWithOrdersAndReviewsCount(ShopContext db)
        {
            var customersWithOrdersAndReviewsCount = db.Customers
                .Select(c => new
                {
                    Name = c.Name,
                    OrdersCount = c.Orders.Count,
                    ReviewsCount = c.Reviews.Count
                })
                .OrderByDescending(d => d.OrdersCount)
                .ThenByDescending(d => d.ReviewsCount)
                .ToList();

            foreach (var customer in customersWithOrdersAndReviewsCount)
            {
                string newLine = Environment.NewLine;
                Console.WriteLine($"{customer.Name}{newLine}Orders: {customer.OrdersCount}{newLine}Reviews: {customer.ReviewsCount}");
            }
        }

        private static void AddItems(ShopContext db)
        {
            while (true)
            {
                var input = Console.ReadLine().Split(';');

                if (input[0] == "END")
                {
                    break; ;
                }

                var itemName = input[0];
                var price = decimal.Parse(input[1]);

                db.Items.Add(new Item
                {
                    Name = itemName,
                    Price = price
                });
            }
            db.SaveChanges();
        }

        private static void PrintDetailsForCustomerWithId(ShopContext db)
        {
            var customerId = int.Parse(Console.ReadLine());

            var details = db.Customers
                .Where(c => c.Id == customerId)
                .Select(d => new
                {
                    Orders = d.Orders.Select(o => new
                    {
                        o.Id,
                        ItemsCount = o.Items.Count
                    })
                        .OrderBy(o => o.Id)
                        .ToList(),
                    Reviews = d.Reviews.Count
                })
                .FirstOrDefault();

            foreach (var order in details.Orders)
            {
                Console.WriteLine($"order {order.Id}: {order.ItemsCount} items");
            }
            Console.WriteLine($"reviews: {details.Reviews}");
        }

        private static void PrintDetailsForCustomerAndHisSalesmanByCustomerId(ShopContext db)
        {
            var customerId = int.Parse(Console.ReadLine());

            var details = db.Customers
                .Where(c => c.Id == customerId)
                .Select(d => new
                {
                    d.Name,
                    OrdersCount = d.Orders.Count,
                    ReviewsCount = d.Reviews.Count,
                    SalesmanName = d.Salesman.Name
                })
                .FirstOrDefault();

            Console.WriteLine($"Customer: {details.Name}");
            Console.WriteLine($"Orders count: {details.OrdersCount}");
            Console.WriteLine($"Reviews count: {details.ReviewsCount}");
            Console.WriteLine($"Salesman: {details.SalesmanName}");
        }

        private static void PrintOrdersWithMoreThanOneItemCount(ShopContext db)
        {
            var customerId = int.Parse(Console.ReadLine());

            var count = db.Customers
                .Where(c => c.Id == customerId)
                .Select(d => d.Orders
                        .Where(o => o.Items.Count > 1)
                        .Count())
                .FirstOrDefault();

            Console.WriteLine("Orders:" + count);
        }
    }
}
