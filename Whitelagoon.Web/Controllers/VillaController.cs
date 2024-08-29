using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace Whitelagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _db;

        public VillaController(ApplicationDbContext db)
        {
            _db=db;
        }

        public IActionResult Index()
        {
            var villas =_db.villas.ToList();
            return View(villas);
        }

        public IActionResult Create() { 

            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa obj)
        {
            if (obj.Name == obj.Description) {
                ModelState.AddModelError("","The Description Can not Exactly math the Name");
            }


            if (ModelState.IsValid)
            {
                _db.villas.Add(obj);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(obj);
        }
    }
}
