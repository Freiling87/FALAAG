﻿using FALAAG.Models;
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

        private static void LoadWallsFromNodes(XmlNodeList walls)
        {
            foreach (XmlNode wallNode in walls)
            {
                Wall wall = new Wall(
                    wallNode.AttributeAsString("WallID"),
                    wallNode.AttributeAsString("Name"));

				foreach (XmlNode actionOptionNode in wallNode.SelectNodes("./ActionOptions/ActionOption"))
				{
                    wall.ActionOptions.Add(new ActionOption(
                        actionOptionNode.AttributeAsString("ActionID"),
                        actionOptionNode.AttributeAsString("Name"),
                        Enum.Parse<SkillType>(actionOptionNode.AttributeAsString("SkillType")),
                        actionOptionNode.AttributeAsInt("Difficulty"),
                        actionOptionNode.AttributeAsInt("Audibility"),
                        actionOptionNode.AttributeAsInt("Visibility"),
                        wall,
                        actionOptionNode.AttributeAsInt("Duration")));
                }

				_wallTemplates.Add(wall);
            }
        }

        internal static Wall GetWallByID(string id) =>
            _wallTemplates.FirstOrDefault(w => w.ID == id).Clone();
	}
}