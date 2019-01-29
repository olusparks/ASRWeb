using System;
using System.Collections.Generic;

namespace ASRWebMVC.Models
{
    public partial class User
    {
        public User()
        {
            SlotBookedInStudent = new HashSet<Slot>();
            SlotStaff = new HashSet<Slot>();
        }

        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Slot> SlotBookedInStudent { get; set; }
        public virtual ICollection<Slot> SlotStaff { get; set; }
    }
}
