using CS_Advanced_Atsiskaitymas_Restoranas_v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CS_Advanced_Atsiskaitymas_Restoranas_v2.Repositories
{
    internal class OrderItemsRepository
    {
        private readonly string _orderItemsdirectoryPath;
        public OrderItemsRepository(string orderItemsdirectoryPath)
        {
            _orderItemsdirectoryPath = orderItemsdirectoryPath;
        }
        private int GetOrderId(Order order)
        {
            int orderId = -1;
            try
            {
                var propertyInfo = typeof(Order).GetProperty("Id");
                object propertyValue = propertyInfo.GetValue(order, null);
                orderId = (int)propertyValue;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return orderId;
        }
        private string GetFilePathByOrderId(int orderId)
        {
            string fileName = $"{orderId}.csv";
            return Path.Combine(_orderItemsdirectoryPath, fileName);

        }
        public void CreateOrderItemsFile(Order order)
        {
            string fullPath = GetFilePathByOrderId(order.Id);
            try
            {
                //ar turim overwritint ar ne?
                if (!File.Exists(Path.Combine(fullPath)))
                    File.Create(fullPath).Close();

                //use update here
                using (var writer = new StreamWriter(fullPath, append: true))
                {
                    foreach (var item in order.Items)
                    {
                        writer.WriteLine(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public List<OrderItem>? GetAll(int orderId)
        {
            string fullPath = GetFilePathByOrderId(orderId);
            List<OrderItem> items = new List<OrderItem>();

            try
            {
                if (!File.Exists(fullPath)) return null;
                string? csvLine = null;
                using (var reader = new StreamReader(fullPath))
                {
                    while (null != (csvLine = reader.ReadLine()))
                    {
                        switch (csvLine.Split(";")[1]) //1 - Type: string FoodItem, BeverageItem
                        {
                            case "FoodItem":
                                items.Add(new FoodItem(csvLine));
                                break;
                            case "BeverageItem":
                                items.Add(new BeverageItem(csvLine));
                                break;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return items;

        }
        public OrderItem? GetByOrderIdAndItemId(int orderId, int itemId)
        {
            var items = GetAll(orderId);
            foreach (OrderItem item in items)
            {
                if (itemId == item.Id)
                {
                    return item;
                }
            }
            return default;
        }

        public int GetLastId(int orderId)
        {
            var items = GetAll(orderId);
            int newMax = -1;
            foreach (OrderItem item in items)
            {
                newMax = item.Id <= newMax ? newMax : item.Id;  // < ?             
            }
            return newMax;
        }

        public void Update(int orderId, OrderItem orderItem)
        {
            if (orderItem == null) return;
            string fullPath = GetFilePathByOrderId(orderId);
            List<OrderItem> items = new List<OrderItem>();
            try
            {
                string csvLine;
                using (var reader = new StreamReader(fullPath))
                {
                    while (null != (csvLine = reader.ReadLine()))
                    {
                        OrderItem? orderItemFromFile = null;
                        var csvLineArr = csvLine.Split(";");
                        switch (csvLineArr[1])                      //string type: FoodItem, BeverageItem
                        {
                            case "FoodItem":
                                orderItemFromFile = new FoodItem(csvLine);
                                break;
                            case "BeverageItem":
                                orderItemFromFile = new BeverageItem(csvLine);
                                break;
                        }
                        if (orderItemFromFile == null) continue;

                        if (orderItem.Id == orderItemFromFile.Id)   //update amount
                        {
                            if (orderItem.Amount > 0)
                            {
                                orderItemFromFile.Amount += orderItem.Amount;
                                items.Add(orderItemFromFile);
                            }
                        }
                        else
                        {
                            items.Add(orderItemFromFile);
                        }
                    }
                    if (items.Find(x => x.Id == orderItem.Id) == null)
                        items.Add(orderItem);
                }

                File.Create(fullPath).Close();
                using (var writer = new StreamWriter(fullPath, append: true))
                {
                    foreach (var item in items)
                    {
                        writer.WriteLine(item.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to update entity.");
                Console.WriteLine(ex.Message);
            }
        }
        public void Delete(int orderId)    //deletes file
        {
            string fullPath = GetFilePathByOrderId(orderId);
            try
            {
                File.Delete(fullPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to remove Order items.");
                Console.WriteLine(ex.Message);
            }
        }
    }
}