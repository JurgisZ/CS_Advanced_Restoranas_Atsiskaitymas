using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Advanced_Atsiskaitymas_Restoranas_v2.Models
{
    internal class Table : EntityBase
    {
        //public int TableNumber { get; set; } = base.Id
        public int Seats { get; set; }
        public int? OrderId { get; set; }
        public bool Disabled { get; set; }  //lets say table is physicaly removed due to maintenance or it's winter season and outside tables ar not available
        public Table(string csvLine)
        {
            try
            {
                string[] csvValues = csvLine.Split(';');

                base.Id = Convert.ToInt32(csvValues[0]);
                //TableNumber = Convert.ToInt32(csvValues[1]);
                Seats = Convert.ToInt32(csvValues[1]);
                OrderId = string.IsNullOrEmpty(csvValues[2]) ? null : Convert.ToInt32(csvValues[2]);
                Disabled = Convert.ToBoolean(csvValues[3]);
            }
            catch(Exception ex) 
            {
                Console.WriteLine("Failed to initialize Table object.");
                Console.WriteLine(ex.Message);
            }
        }

        public override string ToString()
        {
            return $"{base.Id};{Seats};{OrderId};{Disabled}";
        }

        public override string ToMenuString()
        {
            return $"{base.Id}. Available seats: { Seats}";
        }
    }
}
