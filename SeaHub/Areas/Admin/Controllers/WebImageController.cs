﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeaHub.DataAccess.Data;
using SeaHub.DataAccess.Data.Repository.IRepository;
using SeaHub.Models;
using SeaHub.Utility;
using System.IO;
using System.Linq;

namespace SeaHub.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class WebImageController : Controller
    {

        private readonly ApplicationDbContext _db;

        public WebImageController(ApplicationDbContext db)
        {
            _db = db;
        }

        //Index
        public IActionResult Index()
        {
            return View();
        }

        //Resim Getir
        public IActionResult Upsert(int? id)
        {
            WebImages imageObj = new WebImages();
            if (id == null)
            {

            }
            else
            {
                imageObj = _db.WebImages.FirstOrDefault(m => m.Id == id);
                if (imageObj == null)
                {
                    return NotFound();
                }
            }

            return View(imageObj);
        }

        //Resim Kaydet 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(int id, WebImages imageObj)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }
                    imageObj.Picture = p1;
                }

                if (imageObj.Id == 0)
                {
                    _db.WebImages.Add(imageObj);
                }
                else
                {
                    var imageFromDb = _db.WebImages.Where(c => c.Id == id).FirstOrDefault();
                    imageFromDb.Name = imageObj.Name;
                    if (files.Count > 0)
                    {
                        imageFromDb.Picture = imageObj.Picture;
                    }
                }
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(imageObj);
        }

        #region API CALLS

        //GetAll 
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _db.WebImages.ToList() });
        }

        //Delete 
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _db.WebImages.Find(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }
            _db.WebImages.Remove(objFromDb);
            _db.SaveChanges();

            return Json(new { success = true, message = "Deleted Succesful." });
        }

        #endregion

    }
}
