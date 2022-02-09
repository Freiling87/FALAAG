using FALAAG.Core;
using System.Collections.Generic;
using System.ComponentModel;

namespace FALAAG.Models
{
    // Pattern: Constructor Chain
    public class EntityAttribute : INotifyPropertyChanged
    {
		public int BaseValue { get; set; }
        public string DisplayName { get; }
        public string DiceNotation { get; }
        public string ID { get; }
		public int ModifiedValue { get; set; }
        public string ModifiedValueDescriptor
		{
			get
			{
				switch (ModifiedValue)
                {
                    case <= 5:      // 00 - 05       5
                        return "Extremely Low";
                    case <= 20:     // 06 - 20      14
                        return "Very Low";
                    case <= 35:     // 21 - 35      14
                        return "Low";
                    case <= 65:     // 36 - 65      19
                        return "Average";
                    case <= 80:     // 66 - 80      14
                        return "High";
                    case <= 95:     // 81 - 95      14
                        return "Very High";
                    case <= 100:    // 96 - 100      5
                        return "Extremely High";
                    default:
                        return "ERROR";
                }
			}
		}
		public int Modifier { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public string Description { get; set; }
        public List<Skill> UsedIn = new List<Skill>(); // Should be a one-time generated assignment by querying all skills that have a matching SkillComponent, in order of its importance.

        public EntityAttribute(string key, string displayName, string description, string diceNotation)
            : this(key, displayName, description, diceNotation, DiceService.Instance.Roll(diceNotation).Value)
        { }

        public EntityAttribute(string key, string displayName, string description, string diceNotation, int baseValue)
            : this(key, displayName, description, diceNotation, baseValue, baseValue)
        { }

        public EntityAttribute(string id, string displayName, string description, string diceNotation, int baseValue, int modifiedValue)
        {
            ID = id;
            Description = description;
            DisplayName = displayName;
            DiceNotation = diceNotation;
            BaseValue = baseValue;
            ModifiedValue = modifiedValue;
        }

        public void ReRoll()
        {
            BaseValue = DiceService.Instance.Roll(DiceNotation).Value;
            ModifiedValue = BaseValue;
        }
    }
}