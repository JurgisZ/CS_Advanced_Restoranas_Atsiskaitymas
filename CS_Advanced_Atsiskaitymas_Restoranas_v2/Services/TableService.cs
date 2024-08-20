using CS_Advanced_Atsiskaitymas_Restoranas_v2.Repositories;
using CS_Advanced_Atsiskaitymas_Restoranas_v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS_Advanced_Atsiskaitymas_Restoranas_v2.Services.Interfaces;

namespace CS_Advanced_Atsiskaitymas_Restoranas_v2.Services
{
    internal class TableService : ITableService
    {
        private readonly IRepository<Table> _repository;
        public TableService(IRepository<Table> repository)
        {
            _repository = repository;
        }
        public List<Table>? GetAvailableTables()
        {
            var tables = _repository.GetAll().Where(x => !x.Disabled).ToList();
            tables = tables.OrderBy(x => x.Id).ToList();
            return tables;
        }
        public Table? GetById(int id)
        {
            return _repository.GetById(id);
        }
        public void Update(Table table)
        {
            _repository.Update(table);
        }
        public void CompleteOrder(int tableId)
        {
            Table table = (Table)_repository.GetById(tableId);
            if (table == null) return;
            table.OrderId = null;
            _repository.Update(table);

        }
    }
}
