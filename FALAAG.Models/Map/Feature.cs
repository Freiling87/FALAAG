using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FALAAG.Models
{
	public class Feature : PhysicalObject
	{
		public string NarrationEntry { get; internal set; }
		public List<Item> StoredItems = new List<Item>();

		public void AddItem(Item item)
		{
			StoredItems.Add(item);
			item.StoringFeature = this;
		}
	}
}
