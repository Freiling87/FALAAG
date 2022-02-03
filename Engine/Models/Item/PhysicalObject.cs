using Engine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
	public class PhysicalObject : BaseNotificationClass
	{
		// TODO: Add observer methods for whenever they ought to be narrated (OnEntered, OnChanged, OnDestroyed, etc.) 

		public string NarrationEntry;
		public void NarrateEntry()
		{
			MessageBroker.GetInstance().RaiseMessage(NarrationEntry);
		}
	}
}
