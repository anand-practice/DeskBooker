
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeskBooker.Core.Domain;

namespace DeskBooker.Data.Repositories
{
    public class DeskRepository : IDeskRepository
    {
        private IUnitOfWork _unitOfWork;
        public DeskRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Desk>> GetAvailableDesks(DateTime date)
        {

            var bookedDeskIds = (await _unitOfWork.Repository<DeskBooking>()
            .FindAllAsync(x => x.Date == date)).Select(x => x.DeskId).ToList();

            return (await _unitOfWork.Repository<Desk>()
              .FindAllAsync(x => !bookedDeskIds.Contains(x.Id)))
              .ToList();

            // return (List<Desk>)await _unitOfWork.Repository<Desk>().FindAllAsync(x => x.BookingDate == date);
        }
    }
}