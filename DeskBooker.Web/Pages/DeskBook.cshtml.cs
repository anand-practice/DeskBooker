using DeskBooker.Core.Domain;
using DeskBooker.Core.Processor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DeskBooker.Web.Pages
{
    public class DeskBookModel : PageModel
    {
        private IDeskBookingRequestProcessor _deskBookingRequestProcessor;

        [BindProperty]
        public DeskBookingRequest DeskBookingRequest { get; set; }
        public DeskBookModel(IDeskBookingRequestProcessor deskBookingRequestProcessor)
        {
            _deskBookingRequestProcessor = deskBookingRequestProcessor;
        }

        public IActionResult OnPost()
        {
            IActionResult actionResult = Page();
            if (ModelState.IsValid)
            {
                var response = _deskBookingRequestProcessor.BookDeskAsync(DeskBookingRequest);
                if (response.Result.ResultCode == DeskBookingResultCode.NotAvailable)
                    ModelState.AddModelError("DeskBookingRequest.Date", "No desks on this date");
                else
                {
                    actionResult = RedirectToPage("BookingConfirmation", new
                    {
                        response.Result.DeskBookingId
                    });
                }
            }
            return actionResult;
        }
    }
}