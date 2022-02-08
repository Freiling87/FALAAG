using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FALAAG.Models
{
	public class Player : Entity
    {
		#region Header
		private int _xp;

        public ObservableCollection<JobStatus> JobsActive { get; } = new ObservableCollection<JobStatus>();
        public ObservableCollection<Recipe> RecipesKnown { get; } = new ObservableCollection<Recipe>();
        public int Xp 
        { 
            get =>  _xp;
            private set 
            { 
                _xp = value;
                SetLevelAndMaximumHitPoints();
            }
        }

		public event EventHandler OnLeveledUp;

        public Player(string nameActual, string nameGeneral, int xp, int hpMax, int hpCur, IEnumerable<EntityAttribute> attributes, int cash) : 
                    base("Player", nameActual, nameGeneral, hpMax, hpCur, attributes, cash)
		{
            Xp = xp;
        }
        #endregion
        public void AddExperience(int xp) =>
            Xp += xp;
        public void LearnRecipe(Recipe recipe)
        {
            if (!RecipesKnown.Any(r => r.ID == recipe.ID))
                RecipesKnown.Add(recipe);
        }
        private void SetLevelAndMaximumHitPoints()
        {
            int originalLevel = Level;
            Level = (Xp / 100) + 1;

            if (Level != originalLevel)
            {
                HpMax = 100 + (Level * 10);
                OnLeveledUp?.Invoke(this, System.EventArgs.Empty);
            }
        }
	}
}