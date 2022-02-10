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

		public void NarrateEntry()
		{
			MessageBroker.GetInstance().RaiseMessage(NarrationEntry);
		}
	}
}
