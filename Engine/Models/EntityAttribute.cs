﻿using Engine.Services;

namespace Engine.Models
{
    // Pattern: Constructor Chain
    public class EntityAttribute : BaseNotificationClass
    {
        private int _modifiedValue;
        public int Modifier { get; set; }
        public string Key { get; }
        public string DisplayName { get; }
        public string DiceNotation { get; }
        public int BaseValue { get; set; }

        public int ModifiedValue
        {
            get => _modifiedValue;
            set
            {
                _modifiedValue = value;
                OnPropertyChanged();
            }
        }

        // Constructor that will use DiceService to create a BaseValue.
        // The constructor this calls will put that same value into BaseValue and ModifiedValue
        public EntityAttribute(string key, string displayName, string diceNotation)                                     // A
            : this(key, displayName, diceNotation, DiceService.Instance.Roll(diceNotation).Value)                       // B
        { }

        // Constructor that takes a baseValue and also uses it for modifiedValue,
        // for when we're creating a new attribute
        public EntityAttribute(string key, string displayName, string diceNotation, int baseValue)                      // B
            : this(key, displayName, diceNotation, baseValue, baseValue)                                                // C
        { }

        // This constructor is eventually called by the others, 
        // or used when reading a Player's attributes from a saved game file.
        public EntityAttribute(string key, string displayName, string diceNotation, int baseValue, int modifiedValue)   // C
        {
            Key = key;
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