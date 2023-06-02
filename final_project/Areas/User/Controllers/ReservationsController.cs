using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using models;
using Services;

namespace final_project.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles= "Patient")]

    public class ReservationsController : Controller
    {
        public Ireservationservice _ireservationservice { get; }
        public Iclinicservice _iclinicservice { get; }
        public Iuserservice _iuserservice { get; }
        public Ischaduleservice _ischaduleservice { get; }
        public ReservationsController(Ireservationservice ireservationservice,
                                      Iclinicservice iclinicservice,
                                      Iuserservice iuserservice,
                                      Ischaduleservice ischaduleservice)
        {
            _ireservationservice = ireservationservice;
            _iclinicservice = iclinicservice;
            _ischaduleservice = ischaduleservice;
            _iuserservice = iuserservice;
        }

        //// GET: ReservationsController

        public ActionResult GetPatientReservation()
        {
            List<Appointment> reservationlist = _ireservationservice.getpatientreservation(getuser());
            return View(reservationlist);
        }
        public ActionResult SetReservation(int clinic)
        {
            DateTime today = DateTime.Now;
            today = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
            ViewBag.clinic = _iclinicservice.GetClinic(clinic);
            ViewBag.date = today.ToString("yyyy-MM-dd");
            ViewBag.reservationlist = new SelectList(_ireservationservice.freereservation(clinic, today), "Time", "Time");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetReservation(int clinic, IFormCollection data)
        {
            DateTime date = Convert.ToDateTime(data["date"]);
            date = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            DateTime today = DateTime.Now;
            today = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
            ViewBag.clinic = _iclinicservice.GetClinic(clinic);
            if (date < today)
            {
                date = today;
            }
            ViewBag.date = date.ToString("yyyy-MM-dd");
            ViewBag.reservationlist = new SelectList(_ireservationservice.freereservation(clinic, date), "Time", "Time");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult submitReservation(int clinic, IFormCollection data)
        {
            try
            {
                Schadule sch = _ischaduleservice.getschadulebydate(clinic, Convert.ToDateTime(data["Date"]));
                Appointment appoint = new Appointment() { schaduleId = sch.ID, PatientId = getuser(), Time = data["Time"], Date = Convert.ToDateTime(data["Date"]), DoctorId = int.Parse(data["doctor"]), State = true, completed = false };
                if (_ireservationservice.bookappointment(appoint))
                {
                    return RedirectToAction("GetPatientReservation", "Reservations");
                }
                return View();
            }
            catch (Exception ex) { return View(); }

        }
        public ActionResult CancelReservation(int appointment)
        {
            try
            {
                if (_ireservationservice.cancelpatientreservation(appointment))
                {
                    return RedirectToAction("GetPatientReservation", "Reservations");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex) { return View(); }

        }
        public ActionResult ModifyReservation(int appointment)
        {
            Schadule sch = _ischaduleservice.getschadulebyappoint(appointment);
            ViewBag.reservationlist = new SelectList(_ireservationservice.freereservation(sch.ClinicId, sch.date), "Time", "Time");
            ViewBag.date = sch.date.ToString("yyyy-MM-dd");
            ViewBag.appointment = appointment;
            ViewBag.clinicobj = _iclinicservice.GetClinic(sch.ClinicId);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModifyReservation(int appointment, IFormCollection data)
        {

            DateTime date = Convert.ToDateTime(data["Date"]);
            date = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            DateTime today = DateTime.Now;
            today = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
            if (date < today)
            {
                return RedirectToAction("ModifyReservation", "Reservations", new { @appointment = appointment });
            }
            else
            {
                int clinic = int.Parse(data["clinic"]);
                ViewBag.date = date.ToString("yyyy-MM-dd");
                ViewBag.appointment = appointment;
                ViewBag.clinicobj = _iclinicservice.GetClinic(clinic);
                ViewBag.reservationlist = new SelectList(_ireservationservice.freereservation(clinic, date), "Time", "Time");
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult submitmodifyReservation(int appointment, IFormCollection data)
        {
            try
            {
                int clinic = int.Parse(data["clinic"]);
                DateTime date = Convert.ToDateTime(data["Date"]);
                Appointment newappoint = new Appointment() { PatientId = getuser(), Time = data["Time"], Date = Convert.ToDateTime(data["Date"]), DoctorId = int.Parse(data["doctor"]), State = true, completed = false };
                if (_ireservationservice.updateappointment(newappoint, appointment, clinic, date))
                {
                    return RedirectToAction("GetPatientReservation", "Reservations");
                }
                return View();
            }
            catch (Exception ex) { return View(); }

        }
        private int getuser()
        {
            return _iuserservice.getid();
        }
    }

}
