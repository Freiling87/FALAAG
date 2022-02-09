using System.Collections.Generic;

namespace FALAAG.Models
{
    public class Automat : Entity
    {
        public Automat(string id, string name) : base(id, name, name, new List<EntityAttribute>(), new List<Skill>())
		{
        }
    }
}