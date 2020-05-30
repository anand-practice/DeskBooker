
using DeskBooker.Core.Domain;
using DeskBooker.Data.Repositories;

public class DeskBookingRepository : IDeskBookingRepository
{
    private IUnitOfWork _unitOfWork;

    public DeskBookingRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public void Save(DeskBooking deskBooking)
    {
        _unitOfWork.Repository<DeskBooking>().AddAsync(deskBooking);
    }
}