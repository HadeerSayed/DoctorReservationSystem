using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using models;
using Newtonsoft.Json.Linq;
using Services;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace final_project.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    [Authorize(Roles = "Doctor")]
    public class ReservationsController : Controller
    {
        public Ireservationservice _ireservationservice { get; }
        public Iclinicservice _iclinicservice { get; }
        public Iuserservice _iuserservice { get; }
        public Ischaduleservice _ischaduleservice { get; }
        public Ipatientservice _ipatientservice { get; }
        public ReservationsController(Ireservationservice ireservationservice,
                                      Iclinicservice iclinicservice,
                                      Iuserservice iuserservice,
                                      Ischaduleservice ischaduleservice,
                                      Ipatientservice ipatientservice)
        {
            this._ireservationservice= ireservationservice;
            this._iclinicservice = iclinicservice;
            this._iuserservice = iuserservice;
            this._ischaduleservice = ischaduleservice;
            _ipatientservice = ipatientservice;
        }
        public ActionResult Reservation()
        {
            List<Clinic> clinics = _iclinicservice.GetClinicList(getuser());
            DateTime date1;
            ViewBag.clinics = new SelectList(clinics, "ID", "Name", clinics.ElementAt(0).ID);
            date1 = DateTime.Now;
            date1 = new DateTime(date1.Year, date1.Month, date1.Day, 0, 0, 0);
            ViewBag.date = date1.ToString("yyyy-MM-dd");
            List<Appointment> reservationlist = _ireservationservice.reservation(clinics.ElementAt(0).ID, date1);
            return View(reservationlist);
        }
        [HttpPost]
        public ActionResult Reservation(IFormCollection collection)
        {
            List<Clinic> clinics = _iclinicservice.GetClinicList(getuser());
            DateTime date1;
            ViewBag.clinics = new SelectList(clinics, "ID", "Name",int.Parse(collection["clinic"]));
            if (collection["date"] != "" && Convert.ToDateTime(collection["date"]) > DateTime.Now)
            {
                date1 = Convert.ToDateTime(collection["date"].ToString());
            }
            else
            {
                date1 = DateTime.Now;
            }
            date1 = new DateTime(date1.Year, date1.Month, date1.Day, 0, 0, 0);
            ViewBag.date = date1.ToString("yyyy-MM-dd");
            List<Appointment> reservationlist = _ireservationservice.reservation(int.Parse(collection["clinic"]), date1);
            return View(reservationlist);
        }
        public ActionResult CancelReservation(int appointment, string? date1, int? clinic1)
        {
            try
            {
                if (_ireservationservice.cancelreservation(appointment))
                {
                    return RedirectToAction("Reservation", "Reservations", new { date = date1, clinic = clinic1 });
                }
                else
                {
                    ViewBag.message = "you can't delete this reservation";
                    return RedirectToAction("Reservation", "Reservations", new { date = date1, clinic =clinic1});
                }
            }
            catch (Exception ex) {
                ViewBag.message = "you can't delete this reservation";
                return RedirectToAction("Reservation", "Reservations", new { date = date1, clinic = clinic1});
            }

        }
        public ActionResult completed(int appointment, string? date1, int? clinic1)
        {
            try
            {
                if (_ireservationservice.completedreservation(appointment))
                {
                    return RedirectToAction("Reservation", "Reservations", new { date = date1, clinic = clinic1 });
                }
                else
                {
                    ViewBag.message = "you can't delete this reservation";
                    return RedirectToAction("Reservation", "Reservations", new { date =date1, clinic = clinic1 });
                }
            }
            catch (Exception ex) {
                ViewBag.message = "you can't delete this reservation";
                return RedirectToAction("Reservation", "Reservations", new { date = date1, clinic = clinic1 });
            }

        }
        public ActionResult Reappointment(int clinic1,int patient)
        {
            Patient p = _ipatientservice.getpatientinfo(patient);
            List<Schadule> sch = _ischaduleservice.getreappointschadulelist(clinic1);
            var sch2 = sch.Select(s => new
            {
                Value = s.ID,
                Text = s.date.Year + "-" + s.date.Month + "-" + s.date.Day
            });
            ViewBag.list = new SelectList(sch2, "Value", "Text");
            ViewBag.patient = p;
            ViewBag.clinic = clinic1;
            DateTime date1 = Convert.ToDateTime(sch[0].date);
            date1 = new DateTime(date1.Year, date1.Month, date1.Day, 0, 0, 0);
            List<Appointment> freeslot = _ireservationservice.freereservation(clinic1, date1);
            ViewBag.slot = new SelectList(freeslot.Select(s => new{ Value = s.schaduleId + "-" + s.Time,
                                                                    Text = s.Time}), "Value", "Text");
            return View();
        }
        [HttpPost]
        public ActionResult Reappointment(IFormCollection collection)
        {
            Patient p = _ipatientservice.getpatientinfo(int.Parse(collection["patient"]));
            List<Schadule> sch = _ischaduleservice.getreappointschadulelist(int.Parse(collection["clinic"]));
            var sch2 = sch.Select(s => new
            {
                Value = s.ID,
                Text = s.date.Year + "-" + s.date.Month + "-" + s.date.Day
            });
            ViewBag.list = new SelectList(sch2, "Value", "Text");
            ViewBag.patient = p;
            ViewBag.clinic = int.Parse(collection["clinic"]);
            if (int.Parse(collection["ID"]) != null&& int.Parse(collection["ID"])!=0)
            {
                var t = sch.FirstOrDefault(p => p.ID == int.Parse(collection["ID"])).date;
                DateTime date1 = new DateTime(t.Year, t.Month, t.Day, 0, 0, 0);
                List<Appointment> freeslot = _ireservationservice.freereservation(int.Parse(collection["clinic"]), date1);
                ViewBag.slot = new SelectList(freeslot.Select(s => new
                {
                    Value =s.schaduleId+"-"+s.Time ,
                    Text = s.Time
                }), "Value", "Text");
            }
            return View();
        }
        [HttpPost]
        public ActionResult submitReappointment(IFormCollection collection)
        {
            Appointment appoint = new Appointment() {
                completed = false,
                State = true,
                PatientId = int.Parse(collection["PatientId"]),
                DoctorId = getuser(),
                Time = collection["schid"].ToString().Split('-')[1],
                schaduleId = int.Parse(collection["schid"].ToString().Split('-')[0])
            };
            _ireservationservice.addreservation(appoint);
            return RedirectToAction("Reservation", "Reservations");
        }
        private int getuser()
        {
            return _iuserservice.getid();
        }

    }
}
