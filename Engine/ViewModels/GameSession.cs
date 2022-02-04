using Engine.Factories;
using Engine.Models;
using Engine.Services;
using Newtonsoft.Json;
using System.Linq;
using System.ComponentModel;

namespace Engine.ViewModels
{
	public class GameSession : INotifyPropertyChanged
    {
        public string Version { get; } = "0.1.000";
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly MessageBroker _messageBroker = MessageBroker.GetInstance();

        #region Header
        private Automat _currentAutomat;
        private Battle _currentBattle;
        private Cell _currentCell;
        private GameDetails _gameDetails;
        private NPC _currentNPC;
        private Player _player;

        [JsonIgnore]
        public Automat CurrentAutomat
        {
            get { return _currentAutomat; }
            set
            {
                _currentAutomat = value;
            }
        }
        public Cell CurrentCell
        {
			get { return _currentCell; }
            set
            {
                _currentCell = value;
                CurrentNPC = _currentCell.GetNPC();
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
        public GameDetails GameDetails
        {
            get => _gameDetails;
            set
            {
                _gameDetails = value;
            }
        }
        [JsonIgnore]
        public bool HasAutomat => CurrentAutomat != null;
        [JsonIgnore]
        public bool HasNPC => CurrentNPC != null;

        public GameSession(Player player, int x, int y, int z)
        {
            PopulateGameDetails();
            CurrentWorld = WorldFactory.CreateWorld();
            Player = player;
            CurrentCell = CurrentWorld.LocationAt(x, y, z);
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
                    _messageBroker.RaiseMessage($"  {itemQuantity.Quantity} {ItemFactory.ItemName(itemQuantity.ID)}");
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
        public void MoveToCell(Cell cell)
		{
            CurrentCell = CurrentWorld.LocationAt(cell.X, cell.Y, cell.Z);
            //ClearNarrationPanel();
            CurrentCell.NarrateEntry();

            foreach (NPC npc in CurrentCell.NPCs.Where(npc => Player.Noticed(npc)))
                npc.NarrateEntry();

            foreach (Automat automat in CurrentCell.Automats.Where(Automat => Player.Noticed(Automat)))
                automat.NarrateEntry();

            foreach (Gate gate in CurrentCell.Gates.Where(gate => Player.Noticed(gate)))
                gate.NarrateEntry();

            foreach (Feature feature in CurrentCell.PhysicalFeatures.Where(feature => Player.Noticed(feature)))
                feature.NarrateEntry();

            foreach (Item item in CurrentCell.Items.Where(Item => Player.Noticed(Item)))
                item.NarrateEntry();
        }
        public void MoveNorth()
        {
            if (HasCellN)
                CurrentCell = CurrentWorld.LocationAt(CurrentCell.X, CurrentCell.Y + 1, CurrentCell.Z);
        }
        public void MoveEast()
        {
            if (HasCellE)
                CurrentCell = CurrentWorld.LocationAt(CurrentCell.X + 1, CurrentCell.Y, CurrentCell.Z);
        }
        public void MoveSouth()
        {
            if (HasCellS)
                CurrentCell = CurrentWorld.LocationAt(CurrentCell.X, CurrentCell.Y - 1, CurrentCell.Z);
        }
        public void MoveWest()
        {
            if (HasCellW)
                CurrentCell = CurrentWorld.LocationAt(CurrentCell.X - 1, CurrentCell.Y, CurrentCell.Z);
        }
        public void Ascend()
		{
            if (HasCellA)
                CurrentCell = CurrentWorld.LocationAt(CurrentCell.X, CurrentCell.Y, CurrentCell.Z + 1);
        }
        public void Descend()
        {
            if (HasCellB)
                CurrentCell = CurrentWorld.LocationAt(CurrentCell.X, CurrentCell.Y, CurrentCell.Z - 1);
        }
        [JsonIgnore]
        public bool HasCellN => 
            CurrentWorld.LocationAt(CurrentCell.X, CurrentCell.Y + 1, CurrentCell.Z) != null;
        [JsonIgnore]
        public bool HasCellE =>
            CurrentWorld.LocationAt(CurrentCell.X + 1, CurrentCell.Y, CurrentCell.Z) != null;
        [JsonIgnore]
        public bool HasCellS =>
            CurrentWorld.LocationAt(CurrentCell.X, CurrentCell.Y - 1, CurrentCell.Z) != null;
        [JsonIgnore]
        public bool HasCellW =>
            CurrentWorld.LocationAt(CurrentCell.X - 1, CurrentCell.Y, CurrentCell.Z) != null;
        [JsonIgnore]
        public bool HasCellA =>
            CurrentWorld.LocationAt(CurrentCell.X, CurrentCell.Y, CurrentCell.Z + 1) != null;
        [JsonIgnore]
        public bool HasCellB =>
            CurrentWorld.LocationAt(CurrentCell.X, CurrentCell.Y, CurrentCell.Z - 1) != null;
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
            CurrentNPC = CurrentCell.GetNPC();
        private void OnPlayerKilled(object sender, System.EventArgs eventArgs)
        {
            _messageBroker.RaiseMessage("");
            _messageBroker.RaiseMessage($"You got knocked the fuck out!");

            CurrentCell = CurrentWorld.LocationAt(0, 0, 0);
            Player.HealCompletely();
        }
        private void OnPlayerLeveledUp(object sender, System.EventArgs eventArgs)
            => _messageBroker.RaiseMessage($"You are now level {Player.Level}!");
        #endregion
    }
}