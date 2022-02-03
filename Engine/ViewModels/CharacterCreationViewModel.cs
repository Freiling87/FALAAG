using System;
using System.Collections.ObjectModel;
using System.Linq;
using Engine.Factories;
using Engine.Models;
using Engine.Services;

namespace Engine.ViewModels
{
    public class CharacterCreationViewModel : BaseNotificationClass
    {
        private Archetype _selectedArchetype;
        private int _totalMod;
        private int _totalRoll;
        private int _totalNet;

        public GameDetails GameDetails { get; }
        public Archetype SelectedArchetype
        {
            get => _selectedArchetype;
            set
            {
                _selectedArchetype = value;
                OnPropertyChanged();
            }
        }
        public string Name { get; set; }
        public ObservableCollection<EntityAttribute> PlayerAttributes { get; set; } = new ObservableCollection<EntityAttribute>();
        public int TotalMod
        {
            get => _totalMod;
            set
            {
                _totalMod = value;
                OnPropertyChanged();
            }
        }
        public int TotalRoll
        {
            get => _totalRoll;
            set
            {
                _totalRoll = value;
                OnPropertyChanged();
            }
        }
        public int TotalNet 
        { 
            get => _totalNet;
            set
            {
                _totalNet = value;
                OnPropertyChanged();
            }
        }

        public bool HasArchetypes =>
            GameDetails.Archetypes.Any();

        public bool HasArchetypeAttributeModifiers =>
            HasArchetypes && 
            GameDetails.Archetypes.Any(r => r.AttributeModifiers.Any());

        public CharacterCreationViewModel()
        {
            GameDetails = GameDetailsService.ReadGameDetails();

            if (HasArchetypes)
                SelectRandomArchetype();

            RollNewCharacter();
        }

        public void RollNewCharacter()
        {
            SelectRandomArchetype();

            PlayerAttributes.Clear();

            foreach (EntityAttribute playerAttribute in GameDetails.Attributes)
            {
                playerAttribute.ReRoll();
                PlayerAttributes.Add(playerAttribute);
            }

            RetotalAggregateMeasures();
            ApplyAttributeModifiers();
        }
        public void RetotalAggregateMeasures()
		{
            TotalRoll = GameDetails.Attributes.Sum(a => a.BaseValue);
            TotalMod = GameDetails.Attributes.Sum(a => a.Modifier);
            TotalNet = TotalRoll + TotalMod;
        }
        public void SelectRandomArchetype()
		{
            var random = new Random();
            int index = random.Next(GameDetails.Archetypes.Count);
            SelectedArchetype = GameDetails.Archetypes[index];
		}

        public void ApplyAttributeModifiers()
        {
            foreach (EntityAttribute playerAttribute in PlayerAttributes)
            {
                var attributeArchetypeModifier = 
                    SelectedArchetype.AttributeModifiers.FirstOrDefault(pam => pam.Key.Equals(playerAttribute.Key));

                // For display in creator screen, may not be the best way
                playerAttribute.Modifier = attributeArchetypeModifier?.Modifier ?? 0;

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