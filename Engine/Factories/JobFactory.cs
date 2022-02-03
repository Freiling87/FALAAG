﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Engine.Models;
using Engine.Shared;

namespace Engine.Factories
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
                    itemsToComplete.Add(new ItemQuantity(childNode.AttributeAsString("ID"),
                                                         childNode.AttributeAsInt("Quantity")));

                foreach (XmlNode childNode in node.SelectNodes("./RewardItems/Item"))
                    rewardItems.Add(new ItemQuantity(childNode.AttributeAsString("ID"),
                                                     childNode.AttributeAsInt("Quantity")));

                _Jobs.Add(new Job(node.AttributeAsString("ID"),
                                      node.AttributeAsString("Name"),
                                      node.AttributeAsString("Description"),
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