using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Whitelagoon.Web.ViewModel;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace Whitelagoon.Web.Controllers
{
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public AmenityController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        public IActionResult Index()
        {
            var Amenitys = _UnitOfWork.Amenity.GetAll(includedProperties: "Villa"); 
            return View(Amenitys);
        }

        public IActionResult Create() {

            AmenitiesVM AmenityVM = new()
            {
                VillaList = _UnitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };

            return View(AmenityVM);
        }

        [HttpPost]
        public IActionResult Create(AmenitiesVM obj)
        {
           

            if (ModelState.IsValid)
            {
                _UnitOfWork.Amenity.Add(obj.Amenity);
                _UnitOfWork.Save();
                TempData["success"] = "The Amenity  Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }


        public IActionResult Update(int AmenityId)
        {
            AmenitiesVM AmenityVM = new()
            {
                VillaList = _UnitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity= _UnitOfWork.Amenity.Get(u=>u.Id== AmenityId)
            };


            if (AmenityVM.Amenity == null) {

                return RedirectToAction("Error","Home");
            }

            return View(AmenityVM);
        }

        [HttpPost]
        public IActionResult Update(AmenitiesVM AmenityVM)
        {

            if (ModelState.IsValid)
            {
                _UnitOfWork.Amenity.Update(AmenityVM.Amenity);
                _UnitOfWork.Save();
                TempData["success"] = "The Amenity  Updated Successfully";
                return RedirectToAction(nameof(Index));
            }


            AmenityVM.VillaList = _UnitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            return View(AmenityVM);
        }



        public IActionResult Delete(int AmenityId)
        {
            AmenitiesVM AmenityVM = new()
            {
                VillaList = _UnitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = _UnitOfWork.Amenity.Get(u => u.Id == AmenityId)
            };


            if (AmenityVM.Amenity == null)
            {

                return RedirectToAction("Error", "Home");
            }

            return View(AmenityVM);
        }


        [HttpPost]
        public IActionResult Delete(AmenitiesVM AmenityVM)
        {
            if (ModelState.IsValid)
            {
                Amenity? obj= _UnitOfWork.Amenity.Get(u=>u.Id == AmenityVM.Amenity.Id);
                _UnitOfWork.Amenity.Remove(obj);
                _UnitOfWork.Save();
                TempData["success"] = "The Amenity Data deleted Successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The Amenity Data deleted UnSuccessfully";
            return View();
        }
    }
}
