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
        public string Key { get; }
		public int ModifiedValue { get; set; }
		public int Modifier { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public string Description { get; set; }
        public List<Skill> UsedIn = new List<Skill>(); // Should be a one-time generated assignment by querying all skills that have a matching SkillComponent, in order of its importance.

        public EntityAttribute(string key, string displayName, string description, string diceNotation)                                     // A
            : this(key, displayName, description, diceNotation, DiceService.Instance.Roll(diceNotation).Value)                       // B
        { }

        public EntityAttribute(string key, string displayName, string description, string diceNotation, int baseValue)                      // B
            : this(key, displayName, description, diceNotation, baseValue, baseValue)                                                // C
        { }

        public EntityAttribute(string key, string displayName, string description, string diceNotation, int baseValue, int modifiedValue)   // C
        {
            Key = key;
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