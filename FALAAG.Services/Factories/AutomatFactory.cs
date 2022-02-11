using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using FALAAG.Models;
using FALAAG.Models.Shared;

namespace FALAAG.Factories
{
    public static class AutomatFactory
    {
        private const string _contentDataFilepath = ".\\GameData\\Automats.xml";

        private static readonly List<Automat> _automatTemplates = new List<Automat>();

        static AutomatFactory()
        {
            if (File.Exists(_contentDataFilepath))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(_contentDataFilepath));

                LoadAutomatsFromNodes(data.SelectNodes("/Automats/Automat"));
            }
            else
                throw new FileNotFoundException($"Missing data file: {_contentDataFilepath}");
        }

        private static void LoadAutomatsFromNodes(XmlNodeList nodes)
        {
            foreach (XmlNode node in nodes)
            {
                Automat Automat = new Automat(node.AttributeAsString("ID"), node.SelectSingleNode("./Name")?.InnerText ?? "");

                foreach (XmlNode childNode in node.SelectNodes("./InventoryItems/Item"))
                {
                    int quantity = childNode.AttributeAsInt("Quantity");

                    // This is to allow for unique items
                    for (int i = 0; i < quantity; i++)
                        Automat.InventoryAddItem(ItemFactory.CreateItem(childNode.AttributeAsString("ID")));
                }

                _automatTemplates.Add(Automat);
            }
        }

        public static Automat GetAutomatByID(string id) =>
            _automatTemplates.FirstOrDefault(t => t.ID == id);
    }
}