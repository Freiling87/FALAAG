using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FALAAG.Models
{
	public class Portal : PhysicalObject
	{
		public Wall Wall { get; set; }
		public bool Passable { get; set; }
		public bool Opened { get; set; }
		public ObjectAttachment Lock =>
			ObjectAttachments.Where(a => a.AttachmentType == AttachmentType.Lock).FirstOrDefault();
		public ObjectAttachment Trap =>
			ObjectAttachments.Where(a => a.AttachmentType == AttachmentType.Trap).FirstOrDefault();
		public ObjectAttachment Sensor =>
			ObjectAttachments.Where(a => a.AttachmentType == AttachmentType.Sensor).FirstOrDefault();

		public Portal(string id)
		{

		}
	}
}
