using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace Whitelagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext _db;

        public VillaNumberController(ApplicationDbContext db)
        {
            _db=db;
        }

        public IActionResult Index()
        {
            var villaNumbers = _db.VillaNumbers.ToList();
            return View(villaNumbers);
        }

        public IActionResult Create() { 

            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa obj)
        {
            if (obj.Name == obj.Description) {
                ModelState.AddModelError("Name", "The Description Can not Exactly math the Name");
            }


            if (ModelState.IsValid)
            {
                _db.villas.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Villa Data Created Successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Villa Data Created UnSuccessfully";
            return View(obj);
        }


        public IActionResult Update(int villaId)
        {
            Villa? obj=_db.villas.FirstOrDefault(u=>u.Id == villaId);

            if (obj == null) {

                return RedirectToAction("Error","Home");
            }

            return View(obj);
        }

        [HttpPost]
        public IActionResult Update(Villa obj)
        {
            if (ModelState.IsValid && obj.Id>0)
            {
                _db.villas.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Villa Data Updated Successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Villa Data Updated UnSuccessfully";
            return View(obj);
        }



        public IActionResult Delete(int villaId)
        {
            Villa? obj = _db.villas.FirstOrDefault(u => u.Id == villaId);

            if (obj == null)
            {

                return RedirectToAction("Error", "Home");
            }

            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(int? villaId)
        {
            if (ModelState.IsValid)
            {
                Villa? obj=_db.villas.FirstOrDefault(u=>u.Id == villaId);
                _db.villas.Remove(obj);
                _db.SaveChanges();
                TempData["success"] = "Villa Data deleted Successfully"; 
                return RedirectToAction("Index");
            }
            TempData["error"] = "Villa Data deleted UnSuccessfully";
            return View();
        }
    }
}
