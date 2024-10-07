using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankHelper
{
    internal static class UtilityDictionary
    {
        public static Dictionary<Utility, string> enumToName = new Dictionary<Utility, string>
        {
            { Utility.ANVIL, "Anvil" },
            { Utility.BLAST_FURNACE, "Blast Furnace" },
            { Utility.BREWING_STAND, "Brewing Stand" },
            { Utility.CARTOGRAPHY_TABLE, "Cartography Table" },
            { Utility.CAULDRON, "Cauldron"},
            { Utility.COMPOSTER, "Composter"},
            { Utility.COMPOSTER_WITH_STORAGE, "Composter with Storage" },
            { Utility.CRAFTING_TABLE, "Crafting Table"},
            { Utility.ENCHANTING_TABLE, "Enchanting Table"},
            { Utility.END_PORTAL, "End Portal"},
            { Utility.ENDER_CHEST, "Ender Chest"},
            { Utility.FLETCHING_TABLE, "Fletching Table"},
            { Utility.FURNACE, "Furnace"},
            { Utility.GRASS_FIELD, "Grass Field" },
            { Utility.GRINDSTONE, "Grindstone"},
            { Utility.JUKEBOX, "Jukebox"},
            { Utility.LECTERN, "Lectern"},
            { Utility.LOOM, "Loom"},
            { Utility.NETHER_PORTAL, "Nether Portal"},
            { Utility.SMITHING_TABLE, "Smithing Table"},
            { Utility.SMOKER, "Smoker"},
            { Utility.STONECUTTER, "Stonecutter"},
            { Utility.WATER_SOURCE, "Water Source"}
        };

        public static Dictionary<string, Utility> nameToEnum = invertDict(enumToName);

        private static Dictionary<string, Utility> invertDict(Dictionary<Utility, string> inDict)
        {
            var outDict = new Dictionary<string, Utility>();

            foreach (Utility inKey in inDict.Keys )
            {
                outDict.Add(inDict[inKey], inKey);
            }

            return outDict;
        }
    }
}
