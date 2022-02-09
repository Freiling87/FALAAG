using System;
using System.Collections.ObjectModel;
using System.Linq;
using FALAAG.Factories;
using FALAAG.Models;
using FALAAG.Services;
using System.ComponentModel;

namespace FALAAG.ViewModels
{
    public class CharacterCreationViewModel : INotifyPropertyChanged
    {
		private int _totalMod;
        private int _totalRoll;
        private int _totalNet;

        public string DescriptionFull { get; set; }
        public GameDetails GameDetails { get; }
        public event PropertyChangedEventHandler PropertyChanged;
		public string Name { get; set; }
        public ObservableCollection<EntityAttribute> PlayerAttributes { get; } = new ObservableCollection<EntityAttribute>();
        public ObservableCollection<Skill> PlayerSkills { get; } = new ObservableCollection<Skill>();
        public int TotalMod
        {
            get => _totalMod;
            set
            {
                _totalMod = value;
            }
        }
        public int TotalRoll
        {
            get => _totalRoll;
            set
            {
                _totalRoll = value;
            }
        }
        public int TotalNet 
        { 
            get => _totalNet;
            set
            {
                _totalNet = value;
            }
        }

        public bool HasArchetypeAttributeModifiers =>
            GameDetails.Archetypes.Any(r => r.AttributeModifiers.Any());

        public bool PinBodyTypeChoice { get; set; }
        public bool PinMindTypeChoice { get; set; }
        public bool PinName { get; set; }
        public bool PinPersonaTypeChoice { get; set; }
        public bool PinRace { get; set; }
        public bool PinSex { get; set; }
		public bool PinSpiritTypeChoice { get; set; }
        public Archetype SelectedBodyType { get; set; }
        public Archetype SelectedMindType { get; set; }
		public Archetype SelectedPersonaType { get; set; }
        public Archetype SelectedRace { get; set; }
        public Archetype SelectedSex { get; set; }
		public Archetype SelectedSpiritType { get; set; }

		public CharacterCreationViewModel()
        {
            GameDetails = GameDetailsService.ReadGameDetails();
            RollNewCharacter();
        }

		public void RollNewCharacter()
        {
            if (!PinName)
                Name = RollRandomName();

            if (!PinSex)
                SelectedSex = RandomSex();

            if (!PinBodyTypeChoice)
                SelectedBodyType = RandomBodyType();

            if (!PinMindTypeChoice)
                SelectedMindType = RandomMindType();

            if (!PinPersonaTypeChoice)
                SelectedPersonaType = RandomPersonaType();

            if (!PinRace)
                SelectedRace = RandomRace();

            if (!PinSpiritTypeChoice)
                SelectedSpiritType = RandomSpiritType();

            PlayerAttributes.Clear();
            PlayerSkills.Clear();

            foreach (EntityAttribute playerAttribute in GameDetails.Attributes)
            {
                playerAttribute.ReRoll();
                PlayerAttributes.Add(playerAttribute);
            }
            ApplyAttributeModifiers();

            foreach (Skill skill in GameDetails.Skills)
            {
                skill.ReRoll();
                PlayerSkills.Add(skill);
            }
            ApplySkillModifiers();

            RetotalAggregateMeasures();
            GenerateFullDescription();
        }

		private void GenerateFullDescription()
		{
		}

		public string RollRandomName()
		{
            return "John Doe";
		}

        public void RetotalAggregateMeasures()
		{
            TotalRoll = GameDetails.Attributes.Sum(a => a.BaseValue);
            TotalMod = GameDetails.Attributes.Sum(a => a.Modifier);
            TotalNet = TotalRoll + TotalMod;
        }

        public void ApplyAttributeModifiers()
        {
            foreach (EntityAttribute playerAttribute in PlayerAttributes)
            {
                playerAttribute.Modifier = 0;

                AttributeModifier raceModifier =
                    SelectedRace.AttributeModifiers.FirstOrDefault(am => am.ID.Equals(playerAttribute.ID));
                playerAttribute.Modifier = raceModifier?.Modifier ?? 0;

                AttributeModifier sexModifier =
                    SelectedSex.AttributeModifiers.FirstOrDefault(am => am.ID.Equals(playerAttribute.ID));
                playerAttribute.Modifier += sexModifier?.Modifier ?? 0;

                AttributeModifier bodyTypeModifier =
                    SelectedBodyType.AttributeModifiers.FirstOrDefault(am => am.ID.Equals(playerAttribute.ID));
                playerAttribute.Modifier += bodyTypeModifier?.Modifier ?? 0;

                AttributeModifier mindTypeModifier =
                    SelectedMindType.AttributeModifiers.FirstOrDefault(am => am.ID.Equals(playerAttribute.ID));
                playerAttribute.Modifier += mindTypeModifier?.Modifier ?? 0;

                AttributeModifier personaTypeModifier =
                    SelectedPersonaType.AttributeModifiers.FirstOrDefault(am => am.ID.Equals(playerAttribute.ID));
                playerAttribute.Modifier += personaTypeModifier?.Modifier ?? 0;

                AttributeModifier spiritTypeModifier =
                    SelectedSpiritType.AttributeModifiers.FirstOrDefault(am => am.ID.Equals(playerAttribute.ID));
                playerAttribute.Modifier += spiritTypeModifier?.Modifier ?? 0;

                // Add Modifiers as a Percentage Bonus
                playerAttribute.ModifiedValue = (int)Math.Clamp(playerAttribute.BaseValue * (1.00f + (playerAttribute.Modifier / 100.00f)), 0, 100);
            }
        }

        public void ApplySkillModifiers()
        {

        }

        public Player GetPlayer()
        {
            Player player = new Player(Name, Name, PlayerAttributes, PlayerSkills);

            player.InventoryAddItem(ItemFactory.CreateItem("BareHands"));
            player.LearnRecipe(RecipeFactory.RecipeByID("ShivCrude_01"));
            player.LearnRecipe(RecipeFactory.RecipeByID("ShivCrude_02"));
            player.InventoryAddItem(ItemFactory.CreateItem("Beer"));

            return player;
        }

        #region Rollers
        public Archetype RandomBodyType() =>
            GameDetails.BodyTypes[new Random().Next(GameDetails.BodyTypes.Count())];
        public Archetype RandomMindType() =>
            GameDetails.MindTypes[new Random().Next(GameDetails.MindTypes.Count())];
        public Archetype RandomPersonaType() =>
            GameDetails.PersonaTypes[new Random().Next(GameDetails.PersonaTypes.Count())];
        public Archetype RandomRace() =>
            GameDetails.Races[new Random().Next(GameDetails.Races.Count())];
        public Archetype RandomSex() =>
            GameDetails.Sexes[new Random().Next(2)];
        public Archetype RandomSpiritType() =>
            GameDetails.SpiritTypes[new Random().Next(GameDetails.SpiritTypes.Count())];
		#endregion
    }
}