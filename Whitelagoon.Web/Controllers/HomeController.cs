using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Whitelagoon.Web.Models;
using Whitelagoon.Web.ViewModel;
using WhiteLagoon.Application.Common.Interfaces;

namespace Whitelagoon.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public HomeController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new()
            {
                VillaList = _UnitOfWork.Villa.GetAll(includedProperties: "VillaAmenity"),
                Nights=1,
                CheckInDate=DateOnly.FromDateTime(DateTime.Now),
            };

            return View(homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

      
        public IActionResult Error()
        {
            return View();
        }
    }
}
