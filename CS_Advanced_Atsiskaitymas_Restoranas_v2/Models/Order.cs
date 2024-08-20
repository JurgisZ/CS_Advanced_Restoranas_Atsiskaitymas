using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Advanced_Atsiskaitymas_Restoranas_v2.Models
{
    internal class Order : EntityBase
    {
        //public int OrderId { get; set; }
        public int TableNumber {  get; set; }
        public int TableSeatsNum {  get; set; }
        public bool IsCompleted { get; set; }
        public DateTime OrderTime { get; set; }
        public List<OrderItem> Items { get; private set; } = new List<OrderItem>();
        public Order(string csvLine)
        {
            try
            {
                string[] csvValues = csvLine.Split(';');

                base.Id = Convert.ToInt32(csvValues[0]);
                //OrderId = Convert.ToInt32(csvValues[1]);
                TableNumber = Convert.ToInt32(csvValues[1]);
                TableSeatsNum = Convert.ToInt32(csvValues[2]);
                IsCompleted = Convert.ToBoolean(csvValues[3]);
                OrderTime = DateTime.Parse((csvValues[4]), CultureInfo.InvariantCulture);

            }
            catch(Exception ex) 
            {
                Console.WriteLine("Failed to initialize Order object.");
                Console.WriteLine(ex.Message);
            }
        }

        public override string ToString()
        {
            return $"{base.Id};{TableNumber};{TableSeatsNum};{IsCompleted};{OrderTime.ToString(CultureInfo.InvariantCulture)}";
        }

        public override string ToMenuString()
        {
            return $"Table number: {TableNumber}, Seats: {TableSeatsNum}, ";
        }
        
    }
    
}
