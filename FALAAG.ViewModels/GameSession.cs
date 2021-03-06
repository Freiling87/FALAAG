using FALAAG.Factories;
using FALAAG.Models;
using FALAAG.Services;
using Newtonsoft.Json;
using System.Linq;
using System.ComponentModel;
using FALAAG.Core;
using System;
using System.Collections.Generic;

namespace FALAAG.ViewModels
{
    public class GameSession : INotifyPropertyChanged
    {
        public string Version { get; } = "0.1.000";
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly MessageBroker _messageBroker = MessageBroker.GetInstance();

		#region Header
		private Battle _currentBattle;
        private Cell _currentCell;
		private NPC _currentNPC;
        private Player _player;

		[JsonIgnore]
		public Automat CurrentAutomat { get; private set; }
		public Cell CurrentCell
        {
			get { return _currentCell; }
            set
            {
                _currentCell = value;
                CurrentNPC = NPCFactory.GetNPCFromCellEncounters(CurrentCell);
                CompleteJobsAtLocation();
                GivePlayerJobsAtLocation();
                CurrentAutomat = CurrentCell.AutomatHere;
            }
        }

		public void ActionResult(ActionCommand ac)
		{
			switch (ac.SkillType)
			{
                case SkillType.Moving:
                    MoveDirection(ac.MoveDirection);
                    return;
                default:
                    throw new NotImplementedException();
			}
		}

		[JsonIgnore]
        public NPC CurrentNPC
        {
            get { return _currentNPC; }
            set
            {
                if (_currentBattle != null)
                {
                    _currentBattle.OnCombatVictory -= OnCurrentNPCKilled;
                    _currentBattle.Dispose();
                }

                _currentNPC = value;

                if (_currentNPC != null)
                {
                    _currentBattle = new Battle(Player, CurrentNPC);
                    _currentBattle.OnCombatVictory += OnCurrentNPCKilled;
                }
            }
        }
        public Player Player
        {
            get { return _player; }
            set
            {
                // Helps clear memory, removing subscriptions to vestigial objects
                if (_player != null)
				{
                    _player.OnLeveledUp -= OnPlayerLeveledUp;
                    _player.OnKilled -= OnPlayerKilled;
                }

                _player = value;

                if (_player != null)
                {
                    _player.OnLeveledUp += OnPlayerLeveledUp;
                    _player.OnKilled += OnPlayerKilled;
                }
            }
        }
        [JsonIgnore]
        public World CurrentWorld { get; }
		[JsonIgnore]
		public GameDetails GameDetails { get; private set; }
		[JsonIgnore]
        public bool HasAutomat => CurrentAutomat != null;
        [JsonIgnore]
        public bool HasNPC => CurrentNPC != null;
        public bool MovementActionScreenModal { get; set; }
        public PopupDetails InventoryDetails { get; set; }
        public PopupDetails JobDetails { get; set; }
        public PopupDetails PlayerDetails { get; set; }
        public PopupDetails RecipesDetails { get; set; }
        public PhysicalObject CurrentInteraction { get; set; } // TODO: Change this to an ActionCommand, which can store a physicalobject as a target anyway; Or eliminate it entirely, once the ActionCommandManager is implemented and you can just traverse the queue for this.
        public List<ActionCommand> CurrentActionCommands { get; set; } = new();
        public ActionCommand SelectedAction { get; set; } // For displaying Outcome prediction in Action Option selection window.
        public bool ActionCommandSelected => SelectedAction != null;
        private ActionRate _actionRate;
        public ActionRate ActionRate
		{
            get { return _actionRate; }
			set
			{
                _actionRate = value;
                Player.ActionRate = value;
			}
		}

        public GameSession(Player player, int x, int y, int z)
        {
            PopulateGameDetails();
            CurrentWorld = WorldFactory.CreateWorld();
            Player = player;
            CurrentCell = CurrentWorld.GetCell(x, y, z);

            InventoryDetails = new PopupDetails
            {
                IsVisible = false,
                Top = 500,
                Left = 10,
                MinHeight = 75,
                MaxHeight = 175,
                MinWidth = 250,
                MaxWidth = 400
            }; 
            JobDetails = new PopupDetails
            {
                IsVisible = false,
                Top = 500,
                Left = 275,
                MinHeight = 75,
                MaxHeight = 175,
                MinWidth = 250,
                MaxWidth = 400
            }; 
            PlayerDetails = new PopupDetails
            {
                IsVisible = false,
                Top = 10,
                Left = 10,
                MinHeight = 75,
                MaxHeight = 400,
                MinWidth = 265,
                MaxWidth = 400
            };
            RecipesDetails = new PopupDetails
            {
                IsVisible = false,
                Top = 500,
                Left = 575,
                MinHeight = 75,
                MaxHeight = 175,
                MinWidth = 250,
                MaxWidth = 400
            };
        }

        #endregion
        #region XAML-bound properties
        public bool RateButtonSlowestIsEnabled { get => ActionRate != ActionRate.Slowest; }
        public bool RateButtonSlowIsEnabled { get => ActionRate != ActionRate.Slow; }
        public bool RateButtonMediumIsEnabled { get => ActionRate != ActionRate.Medium; }
        public bool RateButtonFastIsEnabled { get => ActionRate != ActionRate.Fast; }
        public bool RateButtonFastestIsEnabled { get => ActionRate != ActionRate.Fastest; }
        public bool HasCellA { get => HasNonEdgeNeighbor(Direction.Above); }
        public bool HasCellB { get => HasNonEdgeNeighbor(Direction.Below); }
        public bool HasCellE { get => HasNonEdgeNeighbor(Direction.East); }
        public bool HasCellN { get => HasNonEdgeNeighbor(Direction.North); }
        public bool HasCellS { get => HasNonEdgeNeighbor(Direction.South); }
        public bool HasCellW { get => HasNonEdgeNeighbor(Direction.West); }
        #endregion
        private void PopulateGameDetails()
        {
            GameDetails = GameDetailsService.ReadGameDetails();
        }
        #region Actions
        public void AttackCurrentNPC() =>
            _currentBattle?.AttackOpponent();
        public void CraftItemUsing(Recipe recipe)
        {
            if (Player.Inventory.HasAllTheseItems(recipe.Ingredients))
            {
                Player.InventoryRemoveItems(recipe.Ingredients);

                foreach (ItemQuantity itemQuantity in recipe.OutputItems)
                    for (int i = 0; i < itemQuantity.Quantity; i++)
                    {
                        Item outputItem = ItemFactory.CreateItem(itemQuantity.ID);
                        Player.InventoryAddItem(outputItem);
                        _messageBroker.RaiseMessage($"You craft 1 {outputItem.Name}");
                    }
            }
            else
            {
                _messageBroker.RaiseMessage("You lack one or more required ingredients:");

                foreach (ItemQuantity itemQuantity in recipe.Ingredients)
                    _messageBroker.RaiseMessage($"  {itemQuantity.QuantityItemDescription}");
            }
        }
        public void UseCurrentConsumable()
        {
            if (Player.CurrentConsumable != null)
            {
                if (_currentBattle == null)
                    Player.OnActionPerformed += OnConsumableActionPerformed;

                Player.UseCurrentConsumable();

                if (_currentBattle == null)
                    Player.OnActionPerformed -= OnConsumableActionPerformed;
            }
        }
		#endregion
		#region Message Log
		#endregion
		#region Movement
		public bool HasNonEdgeNeighbor(Direction direction)
		{
            Cell neighbor = CurrentWorld.GetNeighbor(CurrentCell, direction);

            return 
                neighbor != null &&
                !neighbor.IsMapEdge;
        }
        public void MoveDirection(Direction direction)
		{
            CompleteMovement(CurrentWorld.GetNeighbor(CurrentCell, direction));
		}
        public void MoveToCell(int x, int y, int z) => 
            CompleteMovement(CurrentWorld.GetCell(x, y, z));
        public void CompleteMovement(Cell destination)
        {
            CurrentCell = destination;
            Narrator.OnMovement(CurrentCell);
            // TODO: Check for Status Effects preventing movement
            // TODO: Enemy blocking access to wall/portal
            // TODO: Set following code to be contingent on passing check in ActionCommandsDetails
        }
        private Direction GetDirectionFromCurrentCell(Cell targetCell)
		{
			foreach (Direction direction in Enum.GetValues(typeof(Direction)))
			{
                if (CurrentWorld.GetNeighbor(CurrentCell, direction) == targetCell)
                    return direction;
			}

            return Direction.Null;
		}
		#endregion
		#region NPCs
		#endregion
		#region Jobs
		private void CompleteJobsAtLocation()
        {
            foreach (Job job in CurrentCell.JobsHere)
            {
                JobStatus jobToComplete = Player.JobsActive.FirstOrDefault(q => q.Job.ID == job.ID && !q.IsCompleted);

                if (jobToComplete != null && 
                    Player.Inventory.HasAllTheseItems(job.ItemsRequired))
                {

                    _messageBroker.RaiseMessage("");
                    _messageBroker.RaiseMessage($"You completed the \"{job.Name}\" job.");
                    _messageBroker.RaiseMessage($"You receive {job.XpReward} XP.");
                    _messageBroker.RaiseMessage($"You receive ${job.CashReward}.");
                    Player.InventoryRemoveItems(job.ItemsRequired);
                    Player.CashReceive(job.CashReward);
                    Player.AddExperience(job.XpReward);

                    foreach (ItemQuantity itemQuantity in job.ItemsReward)
                    {
                        Item rewardItem = ItemFactory.CreateItem(itemQuantity.ID);
                        _messageBroker.RaiseMessage($"You receive a {rewardItem.Name}.");
                        Player.InventoryAddItem(rewardItem);
                    }

                    jobToComplete.IsCompleted = true;
                }
            }
        }
        private void GivePlayerJobsAtLocation()
        {
            foreach (Job job in CurrentCell.JobsHere)
            {
                if (!Player.JobsActive.Any(q => q.Job.ID == job.ID))
                {
                    Player.JobsActive.Add(new JobStatus(job));

                    _messageBroker.RaiseMessage("");
                    _messageBroker.RaiseMessage($"{CurrentNPC.Name} has offered you the '{job.Name}' job.");
                    _messageBroker.RaiseMessage($"\"{job.Description}\"");

                    _messageBroker.RaiseMessage("Return with:");
                    foreach (ItemQuantity itemQuantity in job.ItemsRequired)
                        _messageBroker.RaiseMessage($"   {itemQuantity.Quantity} {ItemFactory.CreateItem(itemQuantity.ID).Name}");

                    _messageBroker.RaiseMessage("He's offering:");
                    _messageBroker.RaiseMessage($"   {job.XpReward} XP");
                    _messageBroker.RaiseMessage($"   ${job.CashReward}");
                    foreach (ItemQuantity itemQuantity in job.ItemsReward)
                        _messageBroker.RaiseMessage($"   {itemQuantity.Quantity} {ItemFactory.CreateItem(itemQuantity.ID).Name}");
                }
            }
        }
        #endregion
        #region Events
        private void OnConsumableActionPerformed(object sender, string result) =>
            _messageBroker.RaiseMessage(result);
        private void OnCurrentNPCKilled(object sender, System.EventArgs eventArgs) =>
            CurrentNPC = NPCFactory.GetNPCFromCellEncounters(CurrentCell);
        private void OnPlayerKilled(object sender, System.EventArgs eventArgs)
        {
            _messageBroker.RaiseMessage("");
            _messageBroker.RaiseMessage($"You got knocked the fuck out!");

            CurrentCell = CurrentWorld.GetCell(0, 0, 0);
            Player.HealCompletely();
        }
        private void OnPlayerLeveledUp(object sender, System.EventArgs eventArgs)
            => _messageBroker.RaiseMessage($"You are now level {Player.Level}!");
        #endregion
    }
}