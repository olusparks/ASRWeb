using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASRWebMVC.Models
{
	public class SlotModel
	{
		public string RoomId { get; set; }
		public DateTime StartTime { get; set; }
		public string StaffId { get; set; }
		public string BookedInStudentId { get; set; }
		public virtual User Staff { get; set; }
		public virtual User BookedInStudent { get; set; }
	}
}
