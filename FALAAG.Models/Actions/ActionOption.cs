using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FALAAG.Models
{
	/// <summary>
	/// For compiling a list of available actions, for the player or the AI.
	/// This should also cover SkillChecks.
	/// </summary>
	public class ActionOption
	{
		public SkillType SkillType { get; set; }
		public int Difficulty { get; set; }
		public int Noise { get; set; }
		public int Visibility { get; set; }

	}
}
