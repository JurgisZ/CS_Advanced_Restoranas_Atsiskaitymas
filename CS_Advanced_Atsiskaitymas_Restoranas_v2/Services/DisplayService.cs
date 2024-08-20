using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS_Advanced_Atsiskaitymas_Restoranas_v2.Models;
using CS_Advanced_Atsiskaitymas_Restoranas_v2.Services.Interfaces;

namespace CS_Advanced_Atsiskaitymas_Restoranas_v2.Services
{
    internal class DisplayService : IDisplayService
    {
        public void DisplayHelloMessage()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Restaurant manager.");
        }
        public void DisplayReset(string[]? additionalMessages = null)
        {
            Console.Clear();
            Console.WriteLine("Restaurant manager 1.0");
            if (additionalMessages != null)
            {
                foreach (string message in additionalMessages)
                {
                    Console.WriteLine(message);
                }
            }
        }
        private bool DisplayConfirmExit(ref bool exit)
        {
            DisplayReset();
            Console.Write("Are you sure you want to quit? y/n: ");
            if (Console.ReadLine() == "y")
            {
                exit = true;
                return true;
            }

            return false;
        }
        public (string? userNameLogIn, string? userPassCodeLogIn) DisplayLogInMenu(bool failedAttempt)
        {
            string? userLogIn, userPass;
            do
            {
                if (failedAttempt)
                {
                    DisplayReset(new string[] { "Incorrect login details. Try again." });
                }
                else
                    DisplayReset();

                Console.Write("Enter log in user name: ");

            }
            while (string.IsNullOrEmpty(userLogIn = Console.ReadLine()));

            do
            {
                DisplayReset();
                Console.Write("Enter passcode: ");

            }
            while (string.IsNullOrEmpty(userPass = Console.ReadLine()));

            return (userLogIn, userPass);
        }
        public int DisplayMainMenuSelection(ref bool exit, string? additionalMsg = null)
        {
            int option = -1;
            do
            {
                DisplayReset(new string[] { additionalMsg });
                Console.WriteLine("1. Create a new order.");
                Console.WriteLine("2. Add to order.");
                Console.WriteLine("3. Complete order.");
                Console.WriteLine("6. Exit.");
                Console.Write("Select option: ");
                if (!int.TryParse(Console.ReadLine(), out option)) continue;

                if (option == 6 && DisplayConfirmExit(ref exit)) break;
            }
            while (option <= 0 || option > 3);//change to array, or change 4 to 3 !!!!!!!!!!!!!!!!!!!!!!!
            return option;
        }
        public int DisplayStartNewOrderGetTableId(List<Table> availableTables, string? additionalMsg = null)    //list tables
        {
            int tableId = -1;
            do
            {
                DisplayReset(new string[] { additionalMsg });

                Console.WriteLine("Creating new order (tables marked in red are unavailable).");
                if (availableTables.Count == 0 || availableTables.Find(x => x.OrderId == null) == default)
                {
                    Console.WriteLine("Currently there are no available tables.");
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                    return -1;
                }

                foreach (var table in availableTables)
                {
                    if (table.OrderId != null)
                    {
                        var consoleFgCol = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(table.ToMenuString());
                        Console.ForegroundColor = consoleFgCol;
                    }
                    else
                        Console.WriteLine(table.ToMenuString());
                }
                Console.Write("Select one of availalbe tables: ");
            }
            while ((tableId = VerifyTableSelectionReturnId(availableTables, Console.ReadLine())) == -1);

            return tableId;
        }

        private int VerifyTableSelectionReturnId(List<Table> tables, string selection)
        {
            if (!int.TryParse(selection, out var selectionInt))
                return -1;

            var table = tables.Where(x => x.OrderId == null).ToList().Find(x => x.Id == selectionInt);
            if (table == null) return -1;

            return table.Id;
        }
        public bool DisplayConfirmSelectedTable(Table selectedTable)
        {
            DisplayReset();
            if (selectedTable == null) return false;
            Console.WriteLine($"You have selected table number {selectedTable.Id} with {selectedTable.Seats} available seats.");
            Console.Write("Do you want to continue? y/n: ");
            if (Console.ReadLine() == "y")
                return true;
            return false;
        }

        public bool DisplayConfirmContinue(string message, bool clear = true)
        {
            if (clear) DisplayReset();
            Console.WriteLine(message);
            Console.Write("Do you want to continue? y/n: ");
            if (Console.ReadLine() == "y")
                return true;
            return false;
        }
        public int DisplaySelectOptionReturnIndex(string[] categories)
        {
            int selection = -1;
            do
            {
                DisplayReset();
                Console.WriteLine("Available options:");
                if (categories.Length == 0)
                {
                    Console.WriteLine("No options found. Press any key to continue...");
                    Console.ReadKey();
                    return -1;
                }
                for (int i = 0; i < categories.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {categories[i]}");
                }
                Console.Write("Select option: ");
                if (!int.TryParse(Console.ReadLine(), out selection))
                    continue;

            }
            while (CategorySelectValidate(selection, categories) == -1);
            return selection - 1;
        }
        private int CategorySelectValidate(int selection, string[] categories)
        {
            if (categories == null) return -1;
            if ((selection > 0 && selection <= categories.Length))
            {
                return selection;
            }
            else
                return -1;
        }
        public int DisplayAddItemToOrderSelectAmount(OrderItem item)
        {
            string? line;
            int amount = -1;
            do
            {
                DisplayReset();
                Console.WriteLine("Adding " + item.ToMenuString());
                Console.Write("Enter amount to add: ");
                line = Console.ReadLine();
            }
            while (!int.TryParse(line, out amount) && amount < 0);
            return amount;
        }
        public void DisplayOrderContents(string[] orderItemsStrArr, decimal totalSum)
        {
            DisplayReset();
            if (orderItemsStrArr == null || orderItemsStrArr.Length == 0)
            {
                Console.WriteLine($"Current order is empty.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Current order items:");
            foreach (string itemStr in orderItemsStrArr)
            {
                Console.WriteLine($"{itemStr}");
            }

            Console.WriteLine($"Total price: {totalSum} Eur.");
        }

    }
}
