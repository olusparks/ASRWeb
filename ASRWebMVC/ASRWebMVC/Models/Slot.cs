using System;
using System.Collections.Generic;

namespace ASRWebMVC.Models
{
    public partial class Slot
    {
        public string RoomId { get; set; }
        public DateTime StartTime { get; set; }
        public string StaffId { get; set; }
        public string BookedInStudentId { get; set; }

        public virtual User BookedInStudent { get; set; }
        public virtual Room Room { get; set; }
        public virtual User Staff { get; set; }
    }
}
