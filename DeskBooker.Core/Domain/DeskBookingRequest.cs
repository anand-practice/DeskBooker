using System;

namespace DeskBooker.Core.Domain
{
    public class DeskBookingRequest : DeskBookingBase
    {
        public DateTime Date { get;  set; }
    }
}