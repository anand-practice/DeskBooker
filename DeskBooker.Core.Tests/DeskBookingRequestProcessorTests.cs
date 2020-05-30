
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeskBooker.Core.Domain;
using DeskBooker.Core.Processor;
using Moq;
using NUnit.Framework;

public class DeskBookingRequestProcessorTests
{
    DeskBookingRequestProcessor _processor;
    DeskBookingRequest _request;
    private List<Desk> _desks;
    Mock<IDeskBookingRepository> _deskBookingMock;
    Mock<IDeskRepository> _deskRepositoryMock;
    [SetUp]
    public void Initialize()
    {
        _request = new DeskBookingRequest
        {
            FirstName = "First Name",
            LastName = "Last Name",
            Email = "Email",
            Phone = "Phone"
        };
        _desks = new List<Desk> { new Desk { Id = 1, BookingDate = new DateTime() } };
        _deskBookingMock = new Mock<IDeskBookingRepository>();
        _deskRepositoryMock = new Mock<IDeskRepository>();
        _deskRepositoryMock.Setup(x => x.GetAvailableDesks(It.IsAny<DateTime>()))
       .ReturnsAsync(_desks);

        _processor = new DeskBookingRequestProcessor(_deskBookingMock.Object, _deskRepositoryMock.Object);
    }
    [Test]
    public async Task should_return_values_on_booking_requestAsync()
    {
        var response = await _processor.BookDeskAsync(_request);
        Assert.IsNotNull(response);
        Assert.AreEqual(_request.FirstName, response.FirstName);
        Assert.AreEqual(_request.LastName, response.LastName);
        Assert.AreEqual(_request.Email, response.Email);
        Assert.AreEqual(_request.Phone, response.Phone);
    }

    [Test]
    public void should_throw_exception_on_null_input()
    {
        var exception = Assert.ThrowsAsync<ArgumentNullException>(async () => await _processor.BookDeskAsync(null));
        Assert.AreEqual("request", exception.ParamName);
    }

    [Test]
    public async Task should_save_desk_bookingAsync()
    {
        DeskBooking savedBooking = null;
        _deskBookingMock.Setup(x => x.Save(It.IsAny<DeskBooking>())).Callback<DeskBooking>(deskBooking =>
        {
            savedBooking = deskBooking;
        });
        await _processor.BookDeskAsync(_request);
        _deskBookingMock.Verify(x => x.Save(It.IsAny<DeskBooking>()), Times.Once);
        Assert.IsNotNull(savedBooking);
        Assert.AreEqual(_request.FirstName, savedBooking.FirstName);
        Assert.AreEqual(_request.LastName, savedBooking.LastName);
        Assert.AreEqual(_request.Email, savedBooking.Email);
        Assert.AreEqual(_request.Phone, savedBooking.Phone);
        Assert.AreEqual(_desks.First().Id, savedBooking.DeskId);
    }

    [Test]
    public async Task should_not_save_if_unavailableAsync()
    {
        _desks.Clear();
        await _processor.BookDeskAsync(_request);
        _deskRepositoryMock.Verify(x => x.GetAvailableDesks(It.IsAny<DateTime>()), Times.Once);
        _deskBookingMock.Verify(x => x.Save(It.IsAny<DeskBooking>()), Times.Never);
    }

    [TestCase(DeskBookingResultCode.NotAvailable, false)]
    [TestCase(DeskBookingResultCode.Available, true)]
    public async Task should_return_result_code(DeskBookingResultCode expectedResultCode, bool deskAvailable)
    {
        if (!deskAvailable)
        {
            _desks.Clear();
        }
        var result = await _processor.BookDeskAsync(_request);
        Assert.AreEqual(expectedResultCode, result.ResultCode);
    }

    [TestCase(null, false)]
    [TestCase(1, true)]
    public async Task should_return_expected_booking_id(int? expectedId, bool deskAvailable)
    {
        if (!deskAvailable)
        {
            _desks.Clear();
        }
        else
        {
            _deskBookingMock.Setup(x => x.Save(It.IsAny<DeskBooking>()))
            .Callback<DeskBooking>(deskBooking =>
            {
                deskBooking.Id = expectedId.Value;
            });
        }
        var result = await _processor.BookDeskAsync(_request);
        Assert.AreEqual(expectedId, result.DeskBookingId);
    }
}