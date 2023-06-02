using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using models;
using Services;
using System.Numerics;

namespace Controllers
{
    [Area("Doctor")]
    [Authorize(Roles = "Doctor")]
    public class HomeController : Controller
    {

        public Iuserservice _iuserservice { get; }
        public Idepartmentservice _idepartmentservice { get; }
        public Idoctorservice _idoctorservice { get; }
        public Iclinicservice _iclinicservice { get; }
        public Ifeedbackservice _ifeedbackservice { get; }

		public Ititleservice _ititleservice { get; }

		public HomeController(Iuserservice iuserservice,
                              Idoctorservice idoctorservice,
                              Iclinicservice iclinicservice,
                              Ifeedbackservice feedbackservices,
							  Ititleservice titleservices,
                              Idepartmentservice idepartmentservice)
        {
            _iuserservice = iuserservice;
            _idoctorservice = idoctorservice;
            _idepartmentservice = idepartmentservice;
            _iclinicservice= iclinicservice;
            _ifeedbackservice= feedbackservices;
            _ititleservice= titleservices;
        }
        public ActionResult Details()
        {
            List<Clinic> c = _iclinicservice.GetClinicList(getuser());
            List<Feedback> f = _ifeedbackservice.getfeedbacks(getuser());
            ViewBag.clinic = c;
            ViewBag.feedback = f;
            return View(_idoctorservice.getDetailes(getuser()));
        }

        public ActionResult Edit()
        {
            ViewBag.Titles = new SelectList(_ititleservice.titlies(), "ID", "Title");
			ViewBag.DepartmentId = new SelectList(_idepartmentservice.getAllDepartment(), "ID", "Name");
            return View(_idoctorservice.getDetailes(getuser()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Doctor doctor)
        {
            try
            {
                _idoctorservice.UpdateDoctor(getuser(), doctor);
                return RedirectToAction(nameof(Details));
            }
            catch
            {
				ViewBag.Titles = _ititleservice.titlies();
				ViewBag.DepartmentId = new SelectList(_idepartmentservice.getAllDepartment(), "ID", "Name");
				return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> ChangePhoto(Doctor doc)
        {
            _idoctorservice.changephoto(getuser(),doc.imageFile);
            return RedirectToAction(nameof(Details));
        }
        public ActionResult showfeedback()
        {
            return View(_idoctorservice.GetFeedback(getuser()));
        }
        public ActionResult EditPassword()
        {
            ChangePassword p = new ChangePassword();
            p.Id = getuser();
            return View(p);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPassword(ChangePassword p)
        {
            try
            {
                if (_idoctorservice.updatepassword(p))
                {
                    return RedirectToAction("Details","Home");
                }
                else
                {
                    ModelState.AddModelError("OldPassword", "old passwoed isn correct");
                    return View(p);
                }

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
