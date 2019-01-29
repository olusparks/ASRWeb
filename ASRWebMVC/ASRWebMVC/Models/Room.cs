﻿using System;
using System.Collections.Generic;

namespace ASRWebMVC.Models
{
    public partial class Room
    {
        public Room()
        {
            Slot = new HashSet<Slot>();
        }

        public string RoomId { get; set; }

        public virtual ICollection<Slot> Slot { get; set; }
    }
}
