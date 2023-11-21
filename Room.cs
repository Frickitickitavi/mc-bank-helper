using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankHelper
{
    internal class Room
    {
        private const int BASE_PERIMETER = 4;
        private const int ODD_NUMBERED_CHEST_PERIMETER = 3;
        private const int OUTBOUND_SHORTCUT_PERIMETER = 3;
        private const int NONBIDIRECTIONAL_INBOUND_SHORTCUT_PERIMETER = 3;
        private const int SUBROOM_PERIMETER = 4;
        private const int CHEST_WITH_SILO_PERIMETER = 3;

        public Room(string _name)
        {
            name = _name;
            subrooms = new List<Room>();
            outboundShortcuts = new List<Shortcut>();
            utilities = new List<Utility>();
            chests = new List<Chest>();
        }

        public override string ToString()
        {
            return name;
        }

        public string name { get; set; }
        public string? upstreamName { get; set; }
        public string? downstreamName { get; set; }
        public string? shortcutName { get; set; }
        public List<Room> subrooms { get; set; }
        public List<Shortcut> outboundShortcuts { get; set; } = new List<Shortcut>();
        public List<Utility> utilities { get; set; }
        public List<Chest> chests { get; set; } = new List<Chest>();

        // Derived fields; not explicit in JSON
        public List<Shortcut> inboundShortcuts { get; set; } = new List<Shortcut>();

        public List<Room> path { get; set; } = new List<Room>();

        public int perimeter { get; set; } = 0;

        public int area { get; set; } = 0;

        public Room getParent()
        {
            return path.Last();
        }
        
        private string PrintPath()
        {
            switch (path.Count)
            {
                case 0:
                    if (name == "Main Lobby")
                    {
                        return "n/a";
                    }
                    throw new InvalidDataException($"INVALID STATE - Room ${this} is not Main Lobby but has no path");
                case 1:
                    return path[0].name;
                default:
                    var list = "";
                    foreach (var room in path.GetRange(0, path.Count - 1))
                    {
                        list += $"{room.name} > ";
                    }
                    return $"{list}{path[path.Count - 1]}";
            }
        }

        public int EstimatePerimeter()
        {
            var perimeter = BASE_PERIMETER;
            perimeter += (int)Decimal.Ceiling(Decimal.Divide(chests.Count, 2)) * ODD_NUMBERED_CHEST_PERIMETER;
            perimeter += outboundShortcuts.Count * OUTBOUND_SHORTCUT_PERIMETER;
            perimeter += inboundShortcuts.Count(i => i.reverse == null) * NONBIDIRECTIONAL_INBOUND_SHORTCUT_PERIMETER;
            perimeter += subrooms.Count * SUBROOM_PERIMETER;
            perimeter += chests.Count(c => c.silo != null && c.silo > 0) * CHEST_WITH_SILO_PERIMETER;

            return perimeter;
        }

        public int EstimateArea()
        {
            return (int)Math.Pow((double)Decimal.Divide(perimeter, 4), 2);
        }
    }
}
