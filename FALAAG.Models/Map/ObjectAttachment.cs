using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FALAAG.Models
{
	public enum AttachmentType
	{
		Lock,
		Sensor,
		Trap
	}

	public class ObjectAttachment : PhysicalObject
	{
		public bool Locked { get; set; }
		public AttachmentType AttachmentType { get; set; }
	}
}
