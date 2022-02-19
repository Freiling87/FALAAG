using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FALAAG.Models
{
	/// <summary>
	/// TODO: 
	///		Turn ObjectAttachment into an Interface.
	///		Use PhysObject or Item for Locks
	///		Other OA-implementing possibilities: 
	///			Weapon mod
	///			Any object that needs to be Placed in the scene (e.g. crates blocking a door, rug blocking a trapdoor)
	///		bool HidesHostObject
	/// </summary>
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
		public PhysicalObject HostObject { get; set; }

		// TODO: Factory, Constructor
	}
}
