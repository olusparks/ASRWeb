using System;

namespace ASRWebMVC.Models
{
	public class RoomModel
	{
		public string RoomId { get; set; }
		public DateTime StartTime { get; set; }
		public string StaffId { get; set; }
		public string BookedInStudentId { get; set; }
	}
}