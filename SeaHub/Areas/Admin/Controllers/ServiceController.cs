using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SeaHub.DataAccess.Data.Repository.IRepository;
using SeaHub.Models.ViewModels;
using System;
using System.IO;

namespace SeaHub.Areas.Admin.Controllers
{

    [Authorize]
    [Area("Admin")]
    public class ServiceController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        [BindProperty]
        public ServiceVM ServVM { get; set; }

        public ServiceController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            ServiceVM ServVM = new ServiceVM()
            {
                Service = new Models.Service(),
                CategoryList = _unitOfWork.Category.GetCategoryListDropDown(),
                FrequencyList = _unitOfWork.Frequency.GetFrequencyListDropDown(),
            };
            if(id != null)
            {
                ServVM.Service = _unitOfWork.Service.Get(id.GetValueOrDefault());
            }
            return View(ServVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;

                var files = HttpContext.Request.Form.Files;
                if (ServVM.Service.Id == 0)
                {
                    //New Service
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\services");
                    var extension = Path.GetExtension(files[0].FileName);
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }
                    ServVM.Service.ImageUrl = @"\images\services\" + fileName + extension;
                    _unitOfWork.Service.Add(ServVM.Service);
                }
                else
                {
                    //Edit Service
                    var objFromDb = _unitOfWork.Service.Get(ServVM.Service.Id);
                    if (files.Count > 0)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(webRootPath, @"images\services");
                        var extension_new = Path.GetExtension(files[0].FileName);

                        var imagePath = Path.Combine(webRootPath, objFromDb.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                          using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension_new), FileMode.Create))
                            {
                                files[0].CopyTo(fileStreams);
                            }
                            ServVM.Service.ImageUrl = @"\images\services\" + fileName + extension_new;
                    }
                    else
                    {
                        ServVM.Service.ImageUrl = objFromDb.ImageUrl;
                    }
                    _unitOfWork.Service.Update(ServVM.Service);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(ServVM);
            }
        }

        #region API CALLS
        public IActionResult GetAll()
        {
            return Json(new {data = _unitOfWork.Service.GetAll(includeProperties: "Category,Frequency")});
        }

        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Service.Get(id);
            string webRootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, objFromDb.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            if(objFromDb == null)
            {
                return Json( new { success=false, message="Error while deleting."});
            }
            _unitOfWork.Service.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted Successfuly." });
        }

        #endregion

    }
}
