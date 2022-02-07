using System.Collections.Generic;

namespace FALAAG.Models
{
    public class Automat : Entity
    {
        public Automat(string id, string name) : base(id, name, name, 100, 100, new List<EntityAttribute>(), 0)
		{
        }
    }
}