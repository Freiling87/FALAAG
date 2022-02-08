using FALAAG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FALAAG.Actions
{
	class ActionNew : IAction
	{
		public event EventHandler<string> OnActionPerformed;

		public void Execute(Entity actor, Entity target)
		{
			throw new NotImplementedException();
		}
	}
}
