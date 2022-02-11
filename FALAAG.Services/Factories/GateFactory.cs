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
    public static class GateFactory
    {
        private const string _contentDataFilePath = ".\\GameData\\Gates.xml";
        private static readonly List<Gate> _gateTemplates = new List<Gate>();

        static GateFactory()
        {
            if (File.Exists(_contentDataFilePath))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(_contentDataFilePath));

                // XML tags work as a filepath here
                LoadGatesFromNodes(data.SelectNodes("/Gates/Gate"));
            }
            else
                throw new FileNotFoundException($"Missing data file: {_contentDataFilePath}");
        }

        public static Gate CreateGate(string GateTypeID) =>
            _gateTemplates.FirstOrDefault(Gate => Gate.ID == GateTypeID)?.Clone();


        private static void LoadGatesFromNodes(XmlNodeList nodes)
        {
            if (nodes == null)
                return;

            foreach (XmlNode node in nodes)
            {
                Gate gate = new Gate(
                    node.AttributeAsString("ID"),
                    node.AttributeAsString("Name"));

                _gateTemplates.Add(gate);
            }
        }

        internal static Gate GetGateByID(string id) =>
            _gateTemplates.FirstOrDefault(g => g.ID == id).Clone();
	}
}