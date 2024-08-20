using CS_Advanced_Atsiskaitymas_Restoranas_v2.Repositories;
using CS_Advanced_Atsiskaitymas_Restoranas_v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using CS_Advanced_Atsiskaitymas_Restoranas_v2.Services.Interfaces;

namespace CS_Advanced_Atsiskaitymas_Restoranas_v2.Services
{
    internal class OrderService : IOrderService
    {
        private readonly IRepository<Order> _repository;
        private readonly IRepository<FoodItem> _foodItemRepository;
        private readonly IRepository<BeverageItem> _beverageItemRepository;
        private readonly OrderItemsRepository _orderItemsRepository;   //tikriausiai negaliu IRepository naudot
        public OrderService(IRepository<Order> repository, IRepository<FoodItem> foodItemRepository, IRepository<BeverageItem> beverageItemRepository, OrderItemsRepository orderItemsRepository)
        {
            _repository = repository;
            _foodItemRepository = foodItemRepository;
            _beverageItemRepository = beverageItemRepository;
            _orderItemsRepository = orderItemsRepository;
        }
        public int Create(int tableNumber, int tableSeatsNum)
        {
            //create new order using csvLine ctor
            //{base.Id};{TableNumber};{TableSeatsNum};{IsCompleted};{OrderTime.ToString(CultureInfo.InvariantCulture)}"
            string csvLine = $"{_repository.GetLastId() + 1};{tableNumber};{tableSeatsNum};{false};{DateTime.Now.ToString(CultureInfo.InvariantCulture)}";

            Order newOrder = new Order(csvLine);
            _repository.Create(newOrder);
            _orderItemsRepository.CreateOrderItemsFile(newOrder);
            return newOrder == default ? -1 : newOrder.Id;

            //create empty orderId.items file
        }
        public List<Order> GetAll()
        {
            var orders = _repository.GetAll();
            List<Order> ordersWithItems = new List<Order>();
            //load items
            foreach (Order order in orders)
            {
                var tmpOrder = order;
                var items = _orderItemsRepository.GetAll(order.Id);
                ordersWithItems.Add(tmpOrder);
                if (items == null) continue;
                foreach (var item in items)
                {
                    tmpOrder.Items.Add(item);
                }
            }
            return ordersWithItems;
        }
        public Order? GetById(int id)
        {
            Order order = GetAll().Find(x => x.Id == id);
            if (order == null) return null;

            return order;
        }
        public List<Order> GetActiveOrders()
        {
            return GetAll().Where(x => x.IsCompleted == false).ToList();
        }
        public int GetLastId()
        {
            return _repository.GetLastId();
        }
        public List<OrderItem>? GetMenuItemsByCategory(string category)
        {
            switch (category)
            {
                case "FoodItem":
                    List<FoodItem> itemsAsFood = _foodItemRepository.GetAll();
                    List<OrderItem> itemsFoodAsOrderItems = new List<OrderItem>();
                    foreach (FoodItem item in itemsAsFood) //.ToList()
                    {
                        itemsFoodAsOrderItems.Add(item);
                    }
                    return itemsFoodAsOrderItems;

                case "BeverageItem":
                    List<BeverageItem> itemsAsBeverageItems = _beverageItemRepository.GetAll();
                    List<OrderItem> itemsBeverageAsOrderItem = new List<OrderItem>();
                    foreach (BeverageItem item in itemsAsBeverageItems)
                    {
                        itemsBeverageAsOrderItem.Add(item);
                    }
                    return itemsBeverageAsOrderItem;
            }
            return default;
        }
        public string[] OrdersListToMenuStringArr(List<Order> orders)
        {
            string[] ordersStrArr = new string[orders.Count];
            for (int i = 0; i < orders.Count; i++)
            {
                ordersStrArr[i] = orders[i].ToMenuString();
            }
            return ordersStrArr;
        }
        public void Update(Order order)
        {
            _repository.Update(order);
        }
        public string[] OrderItemsListToMenuStringArr(List<OrderItem> orderItems)
        {
            string[] orderItemsStrArr = new string[orderItems.Count];
            for (int i = 0; i < orderItems.Count; i++)
            {
                orderItemsStrArr[i] = $"{orderItems[i].ToMenuString()}";
            }
            return orderItemsStrArr;
        }
        public void AddItemToOrder(int orderId, OrderItem item, int amount)
        {
            if (amount <= 0) return;
            item.Amount = amount;
            _orderItemsRepository.Update(orderId, item);
        }
        public string[]? OrderItemsToMenuStringArrSubTotal(int orderId)
        {
            Order order = GetById(orderId);
            if (order.Items.Count == 0)
                return null;

            string[] strings = new string[order.Items.Count];

            for (int i = 0; i < order.Items.Count; i++)
            {
                strings[i] = $"{order.Items[i].ToMenuString()} Amount: {order.Items[i].Amount}{Environment.NewLine}Subtotal: {order.Items[i].Price * order.Items[i].Amount} Eur{Environment.NewLine}";

            }
            return strings;
        }
        public decimal OrderItemsTotalPrice(int orderId)
        {
            Order order = GetById(orderId);
            if (order.Items.Count == 0) return 0.00m;
            decimal totalAmount = 0.0m;
            foreach (OrderItem item in order.Items)
            {
                totalAmount += item.Amount * item.Price;
            }
            return totalAmount;
        }
        public void CompleteOrder(int orderId)
        {
            _repository.Delete(GetById(orderId));
            _orderItemsRepository.Delete(orderId);
        }

    }
}
