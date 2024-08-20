using CS_Advanced_Atsiskaitymas_Restoranas_v2.Repositories;
using CS_Advanced_Atsiskaitymas_Restoranas_v2.Services;
using CS_Advanced_Atsiskaitymas_Restoranas_v2.Models;
using CS_Advanced_Atsiskaitymas_Restoranas_v2.Services.Interfaces;

namespace CS_Advanced_Atsiskaitymas_Restoranas_v2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IDisplayService displayService = new DisplayService();
            
            IRepository<User> userRepository = new Repository<User>(Path.Combine(Directory.GetCurrentDirectory(), "Data", "Users", "Users.csv"));
            IUserService userService = new UserService(userRepository);

            IRepository<Table> tableRepository = new Repository<Table>(Path.Combine(Directory.GetCurrentDirectory(), "Data", "Tables.csv")); 
            ITableService tableService = new TableService(tableRepository);

            IRepository<Order> orderRepository = new Repository<Order>(Path.Combine(Directory.GetCurrentDirectory(), "Data", "Orders", "Orders.csv"));
            IRepository<FoodItem> menuFoodItemsRepository = new Repository<FoodItem>(Path.Combine(Directory.GetCurrentDirectory(), "Data", "Orders", "OrderItems", "FoodItems.csv"));
            IRepository<BeverageItem> menuBeverageItemsRepository = new Repository<BeverageItem>(Path.Combine(Directory.GetCurrentDirectory(), "Data", "Orders", "OrderItems", "BeverageItems.csv"));
            OrderItemsRepository orderItemsRepository = new OrderItemsRepository(Path.Combine(Directory.GetCurrentDirectory(), "Data", "Orders", "OrderItems"));
            OrderService orderService = new OrderService(orderRepository, menuFoodItemsRepository, menuBeverageItemsRepository, orderItemsRepository);

            RestaurantManager restaurantManager = new RestaurantManager(displayService, userService, tableService, orderService);
                        
            restaurantManager.Start();
        }
    }
}
