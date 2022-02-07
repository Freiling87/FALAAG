using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FALAAG.TimeSystem
{
	public static class TimeSystem
	{
		private static int _currentTime;
		private static List<ScheduledEvent> _scheduledEvents = new List<ScheduledEvent>();

		public static void AdvanceTime(int quantity)
		{
			_currentTime += quantity;
			// TODO: Probably an Update Clock event here?
			int nearestEventDelay = _scheduledEvents.Min(x => x.Delay);
			List<ScheduledEvent> nearestEvents = new List<ScheduledEvent>();

			foreach (ScheduledEvent scheduledEvent in _scheduledEvents)
			{
				if (scheduledEvent.Delay == nearestEventDelay)
				{
					// Initiative roll is in the Getter
					// Reorder the list according to that result
					// Verify that having the roll in the getter won't break things - it shouldn't reroll every time it iterates through a member in the list

					nearestEvents.Add(scheduledEvent);
				}

				scheduledEvent.Delay -= nearestEventDelay;
			}

			nearestEvents.OrderBy(x => x.InitiativeRoll);
		}

		public static void Interrupt(ScheduledEvent scheduledEvent)
		{
			// This is a possibly unneeded failsafe, if AdvanceTime works correctly
		}


	}
}
