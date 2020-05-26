
namespace DeskBooker.Core.Domain
{
    public class DeskBookingResponse : DeskBookingBase
    {
        public DeskBookingResultCode ResultCode { get; set; }
        public int? DeskBookingId { get; set; }
    }
}
