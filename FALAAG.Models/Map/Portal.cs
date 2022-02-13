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
		public List<ObjectAttachment> Attachments { get; set; }
		public ObjectAttachment Lock =>
			Attachments.Where(a => a.AttachmentType == AttachmentType.Lock).FirstOrDefault();
		public ObjectAttachment Trap =>
			Attachments.Where(a => a.AttachmentType == AttachmentType.Trap).FirstOrDefault();
		public ObjectAttachment Sensor =>
			Attachments.Where(a => a.AttachmentType == AttachmentType.Sensor).FirstOrDefault();
		public List<ActionOption> ActionOptions = new List<ActionOption>();
	}
}
