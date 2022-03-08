using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using FALAAG.Core;
using FALAAG.Models.Shared;

namespace FALAAG.Models
{
    public enum ActionRate
    {
        Null, // For storing in PhysObjects and redefining when cloned to an Actor
        Slowest, // Stealthy
        Slow, // Careful
        Medium, // Casual
        Fast, // Hurried
        Fastest // Frantic
    }

    public abstract class Entity : PhysicalObject, INotifyPropertyChanged
    {
		#region Properties

		private Item _currentConsumable;
        private Item _currentWeapon;
        private Dialogue _introduction;
		private string _id;
		private bool _useActualName = false;
        public ActionRate ActionRate { get; set; }

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
        [JsonIgnore]
        public string Hp => $"{HpCur}/{HpMax}";
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
        public Role CurrentRole { get; set; } // Proprietor, Bodyguard, etc.
        public bool UseActualName
		{
            get => _useActualName; 
            set
            {
                _useActualName = value;
            }
        }

        public int Arousal { get; private set; } // Master measure, attributed by inclinations below
        public int Fatigue { get; private set; }
        public int Fear { get; private set; }
        public int Pain { get; private set; }

        public ObservableCollection<EntityAttribute> Attributes { get; } = new ObservableCollection<EntityAttribute>();
        public ObservableCollection<Skill> Skills { get; } = new ObservableCollection<Skill>();

        [JsonIgnore]
        public bool IsAlive => HpCur > 0;

        [JsonIgnore]
        public bool IsDead => !IsAlive;

        #endregion

        public event EventHandler<string> OnActionPerformed;
        public event EventHandler OnKilled;

        protected Entity(string ID, string nameActual, string nameGeneral, IEnumerable<EntityAttribute> attributes, IEnumerable<Skill> skills)
        {
            Cash = 0;
            HpCur = 100;
            HpMax = 100;
            Level = 1;
            NameActual = nameActual;
            NameGeneral = nameGeneral;
            ActionRate = ActionRate.Medium;

            foreach (EntityAttribute attribute in attributes)
                Attributes.Add(attribute);

            foreach (Skill skill in skills)
                Skills.Add(skill);

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
        public int D100() =>
            DiceService.Instance.Roll(100).Value;
        public Skill GetSkillByID(SkillType skillType) =>
            Skills.FirstOrDefault(s => s.ID == skillType.ToString());
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

        public static int Noticed(PhysicalObject physObject)
        {
            // This is an Int so that noticed things can be listed in order of pertinence.
            return 100;
        }
        #region Private functions
        private void RaiseActionPerformedEvent(object sender, string result) =>
            OnActionPerformed?.Invoke(this, result);
        private void RaiseOnKilledEvent() =>
            OnKilled?.Invoke(this, new System.EventArgs());

        public bool SucceedsAt(ActionCommand actionCommand)
        {
            Skill skill = GetSkillByID(actionCommand.SkillType);
            int successTotal = 0;

            MessageBroker.GetInstance().RaiseMessage("Attempt: " + actionCommand.Name + " on " + actionCommand.Target.Name);
            MessageBroker.GetInstance().RaiseMessage("\t  ATTRIBUTE\t\t     ( %ROLL * ATTR * WEIGHT  =  RESULT");
            foreach (AttributeComponent ac in skill.AttributeComponents)
			{
                int result = RollComponent(ac);
                successTotal += (int)(result);
            }

            MessageBroker.GetInstance().RaiseMessage("TOTAL:\t" + successTotal.ToString().PadLeft(3, ' ') + " / " + actionCommand.Difficulty.ToString().PadLeft(3, ' '));

            float successRatio = successTotal / actionCommand.Difficulty;
            actionCommand.Execute(successRatio);

            if (successRatio >= 1)
                return true;

            return false;
        }
        public int RollComponent(AttributeComponent ac)
		{
            int attribute = this.GetAttribute(ac.AttributeKey).ModifiedValue;
            float roll = DiceService.Instance.Roll(100).Value / 100f;
            int result = (int)MathF.Round(attribute * roll * ac.Percent, 0, MidpointRounding.AwayFromZero);

			MessageBroker.GetInstance().RaiseMessage(
                "\t" + this.GetAttribute(ac.AttributeKey).DisplayName.PadRight(24, ' ') + 
                "\t" + roll.ToString().PadLeft(3, ' ') + 
                "\t" + attribute.ToString().PadLeft(3, ' ') +
                "\t" + ac.Percent.ToString().PadRight(4, '0').PadLeft(6, ' ') +
                "\t\t" + result.ToString().PadLeft(3, ' '));

            return result;
		}
        #endregion

        public ActionCommand MovementAction(Direction direction)
        {
            ActionCommand ac = new ActionCommand(SkillType.Moving.ToString(), SkillType.Moving.ToString(), SkillType.Moving, this, this, ActionRate)
            {
                MoveDirection = direction
            };

            return ac;
        }

	}
}