using final_project.Models;
using final_project.Services.Country;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using models;
using Services;

namespace final_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class CountryController : Controller
    {
        public Icountryservice _icountryservice { get; }
        public CountryController(Icountryservice icountryservice)
        {
            _icountryservice = icountryservice;
        }
        // GET: countryController
        public ActionResult Allcountrys()
        {
            return View(_icountryservice.countries());
        }

        // GET: countryController/Create
        public ActionResult Addcountry()
        {
            return View();
        }

        // POST: countryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Addcountry(country country)
        {
            try
            {

                if (_icountryservice.create(country))
                {
                    return RedirectToAction("Allcountrys", "country");

                }
                ViewBag.mess = "there are same country";
                return View();
            }
            catch
            {
                ViewBag.mess = "there are error";
                return View();
            }
        }

        // GET: countryController/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.country=_icountryservice.GetCountry(id);
            return View();
        }

        // POST: countryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                country country=new country() {Id=id, Name = collection["Name"] };
                if (_icountryservice.update(country))
                    return RedirectToAction("Allcountrys");
                else
                {
                    ViewBag.country = _icountryservice.GetCountry(id);
                    ViewBag.mess = "there are same country";
                    return View();
                }
            }
            catch
            {
                ViewBag.country = _icountryservice.GetCountry(id);
                ViewBag.mess = "there are error";

                return View();
            }
        }

        // GET: countryController/Delete/5
        public ActionResult Delete(int id)
        {
            if(_icountryservice.delete(id))
                return RedirectToAction("Allcountrys");
            else
            {
                TempData["message"] = "can't delete this country";
                return RedirectToAction("Allcountrys");
            }
        }

    }
}
