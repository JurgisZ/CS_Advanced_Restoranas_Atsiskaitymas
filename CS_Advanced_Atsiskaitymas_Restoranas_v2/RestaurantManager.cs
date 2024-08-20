using CS_Advanced_Atsiskaitymas_Restoranas_v2.Services;
using CS_Advanced_Atsiskaitymas_Restoranas_v2.Services.Interfaces;
using CS_Advanced_Atsiskaitymas_Restoranas_v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
//using CS_Advanced_Atsiskaitymas_Restoranas_v2.Services.Interfaces;

namespace CS_Advanced_Atsiskaitymas_Restoranas_v2
{
    internal class RestaurantManager
    {
        private readonly IDisplayService _displayService;//_IDisplayService;
        private readonly IUserService _userService;
        private readonly ITableService _tableService;
        private readonly IOrderService _orderService;
        public User? currentUser { get; private set; } = default;

        public RestaurantManager(IDisplayService displayService, IUserService userService, ITableService tableService, IOrderService orderService)
        {
            _displayService = displayService;
            _userService = userService;
            _tableService = tableService;
            _orderService = orderService;

        }
        public void Start()
        {
            bool exit = false;
            while (!exit)
            {
                //Log in
                if (currentUser == default)
                {
                    _displayService.DisplayHelloMessage();
                    currentUser = Authenticate();
                }

                //Main menu select
                int mainMenuSelection = _displayService.DisplayMainMenuSelection(ref exit, $"Hello {currentUser.Name}.");
                if (exit) continue;

                switch (mainMenuSelection)
                {
                    case 1: //list available tables, select
                        int selectedTableIdandNumber = _displayService.DisplayStartNewOrderGetTableId(_tableService.GetAvailableTables(), $"Hello {currentUser.Name}");
                        if (!_displayService.DisplayConfirmSelectedTable(_tableService.GetById(selectedTableIdandNumber))) break;
                            //Create new order by table number, pass table seats to order
                            int orderId = _orderService.Create(selectedTableIdandNumber, _tableService.GetById(selectedTableIdandNumber).Id);
                            
                            //Assign order to table
                            Table table = _tableService.GetById(selectedTableIdandNumber);
                            table.OrderId = orderId;
                            _tableService.Update(table);
                        
                        break;

                    case 2:
                        //select active order to add to
                        List<Order> activeOrders = _orderService.GetActiveOrders();
                        string[] activeOrdersMenuStrings = _orderService.OrdersListToMenuStringArr(activeOrders);
                        int selectedOrderIndex = _displayService.DisplaySelectOptionReturnIndex(activeOrdersMenuStrings);
                        if (selectedOrderIndex == -1) break;
                        int selectedOrderId = activeOrders[selectedOrderIndex].Id;
                        
                        //Select category: FoodItem, BeverageItem
                        string[] orderItemcategories = new string[] { "FoodItem", "BeverageItem" };
                        int itemCategoryIndex = _displayService.DisplaySelectOptionReturnIndex(orderItemcategories);
                        string continueAddOrderItemsMsg = "Continue with Order selection?";
                        if (!_displayService.DisplayConfirmContinue(continueAddOrderItemsMsg)) break;

                        //display item selection
                        List<OrderItem> itemsByCategory = _orderService.GetMenuItemsByCategory(orderItemcategories[itemCategoryIndex]);
                        //if (itemsByCategory == default) - some message, break
                        
                        string[] orderItemsForSelection = _orderService.OrderItemsListToMenuStringArr(itemsByCategory);
                        int selectedItemIndex = _displayService.DisplaySelectOptionReturnIndex(orderItemsForSelection);

                        //Display item and choose amount
                        if (itemsByCategory.Count > 0)
                        {
                            int amountToAdd = _displayService.DisplayAddItemToOrderSelectAmount(itemsByCategory[selectedItemIndex]);
                            _orderService.AddItemToOrder(selectedOrderId, itemsByCategory[selectedItemIndex], amountToAdd);
                        }

                        //Display all order items
                        _displayService.DisplayOrderContents
                            (_orderService.OrderItemsToMenuStringArrSubTotal(selectedOrderId), _orderService.OrderItemsTotalPrice(selectedOrderId));


                        Console.ReadKey();
                        break;
                    case 3:
                        //Display active orders
                        List<Order> activeOrdersForViewing = _orderService.GetActiveOrders();
                        string[] activeOrdersStrArr = _orderService.OrdersListToMenuStringArr(activeOrdersForViewing);
                        int selectedOrderForViewingIndex = _displayService.DisplaySelectOptionReturnIndex(activeOrdersStrArr);
                        if (selectedOrderForViewingIndex == -1) break;
                        Order selectedOrderForViewing = activeOrdersForViewing[selectedOrderForViewingIndex];

                        //Display order items
                        if (selectedOrderForViewingIndex == -1) break;
                        string[] activeOrderItemsForViewing = _orderService.OrderItemsToMenuStringArrSubTotal(selectedOrderForViewing.Id);
                        
                        _displayService.DisplayOrderContents(activeOrderItemsForViewing, 
                                _orderService.OrderItemsTotalPrice(selectedOrderForViewing.Id));

                        if (_displayService.DisplayConfirmContinue($"Completing order {selectedOrderForViewing.ToMenuString()}{Environment.NewLine}", false))
                        {
                            _orderService.CompleteOrder(selectedOrderForViewing.Id);
                            _tableService.CompleteOrder(selectedOrderForViewing.TableNumber);
                        }
                        //propmt print user reciept?
                        //_displayService.DisplayConfirmContinue("Do you want to print user reciept?");
                        //send email
                        
                        Console.ReadKey();
                        break;
                }

            }

        }
        public User Authenticate()
        {
            bool failedAttemptMsg = false;
            User? user = default;

            while (user == default)
            {
                (string userLogInName, string userLogInPassCode) credentialsPair = _displayService.DisplayLogInMenu(failedAttemptMsg);
                user = _userService.ValidateUser(credentialsPair.userLogInName, credentialsPair.userLogInPassCode);
                failedAttemptMsg = true;
            }
            return user;
        }
    }
}
