using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using FALAAG.Models;
using FALAAG.Models.Shared;

namespace FALAAG.Factories
{
    internal static class JobFactory
    {
        private const string _contentDataFilePath = ".\\GameData\\Jobs.xml";

        private static readonly List<Job> _Jobs = new List<Job>();

        static JobFactory()
        {
            if (File.Exists(_contentDataFilePath))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(_contentDataFilePath));
                LoadJobsFromNodes(data.SelectNodes("/Jobs/Job"));
            }
            else
                throw new FileNotFoundException($"Missing data file: {_contentDataFilePath}");
        }

        private static void LoadJobsFromNodes(XmlNodeList nodes)
        {
            foreach (XmlNode node in nodes)
            {
                List<ItemQuantity> itemsToComplete = new List<ItemQuantity>();
                List<ItemQuantity> rewardItems = new List<ItemQuantity>();

                foreach (XmlNode childNode in node.SelectNodes("./ItemsToComplete/Item"))
                {
                    Item item = ItemFactory.CreateItem(childNode.AttributeAsString("ID"));
                    itemsToComplete.Add(new ItemQuantity(item, childNode.AttributeAsInt("Quantity")));
                }

                foreach (XmlNode childNode in node.SelectNodes("./RewardItems/Item"))
                {
                    Item item = ItemFactory.CreateItem(childNode.AttributeAsString("ID"));
                    rewardItems.Add(new ItemQuantity(item, childNode.AttributeAsInt("Quantity")));
                }

                _Jobs.Add(new Job(node.AttributeAsString("ID"),
                                node.SelectSingleNode("./Name")?.InnerText ?? "",
                                node.SelectSingleNode("./Description")?.InnerText ?? "",
                                itemsToComplete,
                                node.AttributeAsInt("RewardXP"),
                                node.AttributeAsInt("RewardCash"),
                                rewardItems));
            }
        }


        internal static Job GetJobByID(string id) =>
            _Jobs.FirstOrDefault(Job => Job.ID == id);
    }
}