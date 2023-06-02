using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using models;
using Services;
using final_project.Models;
using final_project.Areas.Identity.Pages.Account;
using System.Net.Mail;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using final_project.Services.Country;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class DoctorController : Controller
    {
        public Idoctorservice _idoctorservice { get; }
        public Iclinicservice _iclinicservice { get; }
        public Idepartmentservice _idepartmentservice { get; }
        public Icountryservice _icountryservice { get; }
        public Ititleservice _ititleservice { get; }
        private readonly IEmailSender _emailSender;

        public Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager { get; }
        public IHostingEnvironment hosting { get; set; }

        private readonly ILogger<RegisterModel> _logger;

        public DoctorController(ILogger<RegisterModel> logger,
                                Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager,
                                IEmailSender emailSender,
                                Idoctorservice idoctorservice,
                                Idepartmentservice idepartmentservice,
                                Iclinicservice iclinicservice,
                                Icountryservice icountryservice,
                                Ititleservice ititleservice,
                                IHostingEnvironment Hosting)
        {
            _idoctorservice= idoctorservice;
            _idepartmentservice= idepartmentservice;
            userManager = _userManager;
            _logger = logger;
            hosting = Hosting;
            _ititleservice = ititleservice;
            _iclinicservice= iclinicservice;
            _icountryservice = icountryservice;
            _emailSender = emailSender;
        }
        public ActionResult AllDoctors()
        {
            return View(_idoctorservice.GetAllDoctor());
        }

        // GET: DoctorController/Create

        public ActionResult CreateDoctor()
        {
            ViewBag.DepartmentId = new SelectList(_idepartmentservice.getAllDepartment(), "ID", "Name");
            ViewBag.TitleId = new SelectList(_ititleservice.titlies(), "ID", "Title");
            ViewBag.CountryId = new SelectList(_icountryservice.countries(), "Id", "Name");
            return View();
        }

        // POST: DoctorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateDoctorAsync(DoctorClinic dc)
        {
            try
            {
                string userId="";
                #region identity
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = new MailAddress(dc.Email).User,
                    FirstName =dc.FirstName ,
                    LastName = dc.LastName,
                    Email = dc.Email,
                    PhoneNumber = dc.DoctorPhone,
                };
                #region doctorinfo
                var result = await userManager.CreateAsync(user,dc.Password.ToString());
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    userId = await userManager.GetUserIdAsync(user);
                    await userManager.AddToRoleAsync(user, "Doctor");
                }
                #endregion
                string fileName = string.Empty;
                if (dc.imageFile != null)
                {
                    string uploads = Path.Combine(hosting.WebRootPath, "uploads");
                    fileName = dc.imageFile.FileName;
                    string fullPath = Path.Combine(uploads, fileName);
                    dc.imageFile.CopyTo(new FileStream(fullPath, FileMode.Create));
                }
                Doctor doctor=new Doctor()
                {
                    Image = fileName,
                    DepartmentId= dc.DepartmentId,
                    userId= userId,
                    about=dc.about,
                    titleid=dc.titleid
                };
                int doctorid = _idoctorservice.insertDoctor(doctor);
                #endregion
                #region clinic
                Clinic clinic = new Clinic();
                clinic.Address = dc.ClinicAddress;
                clinic.countryid = dc.countryid;
                clinic.Name = dc.ClinicName;
                clinic.cost = dc.cost;
                clinic.Phone = dc.ClinicPhone;
                clinic.DoctorId = doctorid;
                clinic.Longitude= dc.Longitude;
                _iclinicservice.AddDoctorClinic(clinic);
                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var returnUrl = Url.Content("~/");
                var callbackUrl = Url.Page(
                    pageName: "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = user.Id, code = code, returnurl = returnUrl },
                    protocol: Request.Scheme);
                await _emailSender.SendEmailAsync(dc.Email, "confirm your email", $"please confirm your account <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'> click here</a>");

                #endregion
                return RedirectToAction();
            }
            catch
            {
                return View();
            }
        }
        public ActionResult DetailsDoctor(int id)
        {
            dclinic dc = new dclinic();
            dc.doctor = _idoctorservice.getDetailes(id);
            dc.clinic = _iclinicservice.getDoctorClinics(id);
            return View(dc);
        }
        public ActionResult Delete(int id)
        {
            _idoctorservice.DeleteDoctor(id);
            return RedirectToAction("AllDoctors");
        }
    }
}
