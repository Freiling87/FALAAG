using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using FALAAG.Models;
using FALAAG.Services;
using FALAAG.Models.Shared;
using FALAAG.Core;

namespace FALAAG.Factories
{
    public static class NPCFactory
    {
        private const string _contentDataFilePath = ".\\GameData\\NPCs.xml";
        private static readonly GameDetails s_gameDetails;
        private static readonly List<NPC> s_NPCTemplates = new List<NPC>();

        static NPCFactory()
        {
            if (File.Exists(_contentDataFilePath))
            {
                s_gameDetails = GameDetailsService.ReadGameDetails();
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(_contentDataFilePath));

                string rootImagePath =
                    data.SelectSingleNode("/NPCs")
                        .AttributeAsString("RootImagePath");

                LoadNPCsFromNodes(data.SelectNodes("/NPCs/NPC"), rootImagePath);
            }
            else
                throw new FileNotFoundException($"Missing data file: {_contentDataFilePath}");
        }

        public static NPC GetNPCFromCellEncounters(Cell cell)
        {
            if (!cell.Encounters.Any())
                return null;

            int totalChances = cell.Encounters.Sum(m => m.ChanceOfEncountering);
            int randomNumber = DiceService.Instance.Roll(totalChances, 1).Value;
            int runningTotal = 0;

            foreach (NPCEncounter NPCEncounter in cell.Encounters)
            {
                runningTotal += NPCEncounter.ChanceOfEncountering;

                if (randomNumber <= runningTotal)
                    return GetNPC(NPCEncounter.NpcID);
            }

            return GetNPC(cell.Encounters.Last().NpcID);
        }

        private static void LoadNPCsFromNodes(XmlNodeList nodes, string rootImagePath)
        {
            if (nodes == null)
                return;

            foreach (XmlNode node in nodes)
            {
                var attributes = s_gameDetails.Attributes;

                // Temporary, be patient.
                attributes.First(a => a.AttributeKey.Equals(AttributeKey.GMS)).BaseValue =
                    Convert.ToInt32(node.SelectSingleNode("./Dexterity").InnerText);
                attributes.First(a => a.AttributeKey.Equals(AttributeKey.GMS)).ModifiedValue =
                    Convert.ToInt32(node.SelectSingleNode("./Dexterity").InnerText);

                NPC npc = new (
                    node.AttributeAsString("ID"),
                    RandomName(),
                    node.AttributeAsString("NameGeneral"),
                    $".{rootImagePath}{node.AttributeAsString("ImagePath")}",
                    attributes,
                    new List<Skill>(),
                    ItemFactory.CreateItem(node.AttributeAsString("WeaponID")),
                    node.AttributeAsInt("XPReward"));

                XmlNodeList lootItemNodes = node.SelectNodes("./LootItems/LootItem");

                if (lootItemNodes != null)
                    foreach (XmlNode lootItemNode in lootItemNodes)
                        npc.AddItemToLootTable(
                            lootItemNode.AttributeAsString("ID"),
                            lootItemNode.AttributeAsInt("Percentage"));

                s_NPCTemplates.Add(npc);
            }
        }

        private static NPC GetNPC(string id)
        {
            NPC newNPC = s_NPCTemplates.FirstOrDefault(m => m.ID == id).Clone();

            foreach (ItemPercentage itemPercentage in newNPC.LootTable)
                if (DiceService.Instance.Roll(100).Value <= itemPercentage.Percentage)
                    newNPC.InventoryAddItem(ItemFactory.CreateItem(itemPercentage.ID));

            return newNPC;
        }

        public static string RandomName()
		{
            // Yeah do this

            return "John Doe";
		}
    }
}