using System;
using System.Collections.ObjectModel;
using System.Linq;
using Engine.Factories;
using Engine.Models;
using Engine.Services;
using System.ComponentModel;

namespace FALAAG.ViewModels
{
    public class CharacterCreationViewModel : INotifyPropertyChanged
    {
		private int _totalMod;
        private int _totalRoll;
        private int _totalNet;
        private bool _pinName;

        public string DescriptionFull { get; set; }
        public GameDetails GameDetails { get; }
        public event PropertyChangedEventHandler PropertyChanged;
		public string Name { get; set; }
        public ObservableCollection<EntityAttribute> PlayerAttributes { get; } = new ObservableCollection<EntityAttribute>();
        public bool PinName { get => _pinName; set => _pinName = value; }
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
		public bool PinPersonaTypeChoice { get; set; }
		public bool PinSpiritTypeChoice { get; set; }
        public Archetype SelectedBodyType { get; set; }
        public Archetype SelectedMindType { get; set; }
		public Archetype SelectedPersonaType { get; set; }
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

            if (!PinBodyTypeChoice)
                SelectRandomBodyType();

            if (!PinMindTypeChoice)
                SelectRandomMindType();

            if (!PinPersonaTypeChoice)
                SelectRandomPersonaType();

            if (!PinSpiritTypeChoice)
                SelectRandomSpiritType();

            PlayerAttributes.Clear();

            foreach (EntityAttribute playerAttribute in GameDetails.Attributes)
            {
                playerAttribute.ReRoll();
                PlayerAttributes.Add(playerAttribute);
            }

            ApplyAttributeModifiers();
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

        public void SelectRandomBodyType()
		{
            var random = new Random();
            int index = random.Next(GameDetails.BodyTypes.Count);
            SelectedBodyType = GameDetails.BodyTypes[index];
        }
        public void SelectRandomMindType()
        {
            var random = new Random();
            int index = random.Next(GameDetails.MindTypes.Count);
            SelectedMindType = GameDetails.MindTypes[index];
        }
        public void SelectRandomPersonaType()
        {
            var random = new Random();
            int index = random.Next(GameDetails.PersonaTypes.Count);
            SelectedPersonaType = GameDetails.PersonaTypes[index];
        }
        public void SelectRandomSpiritType()
        {
            var random = new Random();
            int index = random.Next(GameDetails.SpiritTypes.Count);
            SelectedSpiritType = GameDetails.SpiritTypes[index];
        }

        public void ApplyAttributeModifiers()
        {
            // TODO: This is where we need to combine all 4 archetypes' attribute modifiers into the Mod Column

            foreach (EntityAttribute playerAttribute in PlayerAttributes)
            {
                playerAttribute.Modifier = 0;

                AttributeModifier bodyTypeModifier = 
                    SelectedBodyType.AttributeModifiers.FirstOrDefault(am => am.Key.Equals(playerAttribute.Key));

                playerAttribute.Modifier = bodyTypeModifier?.Modifier ?? 0;

                AttributeModifier mindTypeModifier =
                    SelectedMindType.AttributeModifiers.FirstOrDefault(am => am.Key.Equals(playerAttribute.Key));

                playerAttribute.Modifier += mindTypeModifier?.Modifier ?? 0;

                AttributeModifier personaTypeModifier =
                    SelectedPersonaType.AttributeModifiers.FirstOrDefault(am => am.Key.Equals(playerAttribute.Key));

                playerAttribute.Modifier += personaTypeModifier?.Modifier ?? 0;

                AttributeModifier spiritTypeModifier =
                    SelectedSpiritType.AttributeModifiers.FirstOrDefault(am => am.Key.Equals(playerAttribute.Key));

                playerAttribute.Modifier += spiritTypeModifier?.Modifier ?? 0;

                playerAttribute.ModifiedValue =
                    playerAttribute.BaseValue + playerAttribute.Modifier;
            }
        }

        public Player GetPlayer()
        {
            Player player = new Player(Name, "NameGeneral_Placeholder", 0, 10, 10, PlayerAttributes, 10);

            // Give player default inventory items, weapons, recipes, etc.
            player.InventoryAddItem(ItemFactory.CreateItem("BareHands"));
            player.LearnRecipe(RecipeFactory.RecipeByID("ShivCrude_01"));
            player.LearnRecipe(RecipeFactory.RecipeByID("ShivCrude_02"));

            return player;
        }
    }
}