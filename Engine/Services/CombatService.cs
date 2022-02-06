using Engine.Models;
using Engine.Shared;
using FALAAG.Core;

namespace Engine.Services
{
    public static class CombatService
    {
        public enum Combatant
        {
            Player,
            Opponent
        }

        public static Combatant FirstAttacker(Entity actor, Entity target)
        {
            int dexA = actor.GetAttribute("GMS").ModifiedValue + DiceService.Instance.Roll(10, 10).Value;
            int dexB = target.GetAttribute("GMS").ModifiedValue + DiceService.Instance.Roll(10, 10).Value;

            return dexA >= dexB
                ? Combatant.Player
                : Combatant.Opponent;
        }

        public static bool AttackSucceeded(Entity actor, Entity target)
        {
            int dexA = actor.GetAttribute("GMS").ModifiedValue + DiceService.Instance.Roll(10, 10).Value;
            int dexB = target.GetAttribute("GMS").ModifiedValue + DiceService.Instance.Roll(10, 10).Value;

            return dexA >= dexB;
        }
    }
}