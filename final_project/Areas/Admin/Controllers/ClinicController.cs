using final_project.Services.Country;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using models;
using Services;

namespace final_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ClinicController : Controller
    {
        public Iclinicservice _iclinicservice { get; }
        public Iuserservice _iuserservice { get; }

        public Icountryservice _icountryservice { get; }

        public Idoctorservice _idoctorservice { get; }

        public ClinicController(Ireservationservice ireservationservice,
                                   Iclinicservice iclinicservice,
                                   Iuserservice iuserservice,
                                   Ischaduleservice ischaduleservice,
                                   Ipatientservice ipatientservice,
                                   Icountryservice icountryservice,
                                   Idoctorservice idoctorservice)
        {

            this._iclinicservice = iclinicservice;
            this._iuserservice = iuserservice;
            _idoctorservice = idoctorservice;


            _icountryservice = icountryservice;
        }
        public ActionResult AllClinics()
        {
            return View(_iclinicservice.getAllClinics());

        }

        public ActionResult AddClinic()
        {
            ViewBag.countryid = new SelectList(_icountryservice.countries(), "Id", "Name");
            var d = _idoctorservice.GetAllDoctor().Select(p => new
            {
                Text = p.ID + "" + p.user.FirstName + p.user.LastName,
                Value = p.ID
            });
            ViewBag.doctorid = new SelectList(d, "Value", "Text");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddClinic(Clinic c)
        {
            try
            {
             
                _iclinicservice.AddDoctorClinic(c);
                return RedirectToAction("AllClinics");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult DeleteClincs(int id)
        {
            _iclinicservice.DeleteDoctorClinic(id);
            return RedirectToAction("AllClinics");
        }
        private int getuser()
        {
            return _iuserservice.getid();
        }
    }
}
