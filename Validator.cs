using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankHelper
{
    internal class Validator
    {
        public static void Validate(Room room)
        {
            ConfirmNoDuplicateRoomNames(room, new List<string>());
            ConfirmNoDuplicateChests(room, new List<RawChest>());
        }

        private static void ConfirmNoDuplicateRoomNames(Room room, List<string> names)
        {
            foreach (var subroom in room.subrooms)
            {
                if (names.Contains(subroom.name))
                {
                    throw new InvalidDataException($"INVALID STATE - Multiple instances of room name {subroom.name}");
                }
                names.Add(subroom.name);
                ConfirmNoDuplicateRoomNames(subroom, names);
            }
        }

        private static void ConfirmNoDuplicateChests(Room room, List<RawChest> rawChests)
        {
            foreach (var chest in room.chests)
            {
                foreach (var rawChest in rawChests)
                {
                    if (chest.name == rawChest.name && chest.modifier == rawChest.modifier)
                    {
                        throw new InvalidDataException($"INVALID STATE - Multiple instances of chest {chest}");
                    }
                }
                rawChests.Add(new RawChest(chest.name, chest.modifier));
            }

            foreach (var subroom in room.subrooms)
            {
                ConfirmNoDuplicateChests(subroom, rawChests);
            }
        }
    }
}
