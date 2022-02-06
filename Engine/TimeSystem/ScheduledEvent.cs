using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Services;
using Engine.ViewModels;
using FALAAG.Core;

namespace Engine.TimeSystem
{
	public class ScheduledEvent
	{
		private int _delay;
		private int _initiativeRoll;

		public int Delay { set; get; }
		public int InitiativeRoll 
		{
			// Tiebreaker for shared AP slots
			set { _initiativeRoll = value; }
			get => 
				_initiativeRoll = DiceService.Instance.Roll(10, 10).Value;
		}

		public void Increment(int duration)
		{
			if (duration >= _delay)
			{
				TimeSystem.Interrupt(this);
			}
				
		}
		public void RollInitiative()
		{
			// Temporary Mockup

			InitiativeRoll = DiceService.Instance.Roll(10, 10).Value;
		}
	}
}
