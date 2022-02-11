using FALAAG.Core;
using System.ComponentModel;

namespace FALAAG.Models
{
	public class PhysicalObject : INotifyPropertyChanged
	{
		// TODO: Add observer methods for whenever they ought to be narrated (OnEntered, OnChanged, OnDestroyed, etc.) 

		public string NarrationEntry;
		public event PropertyChangedEventHandler PropertyChanged;
		public string Name { get; set; }
		public string ID { get; set; }

		private int _opacity;
		// 100: No visbility
		// 50: Reduce the Visibility Distance of all objects past it by 50%
		private int _solidity;
		// 100: Solid earth, blocks sound completely
		// 25: Sheetrock wall, allows sensation of objects beyond at 75% of Audibility Distance 
		private int _visibleDistance;
		private int _audibleDistance;

		public void NarrateEntry()
		{
			MessageBroker.GetInstance().RaiseMessage(NarrationEntry);
		}
	}
}
