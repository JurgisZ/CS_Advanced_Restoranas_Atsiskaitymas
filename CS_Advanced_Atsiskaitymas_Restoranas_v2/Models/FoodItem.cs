using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Advanced_Atsiskaitymas_Restoranas_v2.Models
{
    internal class FoodItem : OrderItem
    {
        public FoodItem(string csvLine) : base(csvLine)
        {

        }
        public override string ToString() 
        { 
            return base.ToString();
        }
        public override string ToMenuString()
        {
            return base.ToMenuString();
        }
    }
}
