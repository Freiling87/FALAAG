using System;
using System.Collections.Generic;
using Engine.Services;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Engine.Models
{
    public abstract class Entity : PhysicalObject, INotifyPropertyChanged
    {
		#region Properties

		private Item _currentConsumable;
        private Item _currentWeapon;
        private Dialogue _introduction;
		private string _id;
		private bool _useActualName = false;

		public int Cash { get; private set; }
		public Item CurrentConsumable
        {
            get => _currentConsumable;
            set
            {
                if (_currentConsumable != null)
                {
                    _currentConsumable.Action.OnActionPerformed -= RaiseActionPerformedEvent;
                }

                _currentConsumable = value;

                if (_currentConsumable != null)
                {
                    _currentConsumable.Action.OnActionPerformed += RaiseActionPerformedEvent;
                }
            }
        }
        public Item CurrentWeapon
        {
            get => _currentWeapon; 
            set
            {
                if (_currentWeapon != null)
                    _currentWeapon.Action.OnActionPerformed -= RaiseActionPerformedEvent;

                _currentWeapon = value;

                if (_currentWeapon != null)
                    _currentWeapon.Action.OnActionPerformed += RaiseActionPerformedEvent;
            }
        }
        public string DescriptionLong { get; set; }
		public int HpCur { get; private set; }
		public int HpMax { get; protected set; }
		public string ID { 
            get => _id; 
            set => _id = value; 
        }
		public Inventory Inventory { get; private set; }
		public int Level { get; protected set; }
		public string Name
        {
            get =>
                _useActualName ?
                    NameActual :
                    NameGeneral;
		}
		public string NameActual { get; private set; }
		public string NameGeneral { get; private set; }
        public bool UseActualName
		{
            get => _useActualName; 
            set
            {
                _useActualName = value;
            }
        }

        public ObservableCollection<EntityAttribute> Attributes { get; } = new ObservableCollection<EntityAttribute>();

        [JsonIgnore]
        public bool IsAlive => HpCur > 0;

        [JsonIgnore]
        public bool IsDead => !IsAlive;

        #endregion

        public event EventHandler<string> OnActionPerformed;
        public event EventHandler OnKilled;

        protected Entity(string ID, string nameActual, string nameGeneral, int hpMax, int hpCur, IEnumerable<EntityAttribute> attributes, int cash, int level = 1)
        {
            Cash = cash;
            HpCur = hpCur;
            HpMax = hpMax;
            Level = level;
            NameActual = nameActual;
            NameGeneral = nameGeneral;

            foreach (EntityAttribute attribute in attributes)
                Attributes.Add(attribute);

            Inventory = new Inventory();
        }

        public void CashReceive(int amountOfGold)
        {
            Cash += amountOfGold;
        }
        public void CashSpend(int amountOfGold)
        {
            if (amountOfGold > Cash)
                throw new ArgumentOutOfRangeException($"{NameGeneral} only has ${Cash}, and cannot spend ${amountOfGold}.");

            Cash -= amountOfGold;
        }
        public void Heal(int hitPointsToHeal)
        {
            HpCur += hitPointsToHeal;

            if (HpCur > HpMax)
            {
                HpCur = HpMax;
            }
        }
        public void HealCompletely()
        {
            HpCur = HpMax;
        }
        // See 15.3 notes about functional programming to understand these.
        public void InventoryAddItem(Item item) =>
            Inventory = Inventory.AddItem(item);
        public void InventoryRemoveItem(Item item) =>
            Inventory = Inventory.RemoveItem(item);
        public void InventoryRemoveItems(List<ItemQuantity> itemQuantities) =>
            Inventory = Inventory.RemoveItems(itemQuantities);
        public void TakeDamage(int hitPointsOfDamage)
        {
            HpCur -= hitPointsOfDamage;

            if (IsDead)
            {
                HpCur = 0;
                RaiseOnKilledEvent();
            }
        }
        // These are Helpers, redundant to deeper methods but intended to keep code clean.
        public void UseCurrentConsumable()
        {
            CurrentConsumable.PerformAction(this, this);
            InventoryRemoveItem(CurrentConsumable);
        }
        public void UseCurrentWeaponOn(Entity target)
        {
            CurrentWeapon.PerformAction(this, target);
        }

        internal static bool Noticed(PhysicalObject physObject)
        {
            return true;
        }
        #region Private functions
        private void RaiseActionPerformedEvent(object sender, string result) =>
            OnActionPerformed?.Invoke(this, result);
        private void RaiseOnKilledEvent() =>
            OnKilled?.Invoke(this, new System.EventArgs());
		#endregion
	}
}