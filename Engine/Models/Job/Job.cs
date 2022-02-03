using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Engine.Models
{
    public class Job
    {
		#region Header
		public string ID { get; }
        [JsonIgnore]
        public string Name { get; }
        [JsonIgnore]
        public string NL = Environment.NewLine;
        [JsonIgnore]
        public string Description { get; }

        [JsonIgnore]
        public int CashAdvance { get; }
        [JsonIgnore]
        public int CashRequired { get; }
        [JsonIgnore]
        public int CashReward { get; }
        [JsonIgnore]
        public List<ItemQuantity> ItemsAdvance { get; }
        [JsonIgnore]
        public List<ItemQuantity> ItemsRequired { get; }
        [JsonIgnore]
        public List<ItemQuantity> ItemsReward { get; }
        [JsonIgnore]
        public int XpReward { get; }

        [JsonIgnore]
        public string ToolTipContents =>
            Name + NL + NL +
            Description + NL + NL +
            "=== Required:" + NL +
            string.Join(NL, ItemsRequired.Select(i => i.QuantityItemDescription)) +
            NL + NL +

            "=== Reward:" + NL +
            $"{XpReward} XP" + NL +
            $"${CashReward}" + NL +
            string.Join(NL, ItemsReward.Select(i => i.QuantityItemDescription));

        public Job(string id, string name, string description, List<ItemQuantity> itemsToComplete, int rewardExperiencePoints, int rewardCash, List<ItemQuantity> rewardItems)
        {
            ID = id;
            Name = name;
            Description = description;
            ItemsRequired = itemsToComplete;
            XpReward = rewardExperiencePoints;
            CashReward = rewardCash;
            ItemsReward = rewardItems;
        }
        #endregion
    }
}