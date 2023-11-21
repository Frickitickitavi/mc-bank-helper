using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankHelper
{
    internal class Room
    {
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
        public List<Shortcut> outboundShortcuts { get; set; }
        public List<Utility> utilities { get; set; }
        public List<Chest> chests { get; set; }

        // Derived fields; not explicit in JSON
        public List<Shortcut> inboundShortcuts { get; set; } = new List<Shortcut>();

        public List<Room> path { get; set; } = new List<Room>();
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
    }
}
