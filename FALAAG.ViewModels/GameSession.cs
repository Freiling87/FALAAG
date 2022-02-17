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
        public PopupDetails InventoryDetails { get; set; }
        public PopupDetails JobDetails { get; set; }
        public PopupDetails PlayerDetails { get; set; }
        public PopupDetails RecipesDetails { get; set; }
        public PopupDetails ActionOptionsDetails { get; set; }
        public PhysicalObject CurrentInteraction { get; set; }
        public List<ActionOption> CurrentActionOptions = new List<ActionOption>();
        public ActionOption SelectedAction { get; set; } // For displaying Outcome prediction in Action Option selection window.

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
            ActionOptionsDetails = new PopupDetails
            {
                IsVisible = false,
                Top = 500,
                Left = 575,
                MinHeight = 75,
                MaxHeight = 800,
                MinWidth = 250,
                MaxWidth = 800
            };
        }

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
        public void AttemptAction(Entity entity, ActionOption actionOption)
		{

		}
        #endregion
        #region Message Log
        #endregion
        #region Movement
        public bool HasCellA
        {
            get
            {
                Cell cell = CurrentWorld.GetNeighbor(CurrentCell, Direction.Above);

                if (cell == null || cell.IsMapEdge)
                    return false;

                return true;
            }
        }
        public bool HasCellB
        {
            get
            {
                Cell cell = CurrentWorld.GetNeighbor(CurrentCell, Direction.Below);

                if (cell == null || cell.IsMapEdge)
                    return false;

                return true;
            }
        }
        public bool HasCellE
        {
            get
            {
                Cell cell = CurrentWorld.GetNeighbor(CurrentCell, Direction.East);

                if (cell == null || cell.IsMapEdge)
                    return false;

                return true;
            }
        }
        public bool HasCellN
        {
            get
            {
                Cell cell = CurrentWorld.GetNeighbor(CurrentCell, Direction.North);

                if (cell == null || cell.IsMapEdge)
                    return false;

                return true;
            }
        }
        public bool HasCellS
        {
            get
            {
                Cell cell = CurrentWorld.GetNeighbor(CurrentCell, Direction.South);

                if (cell == null || cell.IsMapEdge)
                    return false;

                return true;
            }
        }
        public bool HasCellW
        {
            get
            {
                Cell cell = CurrentWorld.GetNeighbor(CurrentCell, Direction.West);

                if (cell == null || cell.IsMapEdge)
                    return false;

                return true;
            }
        }
        public void MoveDirection(Direction direction)
		{
            if (HasCell(direction))
                MoveToCell(CurrentWorld.GetNeighbor(CurrentCell, direction));
		}
        public bool HasCell(Direction direction) =>
            CurrentWorld.GetNeighbor(CurrentCell, direction) != null;
        public void MoveToCell(Cell cell)
        {
            bool passed = true;
            Direction path = GetDirectionFromCurrentCell(cell);
            Wall wall = cell.GetWall(path);

            // TODO: Check for Status Effects preventing movement
            // TODO: Enemy blocking access to wall/portal

            if (wall != null && !wall.Passable)
            {
                CurrentInteraction = wall;
                CurrentActionOptions.Concat(wall.ActionOptions);

                if (wall.Portals != null)
                    foreach (Portal portal in wall.Portals)
                    {
                        CurrentActionOptions = CurrentActionOptions.Concat(portal.ActionOptions).ToList();
                    }
                else
                // KoolAidMan shit here
                {
                    MessageBroker.GetInstance().RaiseMessage("You can't walk through walls... yet.");
                    return;
                }

                if (CurrentActionOptions.Count() > 0)
                    ActionOptionsDetails.IsVisible = true;
            }

            // TODO: Set following code to be contingent on passing check in ActionOptionsDetails

            CurrentCell = cell;
            Narrator.OnMovement(CurrentCell);
        }
        public void MoveToCell(int x, int y, int z) =>
            MoveToCell(CurrentWorld.GetCell(x, y, z));
        public void PassedMovementChecks(Cell cell, Entity entity)
		{
            
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