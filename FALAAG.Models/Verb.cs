using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FALAAG.Models
{
	public class Verb
	{
		public string Infinitive { get; }
		public bool Irregular { get; }

		internal string Conjugate(Entity actor)
		{
			throw new NotImplementedException();
		}
	}
}
