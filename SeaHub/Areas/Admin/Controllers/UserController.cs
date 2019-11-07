using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeaHub.DataAccess.Data.Repository.IRepository;
using SeaHub.Utility;
using System.Security.Claims;

namespace SeaHub.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.Admin)]
    [Area("Admin")]
    public class UserController : Controller
    {

        private readonly IUnitOfWork _UnitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return View(_UnitOfWork.User.GetAll(u=>u.Id != claims.Value));
        }

        public IActionResult Lock(string Id)
        {
            if(Id == null)
            {
                return NotFound();
            }
            _UnitOfWork.User.LockUser(Id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult UnLock(string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            _UnitOfWork.User.UnLockUser(Id);
            return RedirectToAction(nameof(Index));
        }

    }
}