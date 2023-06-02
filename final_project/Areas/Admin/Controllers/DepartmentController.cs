using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using models;
using Services;

namespace final_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class DepartmentController : Controller
    {
        public Idepartmentservice _idepartmentservice { get; }
        public DepartmentController(Idepartmentservice idepartmentservice)
        {
            _idepartmentservice = idepartmentservice;
        }
        // GET: DepartmentController
        public ActionResult AllDepartments()
        {

            return View(_idepartmentservice.getAllDepartment());
        }

        // GET: DepartmentController/Create
        public ActionResult AddDepartment()
        {
            return View();
        }

        // POST: DepartmentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDepartment(Department department)
        {
            try
            {

                if (_idepartmentservice.createdepartment(department))
                {
                    return RedirectToAction("AllDepartments", "Department");

                }
                ViewBag.mess = "there are same department";
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: DepartmentController/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.Dept = _idepartmentservice.get(id);
            return View();
        }

        // POST: DepartmentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                Department dept=new Department() {ID=id, Name = collection["Name"] };
                if(_idepartmentservice.edit(dept))
                    return RedirectToAction("AllDepartments");
                else
                {
                    ViewBag.Dept = _idepartmentservice.get(id);
                    ViewBag.mess = "there are same department";
                    return View();
                }
                    
            }
            catch
            {

                ViewBag.Dept = _idepartmentservice.get(id);
                ViewBag.mess = "there are error!";
                return View();
            }
        }

        // GET: DepartmentController/Delete/5
        public ActionResult Delete(int id)
        {
            if(_idepartmentservice.delete(id))
                return RedirectToAction("AllDepartments");
            else
            {
                TempData["message"] = "can't delete this department";
                return RedirectToAction("AllDepartments");
            }
        }

    }
}
