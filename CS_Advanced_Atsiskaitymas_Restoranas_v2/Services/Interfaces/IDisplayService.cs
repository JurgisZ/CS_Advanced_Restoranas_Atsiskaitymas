using CS_Advanced_Atsiskaitymas_Restoranas_v2.Models;

namespace CS_Advanced_Atsiskaitymas_Restoranas_v2.Services.Interfaces
{
    internal interface IDisplayService
    {
        int DisplayAddItemToOrderSelectAmount(OrderItem item);
        bool DisplayConfirmContinue(string message, bool clear = true);
        bool DisplayConfirmSelectedTable(Table selectedTable);
        void DisplayHelloMessage();
        (string? userNameLogIn, string? userPassCodeLogIn) DisplayLogInMenu(bool failedAttempt);
        int DisplayMainMenuSelection(ref bool exit, string? additionalMsg = null);
        void DisplayOrderContents(string[] orderItemsStrArr, decimal totalSum);
        void DisplayReset(string[]? additionalMessages = null);
        int DisplaySelectOptionReturnIndex(string[] categories);
        int DisplayStartNewOrderGetTableId(List<Table> availableTables, string? additionalMsg = null);
    }
}