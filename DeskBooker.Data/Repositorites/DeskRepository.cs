
using System;
using System.Collections.Generic;
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
            return (List<Desk>)await _unitOfWork.Repository<Desk>().FindAllAsync(x => x.BookingDate == date);
        }
    }
}