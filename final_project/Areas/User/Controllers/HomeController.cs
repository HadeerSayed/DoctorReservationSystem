using final_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using models;
using Services;
using System.Numerics;

namespace final_project.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "Patient")]

    public class HomeController : Controller
    {
        public Ipatientservice _ipatientservice { get; }
        public Iuserservice _iuserservice { get; }
		public Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager { get; }

		public HomeController(Ipatientservice ipatientservice,
                                      Iuserservice iuserservice,
									  Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager)
        {
            _ipatientservice = ipatientservice;
            _iuserservice = iuserservice;
            userManager = _userManager;
        }
        // GET: HomeController/Edit/5
        public ActionResult Edit()
        {
            Patient p=_ipatientservice.getpatientinfo(getuser());
            return View(p);
        }

        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(Patient patient)
        {
            try
            {
				Patient p1 = _ipatientservice.getpatientinfo(getuser());
				var user = await userManager.FindByIdAsync(p1.userId);
				user.FirstName = patient.user.FirstName;
				user.LastName = patient.user.LastName;
				user.Gender = patient.user.Gender;
				user.PhoneNumber = patient.user.PhoneNumber;
				await userManager.UpdateAsync(user);
                
				return RedirectToAction("Index","Home", new { Area = "" });
            }
            catch
            {
                return View();
            }
        }

        private int getuser()
        {
            return _iuserservice.getid();
        }
    }
}