using final_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using models;
using Services;

namespace final_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class TitleController : Controller
    {
        public Ititleservice _ititleservice { get; }
        public TitleController(Ititleservice ititleservice)
        {
            _ititleservice = ititleservice;
        }
        // GET: titleController
        public ActionResult Alltitles()
        {
            return View(_ititleservice.titlies());
        }

        // GET: titleController/Create
        public ActionResult Addtitle()
        {
            return View();
        }

        // POST: titleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Addtitle(title title1)
        {
            try
            {

                if (_ititleservice.create(title1))
                {
                    return RedirectToAction("Alltitles", "title");

                }
                ViewBag.mess = "there are same title";
                return View();
            }
            catch
            {
                ViewBag.mess = "there are error";

                return View();
            }
        }

        // GET: titleController/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.titledoc= _ititleservice.GetTitle(id).Title;
            return View();
        }

        // POST: titleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                title title=new title() {ID=id, Title = collection["Title"] };
                if(_ititleservice.update(title))
                    return RedirectToAction("Alltitles");
                else
                {
                    ViewBag.title = _ititleservice.GetTitle(id).Title;
                    ViewBag.mess = "there are same title";

                    return View();
                }
            }
            catch
            {
                ViewBag.title = _ititleservice.GetTitle(id).Title;
                ViewBag.mess = "there are error";

                return View();
            }
        }

		public ActionResult Delete(int id)
		{
			if (_ititleservice.delete(id))
				return RedirectToAction("Alltitles");
			else
			{
				TempData["message"] = "can't delete this title";
				return RedirectToAction("Alltitles");
			}
		}
	}
}
