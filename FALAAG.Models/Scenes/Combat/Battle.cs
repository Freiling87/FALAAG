using System;
using FALAAG.Models.EventArgs;
using FALAAG.Core;
using FALAAG.Models.Shared;

namespace FALAAG.Models
{
    public class Battle : IDisposable
    {
        private readonly MessageBroker _messageBroker = MessageBroker.GetInstance();
        private readonly Player _player;
        private readonly NPC _opponent;

        public event EventHandler<VictoryEventArgs> OnCombatVictory;

        public Battle(Player player, NPC opponent)
        {
            _player = player;
            _opponent = opponent;

            _player.OnActionPerformed += OnCombatantActionPerformed;
            opponent.OnActionPerformed += OnCombatantActionPerformed;
            // This was causing double-messages in combat logs. Not sure if commenting out will cause other issues, so only remove this line when you're sure.
            _opponent.OnKilled += OnOpponentKilled;

            _messageBroker.RaiseMessage("");
            _messageBroker.RaiseMessage($"You see a {_opponent.Name} here!");

            if (FirstAttacker(_player, _opponent) == Combatant.Opponent)
                AttackPlayer();
        }

        private enum Combatant
        {
            Player,
            Opponent
        }

        private static Combatant FirstAttacker(Entity actor, Entity target)
        {
            int dexA = actor.GetAttribute("GMS").ModifiedValue + DiceService.Instance.Roll(10, 10).Value;
            int dexB = target.GetAttribute("GMS").ModifiedValue + DiceService.Instance.Roll(10, 10).Value;

            return dexA >= dexB
                ? Combatant.Player
                : Combatant.Opponent;
        }

        public void AttackOpponent()
        {
            if (_player.CurrentWeapon == null)
            {
                _messageBroker.RaiseMessage("You must select a weapon to attack.");
                return;
            }

            _player.UseCurrentWeaponOn(_opponent);

            if (_opponent.IsAlive)
                AttackPlayer();
        }

        public void Dispose()
        {
            _player.OnActionPerformed -= OnCombatantActionPerformed;
            _opponent.OnActionPerformed -= OnCombatantActionPerformed;
            _opponent.OnKilled -= OnOpponentKilled;
        }

        private void OnOpponentKilled(object sender, System.EventArgs e)
        {
            _messageBroker.RaiseMessage("");
            _messageBroker.RaiseMessage($"You defeated the {_opponent.Name}!");

            _messageBroker.RaiseMessage($"You receive {_opponent.XpReward} XP.");
            _player.AddExperience(_opponent.XpReward);

            _messageBroker.RaiseMessage($"You receive ${_opponent.Cash}.");
            _player.CashReceive(_opponent.Cash);

            foreach (Item Item in _opponent.Inventory.Items)
            {
                _messageBroker.RaiseMessage($"You receive one {Item.Name}.");
                _player.InventoryAddItem(Item);
            }

            OnCombatVictory?.Invoke(this, new VictoryEventArgs());
        }

        private void AttackPlayer() =>
            _opponent.UseCurrentWeaponOn(_player);

        private void OnCombatantActionPerformed(object sender, string result) =>
            _messageBroker.RaiseMessage(result);
    }
}