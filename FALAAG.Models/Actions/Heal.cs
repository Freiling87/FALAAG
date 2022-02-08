using System;
using FALAAG.Models;

namespace FALAAG.Actions
{
    public class Heal : ItemAction, IAction
    {
        private readonly int _hitPointsToHeal;

        public Heal(Item itemInUse, int hitPointsToHeal) : base(itemInUse)
        {
            if (itemInUse.Category != Item.ItemCategory.Consumable)
            {
                throw new ArgumentException($"{itemInUse.Name} is not consumable");
            }

            _hitPointsToHeal = hitPointsToHeal;
        }

        public void Execute(Entity actor, Entity target)
        {
            string actorName = (actor is Player) ? "You" : $"The {actor.NameGeneral.ToLower()}";
            string targetName = (target is Player) ? "yourself" : $"the {target.Name.ToLower()}";

            ReportResult($"{actorName} heal {targetName} for {_hitPointsToHeal} point{(_hitPointsToHeal > 1 ? "s" : "")}.");
            target.Heal(_hitPointsToHeal);
        }
    }
}