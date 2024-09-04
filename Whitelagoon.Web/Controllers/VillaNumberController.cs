using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Whitelagoon.Web.ViewModel;
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
            var villaNumbers = _db.VillaNumbers.Include(u=>u.Villa).ToList();
            return View(villaNumbers);
        }

        public IActionResult Create() {

            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _db.villas.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };

            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Create(VillaNumberVM obj)
        {
           

            bool RoomNumberExists=_db.VillaNumbers.Any(u=>u.Villa_Number==obj.VillaNumber.Villa_Number);

            if (ModelState.IsValid && !RoomNumberExists)
            {
                _db.VillaNumbers.Add(obj.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = "The Villa Number  Created Successfully";
                return RedirectToAction("Index");
            }


            if (RoomNumberExists)
            {
                TempData["error"] = "The Villa Number Already Exist!";
            }
            
            return View(obj);
        }


        public IActionResult Update(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _db.villas.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber=_db.VillaNumbers.FirstOrDefault(u=>u.Villa_Number== villaNumberId)
            };


            if (villaNumberVM.VillaNumber == null) {

                return RedirectToAction("Error","Home");
            }

            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Update(VillaNumberVM villaNumberVM)
        {

            if (ModelState.IsValid)
            {
                _db.VillaNumbers.Update(villaNumberVM.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = "The Villa Number  Updated Successfully";
                return RedirectToAction("Index");
            }


            villaNumberVM.VillaList = _db.villas.ToList().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            return View(villaNumberVM);
        }



        public IActionResult Delete(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _db.villas.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _db.VillaNumbers.FirstOrDefault(u => u.Villa_Number == villaNumberId)
            };


            if (villaNumberVM.VillaNumber == null)
            {

                return RedirectToAction("Error", "Home");
            }

            return View(villaNumberVM);
        }


        [HttpPost]
        public IActionResult Delete(VillaNumberVM villaNumberVM)
        {
            if (ModelState.IsValid)
            {
                VillaNumber? obj=_db.VillaNumbers.FirstOrDefault(u=>u.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);
                _db.VillaNumbers.Remove(obj);
                _db.SaveChanges();
                TempData["success"] = "The Villa Number Data deleted Successfully"; 
                return RedirectToAction("Index");
            }
            TempData["error"] = "Villa Data deleted UnSuccessfully";
            return View();
        }
    }
}
