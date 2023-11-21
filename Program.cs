using System.Text.Json;
using System.Diagnostics;
using System.IO;
using BankHelper;

namespace BankHelper
{
    internal class Program
    {
        public static void Main()
        {
            var watch = new Stopwatch();
            watch.Start();

            var doc = JsonDocument.Parse(File.ReadAllText("C:\\Users\\willf\\source\\repos\\BankHelper\\2nb.json"));
            var root = doc.RootElement;
            var mainLobby = ProcessRoom(root, null);

            ProcessOutboundShortcuts(mainLobby, mainLobby);
            ProcessInboundShortcuts(mainLobby, mainLobby);

            Validator.Validate(mainLobby);

            watch.Stop();
            Console.WriteLine($"Time to process and validate: {watch.ElapsedMilliseconds}ms");

            Console.WriteLine("");
        }

        private static Room ProcessRoom(JsonElement jsonRoom, Room parent)
        {
            var room = new Room(jsonRoom.GetProperty("name").GetString());
            
            if (parent != null)
            {
                room.path.AddRange(parent.path);
                room.path.Add(parent);
            }

            room.subrooms = GetSubrooms(jsonRoom, room);
            room.chests = GetChests(jsonRoom, room);
            room.utilities = GetUtilities(jsonRoom);
            room.outboundShortcuts = GetRawShortcuts(jsonRoom, room);

            JsonElement upstreamName;
            if (jsonRoom.TryGetProperty("upstreamName", out upstreamName))
            {
                room.upstreamName = upstreamName.GetString();
            }

            JsonElement downstreamName;
            if (jsonRoom.TryGetProperty("downstreamName", out downstreamName))
            {
                room.downstreamName = downstreamName.GetString();
            }

            JsonElement shortcutName;
            if (jsonRoom.TryGetProperty("shortcutName", out shortcutName))
            {
                room.shortcutName = shortcutName.GetString();
            }

            return room;
        }

        private static List<Shortcut> GetRawShortcuts(JsonElement roomElement, Room room)
        {
            JsonElement jsonShortcuts;
            var shortcuts = new List<Shortcut>();

            if (roomElement.TryGetProperty("shortcuts", out jsonShortcuts))
            {
                foreach (var jsonShortcut in jsonShortcuts.EnumerateArray())
                {
                    var rawChests = new List<RawChest>();
                    foreach (var jsonChest in jsonShortcut.GetProperty("items").EnumerateArray())
                    {
                        RawChest rawChest = new RawChest();

                        switch (jsonChest.ValueKind)
                        {
                            case JsonValueKind.String:
                                rawChest.name = jsonChest.GetString();
                                break;
                            case JsonValueKind.Object:
                                rawChest.name = jsonChest.GetProperty("name").GetString();
                                rawChest.modifier = jsonChest.GetProperty("modifier").GetString();
                                break;
                        }

                        rawChests.Add(rawChest);
                    }
                    var shortcut = new Shortcut(jsonShortcut.GetProperty("room").GetString(), rawChests);
                    shortcut.source = room;

                    shortcuts.Add(shortcut);
                }
            }

            return shortcuts;
        }

        private static Room FindRoom(string name, Room room)
        {
            if (room.name == name)
            {
                return room;
            }
            foreach (var subroom in room.subrooms)
            {
                var subResult = FindRoom(name, subroom);
                if (subResult != null)
                {
                    return subResult;
                }
            }

            return null;
        }

        private static Chest FindChest(RawChest rawChest, Room room)
        {
            foreach (var chest in room.chests)
            {
                if (chest.name == rawChest.name && chest.modifier == rawChest.modifier)
                {
                    return chest;
                }
            }

            foreach (var subroom in room.subrooms)
            {
                var subResult = FindChest(rawChest, subroom);
                if (subResult != null)
                {
                    return subResult;
                } 
            }

            return null;
        }

        // Return list of shortcuts that lead to room
        private static void FindInboundShortcuts(Room target, Room room, List<Shortcut> list)
        {
            foreach (var shortcut in room.outboundShortcuts)
            {
                if (shortcut.destination == target)
                {
                    list.Add(shortcut);
                }

                foreach (var targetOutboundShortcut in target.outboundShortcuts)
                {
                    if (targetOutboundShortcut.destination == shortcut.source && targetOutboundShortcut.source == shortcut.destination)
                    {
                        shortcut.reverse = targetOutboundShortcut;
                        break;
                    }
                }
            }

            foreach (var subroom in room.subrooms)
            {
                FindInboundShortcuts(target, subroom, list);
            }
        }

        private static void ProcessOutboundShortcuts(Room room, Room mainLobby)
        {
            foreach (var shortcut in room.outboundShortcuts)
            {
                shortcut.destination = FindRoom(shortcut.rawDestination, mainLobby);
                if (shortcut.destination == null)
                {
                    throw new InvalidDataException($"INVALID STATE - Failed to find raw destination {shortcut.rawDestination}");
                }

                foreach (var rawChest in shortcut.rawChests)
                {
                    var chest = FindChest(rawChest, mainLobby);
                    if (chest == null)
                    {
                        throw new InvalidDataException($"INVALID STATE - Failed to find raw chest {rawChest}");
                    }
                    shortcut.chests.Add(chest);
                }
            }

            foreach (var subroom in room.subrooms)
            {
                ProcessOutboundShortcuts(subroom, mainLobby);
            }
        }

        private static void ProcessInboundShortcuts(Room room, Room mainLobby)
        {
            FindInboundShortcuts(room, mainLobby, room.inboundShortcuts);
            foreach (var subroom in room.subrooms)
            {
                ProcessInboundShortcuts(subroom, mainLobby);
            }
        }

        private static List<Room> GetSubrooms(JsonElement roomElement, Room parent)
        {
            JsonElement jsonSubrooms;
            var subrooms = new List<Room>();

            if (roomElement.TryGetProperty("subrooms", out jsonSubrooms))
            {

                foreach (var subroom in jsonSubrooms.EnumerateArray())
                {
                    subrooms.Add(ProcessRoom(subroom, parent));
                }
            }

            return subrooms;
        }


        private static List<Utility> GetUtilities(JsonElement roomElement)
        {
            JsonElement jsonUtilities;
            var utilities = new List<Utility>();

            if (roomElement.TryGetProperty("utilities", out jsonUtilities)) {
                foreach (var utility in jsonUtilities.EnumerateArray())
                {
                    utilities.Add(UtilityDictionary.nameToEnum[utility.GetString()]);
                }
            }

            return utilities;
        }

        private static List<Chest> GetChests(JsonElement roomElement, Room room)
        {
            var chests = new List<Chest>();
            
            JsonElement jsonChests;
            
            if (roomElement.TryGetProperty("chests", out jsonChests))
            {
                foreach (var jsonChest in jsonChests.EnumerateArray())
                {
                    var newChest = new Chest(jsonChest.GetProperty("name").GetString());

                    JsonElement jsonSignName;
                    if (jsonChest.TryGetProperty("signName", out jsonSignName))
                    {
                        newChest.signName = jsonSignName.GetString();
                    }

                    JsonElement silo;
                    if (jsonChest.TryGetProperty("silo", out silo))
                    {
                        newChest.silo = silo.GetInt16();
                    }

                    JsonElement modifier;
                    if (jsonChest.TryGetProperty("modifier", out modifier))
                    {
                        newChest.modifier = modifier.GetString();
                    }

                    newChest.home = room;

                    chests.Add(newChest);
                }
            }

            return chests;
        }
    }
}
