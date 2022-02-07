using FALAAG.Core;

namespace FALAAG.TimeSystem
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
