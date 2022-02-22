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
		public event PropertyChangedEventHandler PropertyChanged;
		public string ID { get; set; }
		public string Name { get; set; }
		public SkillType SkillType { get; set; }
		public int Difficulty { get; set; }
		public int Audibility { get; set; }
		public int Visibility { get; set; }
		public int Duration { get; set; }
		public PhysicalObject Target { get; set; }
		public Entity Actor { get; set; }
		public int Chance { get; set; }

		public string OutcomeDescriptionDetail 
		{
			get => Actor.GetSkillByID(SkillType).Name + " " + Target.Name + Environment.NewLine +
				"===========================================" + Environment.NewLine +
				"More info here";
		} // For Action chooser canvas, formatted mini-page of chances and outcomes

		public ActionOption(string id, string name, SkillType skillType, int difficulty, int audibility, int visibility, PhysicalObject hostObject, int duration)
		{
			ID = id;
			Name = name;
			SkillType = skillType;
			Difficulty = difficulty;
			Duration = duration;
			Audibility = audibility;
			Visibility = visibility;
			Target = hostObject;
		}

		internal void Execute(float successRatio)
		{
			string resultText = "";

			if (successRatio >= 1)
				resultText = "Success";
			else
				resultText = "Failure";

			MessageBroker.GetInstance().RaiseMessage(resultText);
			// Use Delegates
		}
	}
}
