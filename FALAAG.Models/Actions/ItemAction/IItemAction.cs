using FALAAG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FALAAG.Actions
{
	public interface IItemAction
	{
		event EventHandler<string> OnActionPerformed;
		void Execute(Entity actor, Entity target);
	}
}
