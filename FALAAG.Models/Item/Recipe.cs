using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace FALAAG.Models
{
    public class Recipe
    {
        public string ID { get; }
        [JsonIgnore]
        public string Name { get; set; }
        [JsonIgnore]
        public string Description { get; set; }
        public Item GoalItem { get; set; }

        [JsonIgnore] public List<ItemQuantity> Ingredients { get; }
        [JsonIgnore]
        public List<ItemQuantity> OutputItems { get; }

        [JsonIgnore]
        public string ToolTipContents =>
            Name + Environment.NewLine + Environment.NewLine +
            Description + Environment.NewLine + Environment.NewLine + // TODO: Change to GoalItem.Description

            "Inputs" + Environment.NewLine +
            "===========" + Environment.NewLine +
            string.Join(Environment.NewLine, Ingredients.Select(i => i.QuantityItemDescription)) +
            Environment.NewLine + Environment.NewLine +
            "Results" + Environment.NewLine +
            "===========" + Environment.NewLine +
            string.Join(Environment.NewLine, OutputItems.Select(i => i.QuantityItemDescription));

        public Recipe(string id, string name, string description, List<ItemQuantity> ingredients, List<ItemQuantity> outputItems)
        {
            ID = id;
            Description = description;
            Name = name;
            Ingredients = ingredients;
            OutputItems = outputItems;
        }
    }
}