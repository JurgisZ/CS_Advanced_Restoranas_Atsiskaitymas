using CS_Advanced_Atsiskaitymas_Restoranas_v2.Models;

namespace CS_Advanced_Atsiskaitymas_Restoranas_v2.Services.Interfaces
{
    internal interface IOrderService
    {
        void AddItemToOrder(int orderId, OrderItem item, int amount);
        void CompleteOrder(int orderId);
        int Create(int tableNumber, int tableSeatsNum);
        List<Order> GetActiveOrders();
        List<Order> GetAll();
        Order? GetById(int id);
        int GetLastId();
        List<OrderItem>? GetMenuItemsByCategory(string category);
        string[] OrderItemsListToMenuStringArr(List<OrderItem> orderItems);
        string[]? OrderItemsToMenuStringArrSubTotal(int orderId);
        decimal OrderItemsTotalPrice(int orderId);
        string[] OrdersListToMenuStringArr(List<Order> orders);
        void Update(Order order);
    }
}