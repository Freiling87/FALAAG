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
	public class ActionCommand : INotifyPropertyChanged
	{
		// RE: Subclasses for each specific game logic (e.g. Lockpick, Leap, etc.)
		//		The alternative is to keep using a SkillType Enum, and create an
		//		action delegate library (unsure of term) between those enum members 
		//		and the skill-specific game logic. Then, (composition > inheritance?) 
		//		all of those methods are stored within ActionCommand itself. 
		//		I think one of the nice advantages of no-audio/no-graphics will be 
		//		that pretty much every class is a flyweight, relatively speaking.
		public event PropertyChangedEventHandler PropertyChanged;
		public string ID { get; set; }
		public string Name { get; set; }
		public SkillType SkillType { get; set; }

		// These four are calculated according to skill values, ActionRate, and maybe others.
		// TODO: Set these values 
		public int Audibility { get; set; } // Number of cells at which can be heard, without obstructions; higher values can drown out lower values
		public string AudibilityString
		{
			get
			{
				return
				Audibility > 90
					? "Extremely Loud"
				: Audibility > 75
					? "Very Loud"
				: Audibility > 50
					? "Loud"
				: Audibility > 25
					? "Noisy"
				: Audibility > 10
					? "Quiet"
					: "Silent";
			}
		}
		public int Difficulty { get; set; } // Difficulty should not be reported to the player - only SuccessChance.
		public string DifficultyString
		{
			get
			{
				return
				Difficulty > 90
					? "Extremely Hard"
				: Difficulty > 75
					? "Very Hard"
				: Difficulty > 50
					? "Hard"
				: Difficulty > 25
					? "Simple"
				: Difficulty > 10
					? "Easy"
					: "Menial";
			}
		}
		public int Duration { get; set; } // In AP or whatever they're called
		public string DurationString
		{
			get
			{
				return
				Duration > 90
					? "Time-consuming"
				: Duration > 75
					? "Very Slow"
				: Duration > 50
					? "Slow"
				: Duration > 25
					? "Fast-ish"
				: Duration > 10
					? "Fast"
					: "Instant";
			}
		}
		public int Visibility { get; set; } // Number of cells at which can be seen, with normal vision conditions.
		public string VisibilityString
		{
			get
			{
				return
				Visibility > 90
					? "Blatant"
				: Visibility > 75
					? "Obvious"
				: Visibility > 50
					? "Noticeable"
				: Visibility > 25
					? "Subtle"
				: Visibility > 10
					? "Stealthy"
					: "Invisible";
			}
		}

		public PhysicalObject Target { get; set; }
		public Entity Actor { get; set; }
		public ActionRate ActionRate { get; set; }
		public int Chance { get; set; } // Get should calculate odds according to Actor

		private Direction _moveDirection;
		public Direction MoveDirection
		{
			get
			{
				if (SkillType != SkillType.Moving)
					return Direction.Null;

				return _moveDirection;
			}
			set
			{
				if (value != Direction.Null)
					SkillType = SkillType.Moving;

				_moveDirection = value;
			}
		}

		public string OutcomeDescriptionDetail 
		{
			get => Actor.GetSkillByID(SkillType).Name + " " + Target.Name + Environment.NewLine +
				"===========================================" + Environment.NewLine +
				"More info here";
		}

		public ActionCommand(string id, string name, SkillType skillType, Entity actor, PhysicalObject target, ActionRate actionRate)
		{
			ID = id;
			Name = name;
			SkillType = skillType;
			Target = target;
			Actor = actor;

			// These are just placeholders to avoid div/0 errors. 
			Difficulty = 5;
			Duration = 5;
			Audibility = 5;
			Visibility = 5;
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
		public ActionCommand Clone() =>
			new ActionCommand(ID, Name, SkillType, Actor, Target, ActionRate);
		public ActionCommand CloneForInteraction(Entity entity)
		{
			ActionCommand ac = new ActionCommand(ID, Name, SkillType, Actor, Target, ActionRate)
			{
				Actor = entity
			};

			return ac;
		}
	}
}