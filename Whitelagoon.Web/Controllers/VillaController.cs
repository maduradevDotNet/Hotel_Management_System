 using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace Whitelagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment _WebHostEnvironment;

        public VillaController(IUnitOfWork UnitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _UnitOfWork = UnitOfWork;
            _WebHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var villas = _UnitOfWork.Villa.GetAll();
            return View(villas);
        }

        public IActionResult Create() {

            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa obj)
        {
            if (obj.Name == obj.Description)
            {
                ModelState.AddModelError("name", "The description cannot exactly match the Name.");
            }
            if (ModelState.IsValid)
            {
                if (obj.Image!=null)
                {
                    string fileName=Guid.NewGuid().ToString()+Path.GetExtension(obj.Image.FileName) ;
                    string imagePath = Path.Combine(_WebHostEnvironment.WebRootPath, @"images\VillaImage");

                    using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create) ;
                        obj.Image.CopyTo(fileStream);

                    obj.ImageUrl = @"\images\VillaImage\" + fileName;

                }
                else
                {
                    obj.ImageUrl = "/images/placeholder.png";

                }

                _UnitOfWork.Villa.Add(obj);
                _UnitOfWork.Save();
                TempData["success"] = "The villa has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        public IActionResult Update(int villaId)
        {
            Villa? obj = _UnitOfWork.Villa.Get(u=>u.Id==villaId);
            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }

        //[HttpPost]
        //public IActionResult Update(Villa obj)
        //{ 
        //    if (ModelState.IsValid && obj.Id > 0)
        //    {
        //        if (obj.Image != null)
        //        {

        //            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
        //            string imagePath = Path.Combine(_WebHostEnvironment.WebRootPath, @"images\VillaImage");

        //            if (!string.IsNullOrEmpty(obj.ImageUrl))
        //            {
        //               var oldImagePath = Path.Combine(_WebHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\')):


        //                if (System.IO.File.Exists(imagePath))
        //                { 
        //                    System.IO.File.Delete(imagePath);
        //                }
        //            }

        //            using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
        //            obj.Image.CopyTo(fileStream);

        //            obj.ImageUrl = @"\images\VillaImage\" + fileName;

        //        }

        //        _UnitOfWork.Villa.Update(obj);
        //        _UnitOfWork.Save();
        //        TempData["success"] = "The villa has been updated successfully.";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View();
        //}


        [HttpPost]
        public IActionResult Update(Villa obj)
        {
            if (ModelState.IsValid && obj.Id > 0)
            {
                if (obj.Image != null)
                {
                    // Generate new file name
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    string imagePath = Path.Combine(_WebHostEnvironment.WebRootPath, @"images\VillaImage");

                    // Check if there's an existing image and delete it
                    if (!string.IsNullOrEmpty(obj.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_WebHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));

                        // Check if the old image exists and delete it
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Upload the new image
                    using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                    obj.Image.CopyTo(fileStream);

                    // Update the image URL in the object
                    obj.ImageUrl = @"\images\VillaImage\" + fileName;
                }

                // Update villa and save changes
                _UnitOfWork.Villa.Update(obj);
                _UnitOfWork.Save();
                TempData["success"] = "The villa has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            // If model is invalid, return the view
            return View();
        }



        public IActionResult Delete(int villaId)
        {
            Villa? obj = _UnitOfWork.Villa.Get(u=>u.Id==villaId);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? objFromDb = _UnitOfWork.Villa.Get(u => u.Id == obj.Id);
            if (objFromDb is not null)
            {

                if (!string.IsNullOrEmpty(objFromDb.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_WebHostEnvironment.WebRootPath, objFromDb.ImageUrl.TrimStart('\\'));

                    // Check if the old image exists and delete it
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }


                _UnitOfWork.Villa.Remove(objFromDb);
                _UnitOfWork.Save();
                TempData["success"] = "The villa has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Failed to delete the villa.";
            }
            return View();
        }
    }
}
