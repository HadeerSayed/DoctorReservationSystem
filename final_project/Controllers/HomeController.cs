using final_project.Models;
using final_project.Services.Country;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services;
using System.Diagnostics;
using models;

namespace final_project.Controllers
{
    public class HomeController : Controller
    {
        private Iclinicservice _iclinicservice { get; }
        private Idoctorservice _idoctorservice { get; }
        public Icountryservice _icountryservice { get; }
        public Idepartmentservice _idepartmentservice { get;}
		public Ischaduleservice _ischaduleservice { get; }
        public Ifeedbackservice _ifeedbackservice { get; }

        public Iuserservice _iuserservice { get; }
		public Microsoft.AspNetCore.Identity.SignInManager<ApplicationUser> _signinmanager { get; }

		public HomeController(Idoctorservice idoctorservice,
                              Iclinicservice iclinicservice,
							  Ischaduleservice ischaduleservice,
							  Microsoft.AspNetCore.Identity.SignInManager<ApplicationUser> signinmanager,
							  Idepartmentservice idepartmentservice,
                              Iuserservice iuserservice,
                              Ifeedbackservice ifeedbackservice,
                              Icountryservice icountryservice)
        {
            _iclinicservice= iclinicservice;
            _idoctorservice= idoctorservice;
            _icountryservice= icountryservice;
            _idepartmentservice= idepartmentservice;
            _signinmanager= signinmanager;
			_ischaduleservice= ischaduleservice;
			_iuserservice = iuserservice;
            _ifeedbackservice= ifeedbackservice;
        }
		public IActionResult Index()
		{
			ViewBag.coutries = new SelectList(_icountryservice.countries(), "Id", "Name");
			ViewBag.departments = _idepartmentservice.getAllDepartment();
			ViewBag.clinics = _iclinicservice.getAllClinics();
			ViewBag.doctors = _idoctorservice.GetAllDoctor();
			ViewBag.feedbacks = _idoctorservice.GetAllDoctorwfeedback();
			return View();
		}
		public IActionResult AllDoctors(int? page, IFormCollection? collection)
		{
			ViewBag.searchdata = collection;
			string gender = collection["gender"].ToString();
			string department = collection["department"].ToString();
			ViewBag.departments = new SelectList(_idepartmentservice.getAllDepartment(), "ID", "Name");

			List<searchresult> doctors;
			if (collection["location"].ToString() != "" || collection["doctor"].ToString() != "")
			{
				int location = 0;
				if (collection["location"].ToString() != "")
					location = int.Parse(collection["location"].ToString());
				string doctor = collection["doctor"].ToString();
				if (page != null)
				{
					doctors = _iclinicservice.searchclinic(location, doctor, 3, (int)page, gender, department);
					ViewBag.currentpage = page;
				}
				else
				{
					doctors = _iclinicservice.searchclinic(location, doctor, 3, 0, gender, department);
					ViewBag.currentpage = 0;
				}
				return View(doctors);
			}
			else
			{
				if (page != null)
				{
					doctors = _idoctorservice.GetDoctors(3, (int)page, gender, department);
					ViewBag.currentpage = page;
				}
				else
				{
					doctors = _idoctorservice.GetDoctors(3, 0, gender, department);
					ViewBag.currentpage = 0;
				}
				return View(doctors);
			}
		}
		//public IActionResult AllDoctors(int? page, IFormCollection? collection)
		//{
		//	ViewBag.departments = new SelectList(_idepartmentservice.getAllDepartment(),"ID","Name");
		//	ViewBag.data = collection;
		//	List<searchresult> doctors;
		//	int location2 = 0;
		//	if (collection["location"].ToString() != "")
		//	{
		//		location2 = int.Parse(collection["location"].ToString());
		//	}
		//	if (collection["location"].ToString() != "" || collection["doctor"].ToString() != "")
		//	{
		//		if (page != null)
		//		{
		//			doctors = _iclinicservice.searchclinic(location2,collection["doctor"].ToString(), 3, (int)page, "", "");
		//			ViewBag.currentpage = page;
		//		}
		//		else
		//		{
		//			doctors = _iclinicservice.searchclinic(location2,collection["doctor"].ToString(), 3, 0, "", "");
		//			ViewBag.currentpage = 0;
		//		}
		//		return View(doctors);
		//	}
		//	else
		//	{
		//		if (page != null)
		//		{
		//			doctors = _idoctorservice.GetDoctors(3, (int)page, "", "");
		//			ViewBag.currentpage = page;
		//		}
		//		else
		//		{
		//			doctors = _idoctorservice.GetDoctors(3, 0, "", "");
		//			ViewBag.currentpage = 0;
		//		}
		//		return View(doctors);
		//	}
		//}
		//public IActionResult AllDoctors2(int? page, IFormCollection? collection)
		//{
		//	ViewBag.departments = new SelectList(_idepartmentservice.getAllDepartment(), "ID", "Name");
		//	ViewBag.data = collection;
		//	List<searchresult> doctors;
		//	int location2 = 0;
		//	if(collection["location"].ToString() != "")
		//		location2=int.Parse(collection["location"].ToString());
		//	if ((collection["gender"].ToString() != "" || collection["department"].ToString() != "")&&
		//		(collection["location"].ToString() != "" || collection["doctor"].ToString() != ""))
		//	{
		//		if (page != null)
		//		{
		//			doctors = _iclinicservice.searchclinic(location2, collection["doctor"].ToString(), 3, (int)page, collection["gender"].ToString(),collection["department"].ToString());
		//			ViewBag.currentpage = page;
		//		}
		//		else
		//		{
		//			doctors = _iclinicservice.searchclinic(location2, collection["doctor"].ToString(), 3, 0, collection["gender"].ToString(), collection["department"].ToString());
		//			ViewBag.currentpage = 0;
		//		}
		//		return View(doctors);
		//	}
		//	else
		//	{
		//		if (page != null)
		//		{
		//			doctors = _idoctorservice.GetDoctors(3, (int)page, collection["gender"].ToString(), collection["department"].ToString());
		//			ViewBag.currentpage = page;
		//		}
		//		else
		//		{
		//			doctors = _idoctorservice.GetDoctors(3, 0, collection["gender"].ToString(), collection["department"].ToString());
		//			ViewBag.currentpage = 0;
		//		}
		//		return View(doctors);
		//	}

		//}
		public IActionResult Profile(IFormCollection? collection)
		{
			if (collection.Count > 1)
			{
                Doctor d = _idoctorservice.getprofile(int.Parse(collection["doctor"]));
                Clinic c = _iclinicservice.GetClinic(int.Parse(collection["clinic"]));
                List<Schadule> s = _ischaduleservice.getschadulelist(int.Parse(collection["clinic"]));
                List<Feedback> f = _ifeedbackservice.getfeedbacks(int.Parse(collection["doctor"]));
                ViewBag.patient = getuser();
                ViewBag.schadulelist = s;
                ViewBag.clinic = c;
                ViewBag.doctor = d;
                ViewBag.feedback = f;
                return View("Profile");
            }
			return RedirectToAction("AllDoctors");

		}
        public IActionResult setfeedback(IFormCollection collection)
		{
			Feedback feedback = new Feedback() { Comment = collection["comment"],
												DoctorId = int.Parse(collection["doctor"]),
												PatientId=getuser(),
												Rate = collection["rate"].ToString().Split(',').Max()
												};
			_ifeedbackservice.addfeedback(feedback);
			return Profile(collection);
        }
        public IActionResult Delete(int id, IFormCollection collection)
		{
            _ifeedbackservice.removefeedback(id);
			return Profile(collection);
        }
        private int getuser()
		{
			return _iuserservice.getid();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}