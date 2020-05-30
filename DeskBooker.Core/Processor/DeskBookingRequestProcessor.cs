using System;
using System.Linq;
using System.Threading.Tasks;
using DeskBooker.Core.Domain;

namespace DeskBooker.Core.Processor
{
    public class DeskBookingRequestProcessor : IDeskBookingRequestProcessor
    {
        private IDeskBookingRepository _deskBookingRepository;
        private IDeskRepository _deskRepository;

        public DeskBookingRequestProcessor(IDeskBookingRepository deskBooking,
                                IDeskRepository deskRepository)
        {
            _deskBookingRepository = deskBooking;
            _deskRepository = deskRepository;
        }

        public async Task<DeskBookingResponse> BookDeskAsync(DeskBookingRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));
            var desks = await _deskRepository.GetAvailableDesks(request.Date);
            DeskBookingResponse response = Create<DeskBookingResponse>(request);
            if (desks.FirstOrDefault() is Desk availableDesks)
            {
                var deskBooking = Create<DeskBooking>(request);
                deskBooking.DeskId = availableDesks.Id;
                _deskBookingRepository.Save(deskBooking);
                response.ResultCode = DeskBookingResultCode.Available;
                response.DeskBookingId = deskBooking.Id;
            }
            else
            {
                response.ResultCode = DeskBookingResultCode.NotAvailable;
            }
            return response;
        }

        private static T Create<T>(DeskBookingRequest request) where T : DeskBookingBase, new()
        {
            return new T
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone
            };
        }
    }


}