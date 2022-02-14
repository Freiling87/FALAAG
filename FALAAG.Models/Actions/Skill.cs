using FALAAG.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FALAAG.Models
{
	public enum SkillType
	{
		Climbing,
		Vaulting,
		Crawling,
		WireCutting,
		KoolAidManning,
	}
	public class Skill
	{
		public int Peak { get; set; } // Peak of skill achieved, to allow faster relearning after decay.
		public int DecayRate { get; set; } // Normally static, but can be modified rarely
		public List<AttributeComponent> AttributeComponents = new List<AttributeComponent>();
		// List of e.g. Attributes and their percentage contribution to passing the skill attempt.
		// Consider turning this into an Interface so it can be applied to items as well.
		// You might also need a list of lists, for when certain components of the skill are replaceable by others:
		// e.g., FORCE DOOR: ((Crowbar, Lockpick), (FMS))
		public string Name { get; set; }
		public string ID { get; set; }
		public string Description { get; set; }
		public int Modifier { get; set; }
		public int Value { get; set; }
		public int ModifiedValue { get; set; }
		public string ModifiedValueDescriptor
		{
			get
			{
				switch (ModifiedValue)
				{
					case <= 5:      // 00 - 05       5
						return "Useless";
					case <= 20:     // 06 - 20      14
						return "Awful";
					case <= 35:     // 21 - 35      14
						return "Bad";
					case <= 65:     // 36 - 65      19
						return "Average";
					case <= 80:     // 66 - 80      14
						return "Good";
					case <= 95:     // 81 - 95      14
						return "Great";
					case <= 100:    // 96 - 100      5
						return "Masterful";
					default:
						return "ERROR";
				}
			}
		}
		public int BaseValue { get; set; }

		public event EventHandler<string> OnActionPerformed;

		public Skill (string id, string name, string description)
		{
			ID = id;
			Name = name;
			Description = description;
		}

		protected void ReportResult(string result)
		{
			OnActionPerformed?.Invoke(this, result);
		}

		public Skill Clone() =>
			new Skill(ID, Name, Description);

		public void ReRoll()
		{
			BaseValue = DiceService.Instance.Roll(100).Value;
			ModifiedValue = BaseValue;
		}
	}
}
