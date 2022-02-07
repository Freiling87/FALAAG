using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FALAAG.Actions;
using Newtonsoft.Json;

namespace FALAAG.Models
{
	public class Item : PhysicalObject
	{
		public enum ItemCategory
		{
			Consumable,
			MacGuffin,
			Miscellaneous,
			Weapon,
			Wearable,
		}

		[JsonIgnore]
		public IAction Action { get; set; }
		[JsonIgnore]
		public ItemCategory Category { get; }
		[JsonIgnore]
		public bool unstackable { get; }
		public string ID { get; }
		[JsonIgnore]
		public string Name { get; }
		[JsonIgnore]
		public int Mass { get; }
		[JsonIgnore]
		public int Value { get; }
		[JsonIgnore]
		public int Weight { get; }
		[JsonIgnore]
		public int SellPriceSimple =>
			(int)(Value * 0.7f);

		public Item(ItemCategory category, string itemTypeID, string name, int price,
						bool isUnique = false, IAction action = null)
		{
			Category = category;
			ID = itemTypeID;
			Name = name;
			Value = price;
			unstackable = isUnique;
			Action = action;
		}

		public void PerformAction(Entity actor, Entity target) =>
			Action?.Execute(actor, target);

		public Item Clone() =>
			new Item(Category, ID, Name, Value, unstackable, Action);
	}
}
