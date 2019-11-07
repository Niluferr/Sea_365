using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeaHub.Extensions;
using SeaHub.Utility;
using SeaHub.Models;
using SeaHub.Models.ViewModels;
using SeaHub.DataAccess.Data.Repository.IRepository;

namespace SeaHub.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private HomeViewModel HomeVM;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            HomeVM = new HomeViewModel() {
                CategoryList = _unitOfWork.Category.GetAll(),
                ServiceList  = _unitOfWork.Service.GetAll(includeProperties:"Frequency")
            };
            return View(HomeVM);
        }

        public IActionResult Details(int id)
        {
            var serviceFromDb = _unitOfWork.Service.GetFirstOrDefault(includeProperties: "Category,Frequency", filter: c => c.Id == id);
            return View(serviceFromDb);
        }

        public IActionResult AddToCart(int serviceId)
        {
            List<int> sessionList = new List<int>();
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SD.SessionCart)))
            {
                sessionList.Add(serviceId);
                HttpContext.Session.SetObject(SD.SessionCart, sessionList);
            }
            else
            {
                sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
                if (!sessionList.Contains(serviceId))
                {
                    sessionList.Add(serviceId);
                    HttpContext.Session.SetObject(SD.SessionCart, sessionList);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
