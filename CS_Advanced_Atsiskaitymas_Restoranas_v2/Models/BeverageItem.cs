using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Advanced_Atsiskaitymas_Restoranas_v2.Models
{
    internal class BeverageItem : OrderItem
    {
        public bool IsAlcoholicBeverage { get; set; }
        public BeverageItem(string csvLine) : base(csvLine)
        {
            if(Boolean.TryParse(csvLine.Split(";")[5], out bool isAlcoholic))
            {
                IsAlcoholicBeverage = isAlcoholic;
            }
            else
            {
                IsAlcoholicBeverage = false;
            }
        }
        public override string ToString()
        {
            return $"{base.ToString()};{IsAlcoholicBeverage}";
        }
        public override string ToMenuString()
        {
            return base.ToMenuString();
        }
    }
}
