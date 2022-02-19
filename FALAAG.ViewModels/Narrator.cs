using FALAAG.Core;
using FALAAG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FALAAG.ViewModels
{
	public static class Narrator
	{
		public enum NarrativePerson
		{
			Second,
			Third
		}
		public enum NarrativeTense
		{
			Present,
			Past
		}

		public static void OnMovement(Cell cell)
		{
			IntroduceCell(cell);
			IntroduceNPCs(cell);
			IntroduceFeatures(cell);
			IntroduceWalls(cell);
			IntroduceItems(cell);
		}

		// Introduction methods should have more detail than normal. Reintroduce will be for when the player has already encountered the object and gets the idea.
		public static void IntroduceCell(Cell cell)
		{
			// TODO: Change all names to lower by default, since you might need to enforce capitalization on some (e.g. brand names).
            string narration = PlayerCharacterPronoun() + " walk to a " + cell.Name.ToLower() + ".";

            MessageBroker.GetInstance().RaiseMessage(narration);
        }
        public static void IntroduceNPC(NPC npc)
		{
            string narration = "You see a " + npc.Name + ".";

            MessageBroker.GetInstance().RaiseMessage(narration);
        }
		public static void IntroduceNPCs(Cell cell)
		{
			if (!cell.NPCs?.Any() ?? false)
				return;

			List<NPC> unintroduced = cell.NPCs;
			List<Role> roles = cell.NPCs.Select(n => n.CurrentRole).Distinct().ToList();
			// Consider replacing Role with current Activity.
			List<NPC> somebodies = cell.NPCs.Where(n => n.Importance > 50).ToList();
			List<NPC> nobodies = cell.NPCs.Where(n => n.Importance <= 50).ToList();

			foreach (NPC npc in cell.NPCs)
				MessageBroker.GetInstance().RaiseMessage("A " + npc.Name + " is here.");
		}
		public static void IntroduceFeatures(Cell cell)
		{
			if (!cell.Features?.Any() ?? false)
				return;

			List<string> featureTypes = cell.Features.Select(f => f.Name).Distinct().ToList();

			foreach (string featureType in featureTypes)
			{
				List<Feature> features = cell.Features.Where(f => f.Name == featureType).ToList();
				string narration = "";

				if (features.Count == 1)
				{
					narration = "There is a " + featureType + " here.";
				}
				else
				{
					narration = "There are " + features.Count.ToString() + " " + featureType + "s here.";
				}

				MessageBroker.GetInstance().RaiseMessage(narration);
			}
		}
		public static void IntroduceWalls(Cell cell)
		{
			List<string> wallTypes = cell.Walls.Select(g => g.Name).Distinct().ToList();

			foreach (string wallType in wallTypes)
			{
				List<Wall> walls = cell.Walls.Where(g => g.Name == wallType).ToList();
				string narration = "";

				if (walls.Count == 1)
				{
					Wall wall = walls[0];
					narration = "There is a " + wallType.ToLower() + " to the " + wall.GetDirection(cell).ToString().ToLower() + ".";
				}
				else
				{
					// TODO: Make a List → Text method
					narration = "There are " + wallType.ToLower() + "s to the "
						+ string.Join(", ", ListDirections(cell.Walls.Where(g => g.Name == wallType).Select(g => g.GetDirection(cell)).ToList()).ToLower())
						+ ".";
				}

				MessageBroker.GetInstance().RaiseMessage(narration);
			}
		}
		public static void IntroduceItems(Cell cell)
		{
			if (!cell.Items?.Any() ?? false)
				return;

			List<string> itemTypes = cell.Items.Select(f => f.Name).Distinct().ToList();

			foreach (string itemType in itemTypes)
			{
				List<Item> items = cell.Items.Where(f => f.Name == itemType).ToList();
				string narration = "";

				if (items.Count == 1)
				{
					narration = "There is a " + itemType + " here.";
				}
				else
				{
					narration = "There are " + items.Count.ToString() + " " + itemType + "s here.";
				}

				MessageBroker.GetInstance().RaiseMessage(narration);
			}
		}
		public static string ListDirections(List<Direction> directions)
		{
			string list = "";

			foreach (Direction direction in directions)
			{
				list += direction.ToString();

				if (direction == directions.Last())
					continue; // Do not assume this is the end of the sentence.
				else if (direction == directions[directions.Count() - 1])
					list += " and ";
				else
					list += ", ";
			}

			return list;
		}
		public static string PlayerCharacterPronoun()
		{
			// TODO: Third person
			return "You";
		}
		public static string Conjugate(List<Noun> actors, Verb verb, NarrativeTense tense)
		{
			// TODO: Create list of Irregulars with XML-stored conjugations. 
			string conjugation = "";

			if (verb.Irregular)
			{

			}
			else
			{
				if (tense == NarrativeTense.Past)
					conjugation += "ed";
				else if (tense == NarrativeTense.Present)
				{
					if (actors.Count == 1)
						conjugation += "s";
				}
					
			}

			return conjugation;
		}

	}
}
