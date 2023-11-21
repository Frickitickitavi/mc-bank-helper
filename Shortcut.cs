using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankHelper
{
    internal class Shortcut
    {
        public Shortcut(string _rawDestination, List<RawChest> _rawChests)
        {
            rawDestination = _rawDestination;
            rawChests = _rawChests;
        }

        public override string ToString() {
            return $"{source} to {destination} for {ListChests()}";
        }

        private string ListChests()
        {
            switch (chests.Count)
            {
                case 0:
                    return "nothing!";
                case 1:
                    return chests[0].name;
                case 2:
                    return $"{chests[0].name} and {chests[1].name}";
                default:
                    var list = "";
                    foreach (var chest in chests.GetRange(0, chests.Count - 1))
                    {
                        list += $"{chest.name}, ";
                    }
                    return $"{list} and {chests[chests.Count - 1]}";
            }
        }

        public string rawDestination { get; set; }
        public List<RawChest> rawChests { get; set; } = new List<RawChest>();

        public string? description { get; set; }


        // Derived fields; not explicit in JSON
        public Room destination { get; set; }
        public List<Chest> chests { get; set; } = new List<Chest>();
        public Room source { get; set; }
        public Shortcut reverse { get; set; }
    }
}
