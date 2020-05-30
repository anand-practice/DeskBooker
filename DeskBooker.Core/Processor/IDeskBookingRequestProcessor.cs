using System.Threading.Tasks;
using DeskBooker.Core.Domain;

namespace DeskBooker.Core.Processor
{
    public interface IDeskBookingRequestProcessor
    {
        Task<DeskBookingResponse> BookDeskAsync(DeskBookingRequest request);
    }
}