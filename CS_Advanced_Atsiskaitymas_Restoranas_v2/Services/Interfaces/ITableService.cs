using CS_Advanced_Atsiskaitymas_Restoranas_v2.Models;

namespace CS_Advanced_Atsiskaitymas_Restoranas_v2.Services.Interfaces
{
    internal interface ITableService
    {
        void CompleteOrder(int tableId);
        List<Table>? GetAvailableTables();
        Table? GetById(int id);
        void Update(Table table);
    }
}