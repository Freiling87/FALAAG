using FALAAG.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace FALAAG.Models
{
	public class PhysicalObject : INotifyPropertyChanged
	{
		// TODO: Add observer methods for whenever they ought to be narrated (OnEntered, OnChanged, OnDestroyed, etc.) 

		public string NarrationEntry;
		public event PropertyChangedEventHandler PropertyChanged;
		public string Name { get; set; }
		public string ID { get; set; }
		public List<ActionOption> ActionOptions = new ();
		// Changed ActionOptions to List from ObservableCollection. If you get errors on forms generating this content, this could be why.

		public List<ObjectAttachment> ObjectAttachments { get; set; }

		public ActionOption GetSkillCheck(SkillType skillType) =>
			ActionOptions.FirstOrDefault(action => action.SkillType == skillType);

		private int _opacity;
		// 100: No visbility
		// 50: Reduce the Visibility Distance of all objects past it by 50%
		private int _solidity;
		// 100: Solid earth, blocks sound completely
		// 25: Sheetrock wall, allows sensation of objects beyond at 75% of Audibility Distance 
		private int _visibleDistance;
		// TODO: Ambient sounds generated from something like a MachineOperate() action

		public void NarrateEntry()
		{
			MessageBroker.GetInstance().RaiseMessage(NarrationEntry);
		}
	}
}
