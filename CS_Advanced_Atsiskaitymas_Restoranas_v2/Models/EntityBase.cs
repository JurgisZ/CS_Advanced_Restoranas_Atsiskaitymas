using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Advanced_Atsiskaitymas_Restoranas_v2.Models
{
    internal abstract class EntityBase
    {
        public int Id { get; set; }
        public abstract string ToString();
        public abstract string ToMenuString();

    }
}
