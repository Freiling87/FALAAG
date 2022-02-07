using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using FALAAG.Models;
using FALAAG.Models.Shared;

namespace FALAAG.Factories
{
    public static class WorldFactory
	{
        private const string dataFilepath = ".\\GameData\\Cells.xml";

        public static World CreateWorld()
        {
            World world = new World();

            if (File.Exists(dataFilepath))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(dataFilepath));

                string rootImagePath =
                    data.SelectSingleNode("/Cells")
                        .AttributeAsString("RootImagePath");

                LoadLocationsFromNodes(world,
                                       rootImagePath,
                                       data.SelectNodes("/Cells/Cell"));
            }
            else
                throw new FileNotFoundException($"Missing data file: {dataFilepath}");

            return world;
        }

        private static void LoadLocationsFromNodes(World world, string rootImagePath, XmlNodeList nodes)
        {
            if (nodes == null)
                return;

            foreach (XmlNode node in nodes)
            {
                Cell cell = 
                    new Cell(node.AttributeAsInt("X"),
                                 node.AttributeAsInt("Y"),
                                 node.AttributeAsInt("Z"),
                                 node.AttributeAsString("Name"),
                                 node.AttributeAsString("Description"),
                                 $".{rootImagePath}{node.AttributeAsString("ImageFilename")}");

                AddNPCs(cell, node.SelectNodes("./NPCs/NPC"));
                AddJobs(cell, node.SelectNodes("./Jobs/Job"));
                // TODO: I think Automat was split up to accept multiple, and that's why it's no longer appearing.
                AddAutomats(cell, node.SelectNodes("./Automats/Automat"));

                world.AddLocation(cell);
            }
        }

        private static void AddNPCs(Cell location, XmlNodeList monsters)
        {
            if (monsters == null)
                return;

            foreach (XmlNode monsterNode in monsters)
                location.AddNPC(monsterNode.AttributeAsString("ID"), 
                                monsterNode.AttributeAsInt("Percent"));
        }

        private static void AddJobs(Cell location, XmlNodeList quests)
        {
            if (quests == null)
                return;

            foreach (XmlNode questNode in quests)
                location.JobsHere.Add(JobFactory.GetJobByID(questNode.AttributeAsString("ID")));
        }

        private static void AddAutomat(Cell location, XmlNode automatHere)
        {
            if (automatHere == null)
                return;

            location.AutomatHere=
                AutomatFactory.GetAutomatByID(automatHere.AttributeAsString("ID"));
        }
        private static void AddAutomats(Cell location, XmlNodeList automatsHere)
        {
            if (automatsHere == null)
                return;

            foreach (XmlNode automat in automatsHere)
                location.Automats.Add(AutomatFactory.GetAutomatByID(automat.AttributeAsString("ID")));
        }
    }
}
