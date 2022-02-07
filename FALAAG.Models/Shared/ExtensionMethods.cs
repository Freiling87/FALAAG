﻿using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Xml;
using FALAAG.Models;
using System.Collections.Generic;

namespace FALAAG.Models.Shared
{
    public static class ExtensionMethods
    {
        public static int AttributeAsInt(this XmlNode node, string attributeName) =>
            Convert.ToInt32(node.AttributeAsString(attributeName));

        public static string AttributeAsString(this XmlNode node, string attributeName)
        {
            XmlAttribute attribute = node.Attributes?[attributeName];

            if (attribute == null)
                throw new ArgumentException($"The attribute '{attributeName}' does not exist");

            return attribute.Value;
        }

        public static string StringValueOf(this JObject jsonObject, string key)
        {
            return jsonObject[key].ToString();
        }

        public static string StringValueOf(this JToken jsonToken, string key)
        {
            return jsonToken[key].ToString();
        }

        public static int IntValueOf(this JToken jsonToken, string key)
        {
            return Convert.ToInt32(jsonToken[key]);
        }

        public static EntityAttribute GetAttribute(this Entity entity, string attributeKey)
        {
            return entity.Attributes.First(pa => pa.Key.Equals(attributeKey, StringComparison.CurrentCultureIgnoreCase));
        }

        public static List<Item> ItemsThatAre(this IEnumerable<Item> inventory, Item.ItemCategory category) =>
            inventory.Where(i => i.Category == category).ToList();
    }
}