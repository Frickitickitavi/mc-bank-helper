using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankHelper
{
    internal struct RawChest
    {
        public RawChest(string _name, string _modifier)
        {
            name = _name;
            modifier = _modifier;
        }

        public string name { get; set; }
        public string? modifier { get; set; }

        public override string ToString()
        {
            return modifier == null ? name : $"{name} ({modifier})";
        }
    }
}
