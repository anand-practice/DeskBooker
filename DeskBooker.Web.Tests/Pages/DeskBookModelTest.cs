using System.Collections.Generic;
using DeskBooker.Core.Domain;
using DeskBooker.Core.Processor;
using DeskBooker.Web.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using NUnit.Framework;

namespace DeskBooker.Web.Tests
{
    public class DeskBookModelTest
    {
        Mock<IDeskBookingRepository> _deskBookingRepository;
        Mock<IDeskRepository> _deskRepository;
        Mock<IDeskBookingRequestProcessor> _processor;
        DeskBookModel _deskBookModel;
        private DeskBookingResponse _deskBookingResponse;

        [SetUp]
        public void Setup()
        {
            _deskBookingRepository = new Mock<IDeskBookingRepository>();
            _deskRepository = new Mock<IDeskRepository>();
            _processor = new Mock<IDeskBookingRequestProcessor>();
            _deskBookModel = new DeskBookModel(_processor.Object)
            {
                DeskBookingRequest = new DeskBookingRequest()
            };
            _deskBookingResponse = new DeskBookingResponse
            {
                ResultCode = DeskBookingResultCode.Available
            };
            _processor.Setup(x => x.BookDeskAsync(_deskBookModel.DeskBookingRequest))
            .ReturnsAsync(
                _deskBookingResponse
           );
        }

        [TestCase(0, false)]
        [TestCase(1, true)]
        public void should_call_desk_book_if_model_is_valid(int expectedTimes, bool isValid)
        {
            if (!isValid)
            {
                _deskBookModel.ModelState.AddModelError("Key", "Error");
            }
            _deskBookingResponse.ResultCode = DeskBookingResultCode.NotAvailable;
            _deskBookModel.OnPost();
            _processor.Verify(x => x.BookDeskAsync(_deskBookModel.DeskBookingRequest), Times.Exactly(expectedTimes));
        }

        [Test]
        public void should_add_model_error_if_desk_unavailable()
        {

            _processor.Setup(x => x.BookDeskAsync(_deskBookModel.DeskBookingRequest)).ReturnsAsync(
                new DeskBookingResponse
                {
                    ResultCode = DeskBookingResultCode.NotAvailable
                }
            );

            _deskBookModel.OnPost();

            var modelErrorCount = _deskBookModel.ModelState.ErrorCount;

            Assert.AreEqual(1, modelErrorCount);
            Assert.IsTrue(_deskBookModel.ModelState.TryGetValue("DeskBookingRequest.Date", out ModelStateEntry modelStateEntry));
        }

        [Test]
        public void should_not_add_model_error_if_desk_available()
        {

            _deskBookingResponse.ResultCode = DeskBookingResultCode.Available;

            _deskBookModel.OnPost();

            var modelErrorCount = _deskBookModel.ModelState.ErrorCount;

            Assert.AreEqual(0, modelErrorCount);
            Assert.False(_deskBookModel.ModelState.TryGetValue("DeskBookingRequest.Date", out ModelStateEntry modelStateEntry));
        }

        [Test]
        public void should_redirect_to_book_confirm_page_on_success()
        {
            _deskBookingResponse.ResultCode = DeskBookingResultCode.Available;
            var result = _deskBookModel.OnPost();
            Assert.IsInstanceOf(typeof(RedirectToPageResult), result);
        }

        [Test]
        public void should_not_redirect_to_book_confirm_page_on_failure()
        {
            _deskBookingResponse.ResultCode = DeskBookingResultCode.NotAvailable;
            var result = _deskBookModel.OnPost();
            Assert.IsInstanceOf(typeof(PageResult), result);
        }

        [Test]
        public void should_return_booking_id_on_success()
        {
            _deskBookingResponse.ResultCode = DeskBookingResultCode.Available;
            _deskBookingResponse.DeskBookingId = 1;

            IActionResult actionResult = _deskBookModel.OnPost();

            var redirectToPageResult = (RedirectToPageResult)actionResult;
            Assert.AreEqual("BookingConfirmation", redirectToPageResult.PageName);

            IDictionary<string, object> routeValues = redirectToPageResult.RouteValues;
            Assert.AreEqual(1, routeValues.Count);

            Assert.IsTrue(routeValues.TryGetValue("DeskBookingId", out object deskBookingId));
            Assert.AreEqual(_deskBookingResponse.DeskBookingId, deskBookingId);
        }

    }
}