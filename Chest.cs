using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankHelper
{
    internal class Chest
    {
        public Chest(string _name)
        {
            name = _name;
        }

        public override string ToString()
        {
            return modifier == null ? name : $"{name} ({modifier})";
        }

        public string name { get; set; }
        public string? modifier { get; set; }
        public string? signName { get; set; }
        public int? silo { get; set; }

        // Dervied fields, not explicit in JSON
        public Room home { get; set; }
    }
}
