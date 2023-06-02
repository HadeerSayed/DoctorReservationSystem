using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using models;
using Services;

namespace final_project.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    [Authorize(Roles = "Doctor")]
    public class SchaduleController : Controller
    {
        public Ischaduleservice _ischaduleservice { get; }
        public Iclinicservice _iclinicservice { get; }
        public Iuserservice _iuserservice { get; }
        public Ireservationservice _ireservationservice { get; }
        public SchaduleController(Ischaduleservice ischaduleservice, Iclinicservice iclinicservice, Iuserservice iuserservice, Ireservationservice ireservationservice)
        {
            _ischaduleservice = ischaduleservice;
            _iclinicservice = iclinicservice;
            _iuserservice = iuserservice;
            _ireservationservice = ireservationservice;
        }
        public ActionResult AllSchadules()
        {
            List<Clinic> clinics = _iclinicservice.GetClinicList(getuser());
            if (clinics.Count() != 0)
            {
                ViewBag.clinics = new SelectList(clinics, "ID", "Name");
                List<Schadule> schadules = _ischaduleservice.getschadulelist(clinics.ElementAt(0).ID);
                return View(schadules);
            }
            else
            {
                return View();
            }

            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AllSchadules(IFormCollection clinic)
        {
            List<Clinic> clinics = _iclinicservice.GetClinicList(getuser());
            ViewBag.clinics = new SelectList(clinics, "ID", "Name", int.Parse(clinic["ID"]));
            List<Schadule> schadules = _ischaduleservice.getschadulelist(int.Parse(clinic["ID"]));
            return View(schadules);
        }
        public ActionResult SetSchadule()
        {
            try
            {
                List<Clinic> clinics = _iclinicservice.GetClinicList(getuser());
                ViewBag.clinics = new SelectList(clinics, "ID", "Name");
                if (TempData["message"] != null)
                    ViewBag.success = int.Parse(TempData["message"].ToString());
                else ViewBag.success = 0;
                return View();
            }
            catch
            {
                return NotFound();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetSchadule([Bind("starttime,endtime,ClinicId,duration,date")] Schadule schadule)
        {
            List<Clinic> clinics = _iclinicservice.GetClinicList(getuser());
            ViewBag.clinics = new SelectList(clinics, "ID", "Name");
            try
            {
                DateTime now = DateTime.Now;
                now = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                if (schadule.ClinicId != null && schadule.starttime != null && schadule.endtime != null && schadule.duration != null && schadule.date >= now)
                {
                    int.TryParse(schadule.starttime.Substring(0, 2), out int shour);
                    int.TryParse(schadule.starttime.Substring(3, 2), out int smin);
                    int.TryParse(schadule.endtime.Substring(0, 2), out int ehour);
                    int.TryParse(schadule.endtime.Substring(3, 2), out int emin);
                    if (shour < ehour || shour == ehour && smin < emin)
                    {
                        if (!_ischaduleservice.SchaduleExist(schadule.date, schadule.ClinicId))
                        {
                            _ischaduleservice.setschadule(schadule);
                            TempData["message"] = 1;
                            return RedirectToAction("SetSchadule", "Schadule");
                        }
                        ViewBag.success = 2;
                        return View();
                    }

                    ViewBag.success = -1;
                    return View();
                }
                ViewBag.success = -1;
                return View();
            }
            catch
            {
                ViewBag.success = -1;
                return View();
            }
        }
        public ActionResult EditSchadule(int schadule)
        {
                Schadule schadule1 = _ischaduleservice.getschadule(schadule);
                return View(schadule1);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSchadule(int schadule,[Bind("ID,starttime,endtime,duration")] Schadule schadule1)
        {
            try
            {
                DateTime now = DateTime.Now;
                now = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                schadule1.ID = schadule;
                if (schadule1.starttime != null && schadule1.endtime != null && schadule1.duration != null)
                {
                    int.TryParse(schadule1.starttime.Substring(0, 2), out int shour);
                    int.TryParse(schadule1.starttime.Substring(3, 2), out int smin);
                    int.TryParse(schadule1.endtime.Substring(0, 2), out int ehour);
                    int.TryParse(schadule1.endtime.Substring(3, 2), out int emin);
                    if (shour < ehour || shour == ehour && smin < emin)
                    {
                        if (_ischaduleservice.updateschadule(schadule1))
                        {
                            return RedirectToAction("AllSchadules", "Schadule");
                        }
                        else
                        {
                            return View();
                        }
                    }

                    ViewBag.success = -1;
                    return View();
                }
                ViewBag.success = -1;
                return View();
            }
            catch
            {
                ViewBag.success = -1;
                return View();
            }
        }
        public ActionResult DeleteSchadule(int schadule)
        {
                _ischaduleservice.deleteschadule(schadule);
                return RedirectToAction("AllSchadules", "Schadule");
        }
        private int getuser()
        {
            return _iuserservice.getid();
        }
    }
}