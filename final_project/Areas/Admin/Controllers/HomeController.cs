using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using models;
using Services;

namespace final_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class HomeController : Controller
    {
        // GET: HomeController
        public Ipatientservice _ipatientservice { get; }
        public Iuserservice _iuserservice { get; }


        public HomeController(Ipatientservice ipatientservice, Iuserservice iuserservice)
        {
            _ipatientservice = ipatientservice;
            _iuserservice = iuserservice;
        }

        public ActionResult Index()
        {
             ViewBag.counts= _iuserservice.getcounts();
            List<Patient> patients = _ipatientservice.getall();
            return View(patients);
        }

        public ActionResult Details(int id)
        {
            var pdetails = _ipatientservice.getpatientinfo(id);
            return View(pdetails);
        }
        public IActionResult Delete(int id)
        {
            var p = _ipatientservice.getpatientinfo(id);
            if (p != null)
            {
                _ipatientservice.Delete(id);
            }
            else
            {
                return NotFound();
            }
            return View(nameof(Index));
        }



     
    }
}
