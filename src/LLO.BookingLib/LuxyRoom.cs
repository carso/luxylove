﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace LLO.BookingLib
{
    public partial class LuxyRoom
    {
        public LuxyRoom()
        {
            LuxyBookings = new HashSet<LuxyBooking>();
            LuxyDailyServiceTemplates = new HashSet<LuxyDailyServiceTemplate>();
        }

        public string RoomCode { get; set; }
        public string RoomType { get; set; }
        public decimal? Price { get; set; }
        public bool? IsActive { get; set; }
        public string Name { get; set; }
        public string Floor { get; set; }
        public int? ProductId { get; set; }
        public string RoomNo { get; set; }

        public virtual Product Product { get; set; }
        public virtual ICollection<LuxyBooking> LuxyBookings { get; set; }
        public virtual ICollection<LuxyDailyServiceTemplate> LuxyDailyServiceTemplates { get; set; }
    }
}