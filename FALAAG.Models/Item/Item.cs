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
		public IItemAction Action { get; set; }
		[JsonIgnore]
		public ItemCategory Category { get; }
		[JsonIgnore]
		public bool unstackable { get; }
		[JsonIgnore]
		public string Description { get; }
		public string ID { get; }
		[JsonIgnore]
		public int Mass { get; }
		public Material Material { get; }
		public int Size { get; }
		[JsonIgnore]
		public int Value { get; }
		[JsonIgnore]
		public int Weight { get; }
		[JsonIgnore]
		public int SellPriceSimple =>
			(int)(Value * 0.7f);
		public Feature StoringFeature { get; set; }

		public Item(ItemCategory category, string itemTypeID, string name, string description, int price,
						bool isUnique = false, IItemAction action = null)
		{
			Category = category;
			Description = description;
			ID = itemTypeID;
			Name = name;
			Value = price;
			unstackable = isUnique;
			Action = action;
		}

		public void PerformAction(Entity actor, Entity target) =>
			Action?.Execute(actor, target);

		public Item Clone() =>
			new Item(Category, ID, Name, Description, Value, unstackable, Action);
	}
}
