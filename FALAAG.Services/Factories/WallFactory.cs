using FALAAG.Models;
using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using FALAAG.Actions;
using FALAAG.Models.Shared;

namespace FALAAG.Factories
{
    public static class WallFactory
    {
        private const string _contentDataFilePath = ".\\GameData\\Walls.xml";
        private static readonly List<Wall> _wallTemplates = new List<Wall>();

        static WallFactory()
        {
            if (File.Exists(_contentDataFilePath))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(_contentDataFilePath));

                // XML tags work as a filepath here
                LoadWallsFromNodes(data.SelectNodes("/Walls/Wall"));
            }
            else
                throw new FileNotFoundException($"Missing data file: {_contentDataFilePath}");
        }

        public static Wall CreateWall(string id) =>
            _wallTemplates.FirstOrDefault(w => w.ID == id)?.Clone();

        private static void LoadWallsFromNodes(XmlNodeList nodes)
        {
            if (nodes == null)
                return;

            foreach (XmlNode node in nodes)
            {
                Wall wall = new Wall(
                    node.AttributeAsString("ID"),
                    node.AttributeAsString("Name"));

                _wallTemplates.Add(wall);
            }
        }

        internal static Wall GetWallByID(string id) =>
            _wallTemplates.FirstOrDefault(g => g.ID == id).Clone();
	}
}