using System;
using System.Linq;
using System.Threading.Tasks;
using DeskBooker.Core.Domain;
using DeskBooker.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DeskBooker.Web.Controllers
{
    public class HomeController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ActionResult> Index()
        {
            var temp = await _unitOfWork.Repository<Desk>().GetAllAsync();
            ViewBag.Count = temp.Count();

            var newDesk = new Desk { BookingDate = DateTime.Now };
            await _unitOfWork.Repository<Desk>().AddAsync(newDesk);
            return View();
        }
    }
}