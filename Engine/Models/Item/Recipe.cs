using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Engine.Models
{
    public class Recipe
    {
        public string ID { get; }
        [JsonIgnore]
        public string Name { get; }
        [JsonIgnore]
        public string NL = Environment.NewLine;
        [JsonIgnore]
        public List<ItemQuantity> Ingredients { get; } = new List<ItemQuantity>();
        [JsonIgnore]
        public List<ItemQuantity> OutputItems { get; } = new List<ItemQuantity>();
        [JsonIgnore]
        public string Description { get; }

        [JsonIgnore]
        public string ToolTipContents =>
            Name + NL + NL +

            "=== Ingredients:" + NL +
            string.Join(NL, Ingredients.Select(i => i.QuantityItemDescription)) +
            NL + NL +

            "=== Results:" + NL +
            string.Join(NL, OutputItems.Select(i => i.QuantityItemDescription));

        public Recipe(string id, string name)
        {
            ID = id;
            Name = name;
        }

        public void AddIngredient(string itemID, int quantity)
        {
            if (!Ingredients.Any(x => x.ID == itemID))
                Ingredients.Add(new ItemQuantity(itemID, quantity));
        }

        public void AddOutputItem(string itemID, int quantity)
        {
            if (!OutputItems.Any(x => x.ID == itemID))
                OutputItems.Add(new ItemQuantity(itemID, quantity));
        }

    }
}