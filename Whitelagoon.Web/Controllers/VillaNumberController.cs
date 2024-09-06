using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Whitelagoon.Web.ViewModel;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace Whitelagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public VillaNumberController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        public IActionResult Index()
        {
            var villaNumbers = _UnitOfWork.VillaNumber.GetAll(includedProperties: "Villa"); 
            return View(villaNumbers);
        }

        public IActionResult Create() {

            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _UnitOfWork.Villa.GetAll().Select(u => new SelectListItem
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
           

            bool RoomNumberExists= _UnitOfWork.VillaNumber.Any(u=>u.Villa_Number==obj.VillaNumber.Villa_Number);

            if (ModelState.IsValid && !RoomNumberExists)
            {
                _UnitOfWork.VillaNumber.Add(obj.VillaNumber);
                _UnitOfWork.Save();
                TempData["success"] = "The Villa Number  Created Successfully";
                return RedirectToAction(nameof(Index));
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
                VillaList = _UnitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber= _UnitOfWork.VillaNumber.Get(u=>u.Villa_Number== villaNumberId)
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
                _UnitOfWork.VillaNumber.Update(villaNumberVM.VillaNumber);
                _UnitOfWork.Save();
                TempData["success"] = "The Villa Number  Updated Successfully";
                return RedirectToAction(nameof(Index));
            }


            villaNumberVM.VillaList = _UnitOfWork.Villa.GetAll().Select(u => new SelectListItem
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
                VillaList = _UnitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _UnitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)
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
                VillaNumber? obj= _UnitOfWork.VillaNumber.Get(u=>u.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);
                _UnitOfWork.VillaNumber.Remove(obj);
                _UnitOfWork.Save();
                TempData["success"] = "The Villa Number Data deleted Successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Villa Data deleted UnSuccessfully";
            return View();
        }
    }
}
