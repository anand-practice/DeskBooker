using DeskBooker.Core.Domain;

public interface IDeskBookingRepository
{
    void Save(DeskBooking deskBooking);
    bool CheckifDeskAvailable(DeskBooking deskBooking);
}