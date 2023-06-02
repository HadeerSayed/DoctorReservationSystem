using final_project.Services.Country;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using models;
using Services;

namespace final_project.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    [Authorize(Roles = "Doctor")]
    public class ClinicController : Controller
    {
        public Ireservationservice _ireservationservice { get; }
        public Iclinicservice _iclinicservice { get; }
        public Iuserservice _iuserservice { get; }
        public Ischaduleservice _ischaduleservice { get; }
        public Ipatientservice _ipatientservice { get; }
        public Idoctorservice _idoctorservice { get; }
        public Icountryservice _icountryservice { get; }
        public Idepartmentservice _idepartmentservice { get; }


        public ClinicController(Ireservationservice ireservationservice,
                                      Iclinicservice iclinicservice,
                                      Iuserservice iuserservice,
                                      Ischaduleservice ischaduleservice,
                                      Ipatientservice ipatientservice,
                                      Icountryservice icountryservice,
                                      Idoctorservice idoctorservice)
        {
            this._ireservationservice = ireservationservice;
            this._iclinicservice = iclinicservice;
            this._iuserservice = iuserservice;
            this._ischaduleservice = ischaduleservice;
            _ipatientservice = ipatientservice;
            _idoctorservice = idoctorservice;
            _icountryservice = icountryservice;
        }
        public ActionResult ViewClincs()
        {
            return View(_iclinicservice.getDoctorClinics(getuser()));
        }
        public ActionResult EditeClincs(int id)
        {
            ViewBag.countryid = new SelectList(_icountryservice.countries(), "Id", "Name",id);
            return View(_iclinicservice.GetSelectedClinic(id));

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditeClincs(int id, Clinic clinic)
        {
            ViewBag.countryid = new SelectList(_icountryservice.countries(), "Id", "Name");
            try
            {
                int? docID = _iclinicservice.EditeDoctorClinic(id, clinic);
                return RedirectToAction("ViewClincs", new { id = docID });
            }
            catch
            {
                return View();
            }
        }
        public ActionResult DeleteClincs(int id)
        {
            int? docID = _iclinicservice.DeleteDoctorClinic(id);
            return RedirectToAction("ViewClincs", new { id = docID });
        }
        public ActionResult AddClinic()
        {
            ViewBag.countryid = new SelectList(_icountryservice.countries(), "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddClinic(Clinic c)
        {
            try
            {
                c.DoctorId = getuser();
                _iclinicservice.AddDoctorClinic(c);
                return RedirectToAction("ViewClincs");
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
