using FALAAG.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FALAAG.Models
{
	/// <summary>
	/// For compiling a list of available actions, for the player or the AI.
	/// This should also cover SkillChecks.
	/// </summary>
	public class ActionOption : INotifyPropertyChanged
	{
		// RE: Subclasses for each specific game logic (e.g. Lockpick, Leap, etc.)
		//		The alternative is to keep using a SkillType Enum, and create an
		//		action delegate library (unsure of term) between those enum members 
		//		and the skill-specific game logic. Then, (composition > inheritance?) 
		//		all of those methods are stored within ActionOption itself. 
		//		I think one of the nice advantages of no-audio/no-graphics will be 
		//		that pretty much every class is a flyweight, relatively speaking.
		public event PropertyChangedEventHandler PropertyChanged;
		public string ID { get; set; }
		public string Name { get; set; }
		public SkillType SkillType { get; set; }

		// These four are calculated according to skill values, ActionRate, and maybe others.
		// TODO: Set these values 
		public int Audibility { get; set; } // Number of cells at which can be heard, without obstructions; higher values can drown out lower values
		public int Difficulty { get; set; } // Difficulty should not be reported to the player - only SuccessChance.
		public int Duration { get; set; } // In AP or whatever they're called
		public int Visibility { get; set; } // Number of cells at which can be seen, with normal vision conditions.

		public PhysicalObject Target { get; set; }
		public Entity Actor { get; set; }
		public ActionRate ActionRate { get; set; }
		public int Chance { get; set; }

		public string OutcomeDescriptionDetail 
		{
			get => Actor.GetSkillByID(SkillType).Name + " " + Target.Name + Environment.NewLine +
				"===========================================" + Environment.NewLine +
				"More info here";
		} // For Action chooser canvas, formatted mini-page of chances and outcomes

		public ActionOption(string id, string name, SkillType skillType, Entity actor, PhysicalObject target, ActionRate actionRate)
		{
			ID = id;
			Name = name;
			SkillType = skillType;
			Target = target;
			Actor = actor;

			// These are just placeholders to avoid div/0 errors. 
			Difficulty = 50;
			Duration = 50;
			Audibility = 50;
			Visibility = 50;
		}

		internal void Execute(float successRatio)
		{
			// This is a placeholder but I don't remember the purpose
			string resultText = "";

			if (successRatio >= 1)
				resultText = "Success";
			else
				resultText = "Failure";

			MessageBroker.GetInstance().RaiseMessage(resultText);
			// Use Delegates
		}

		/// <summary>
		/// This is for copying from physical objects that contain options into the action queue.
		/// It might not be needed for other sources of ActionObjects (e.g. Entity), since those will need to be flexible enough to generate on the fly.
		/// </summary>
		/// <returns></returns>
		public ActionOption Clone() =>
			new ActionOption(ID, Name, SkillType, Actor, Target, ActionRate);
	}
}
