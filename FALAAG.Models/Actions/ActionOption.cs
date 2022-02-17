﻿using System;
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
		public string ID { get; set; }
		public string Name { get; set; }
		public SkillType SkillType { get; set; }
		public int Difficulty { get; set; }
		public int Audibility { get; set; }
		public int Visibility { get; set; }

		public ActionOption(string id, string name, SkillType skillType, int difficulty, int audibility, int visibility)
		{
			ID = id;
			Name = name;
			SkillType = skillType;
			Difficulty = difficulty;
			Audibility = audibility;
			Visibility = visibility;
		}
	}
}