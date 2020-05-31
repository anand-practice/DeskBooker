using System;

namespace DeskBooker.Core.Domain
{
    public class DeskBookingBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime Date { get; set; }
    }
}