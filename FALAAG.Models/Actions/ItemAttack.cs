using System;
using FALAAG.Models;
using FALAAG.Models.Shared;
using FALAAG.Core;

namespace FALAAG.Actions
{
    public class ItemAttack : ItemAction, IAction
    {
        private readonly string _damageDice;

        public ItemAttack(Item itemInUse, string damageDice)
            : base(itemInUse)
        {
            if (itemInUse.Category != Item.ItemCategory.Weapon)
                throw new ArgumentException($"{itemInUse.Name} is not a weapon");

            if (string.IsNullOrWhiteSpace(damageDice))
                throw new ArgumentException("damageDice must be valid dice notation");

            _damageDice = damageDice;
        }

        private static bool AttackSucceeded(Entity actor, Entity target)
        {
            int dexA = actor.GetAttribute("GMS").ModifiedValue * DiceService.Instance.Roll(100, 1).Value / 100;
            int dexB = target.GetAttribute("GMS").ModifiedValue * DiceService.Instance.Roll(100, 1).Value / 100;

            return dexA >= dexB;
        }

        public void Execute(Entity actor, Entity target)
        {
            string actorName = (actor is Player) ? "You" : $"The {actor.Name.ToLower()}";
            string targetName = (target is Player) ? "you" : $"the {target.Name.ToLower()}";

            if (AttackSucceeded(actor, target))
            {
                int damage = DiceService.Instance.Roll(_damageDice).Value;
                ReportResult($"{actorName} hit {targetName} for {damage} point{(damage > 1 ? "s" : "")}.");
                target.TakeDamage(damage);
            }
            else
                ReportResult($"{actorName} missed {targetName}.");
        }
    }
}