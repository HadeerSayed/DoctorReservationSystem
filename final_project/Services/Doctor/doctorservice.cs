using final_project.Models;
using final_project.Models.Data;
using Microsoft.EntityFrameworkCore;
using models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Services
{
    public class doctorservice : Idoctorservice
    {
        public identityContext DB { get; }
        public Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager { get; }
        public IHostingEnvironment hosting { get; set; }
        public doctorservice(identityContext DB,
            Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager, IHostingEnvironment Hosting)
        {
            this.DB = DB;
            this.userManager = _userManager;
            this.hosting = Hosting;
        }
        public int insertDoctor(Doctor doctor)
        {
            DB.Doctors.Add(doctor);
            DB.SaveChanges();

            return doctor.ID;
        }
        public List<Doctor> GetAllDoctor()
        {
            return DB.Doctors.Include(p => p.user).Include(p=>p.Department).ToList();
        }
		public List<Doctor> GetAllDoctorwfeedback()
        {
			return DB.Doctors.OrderByDescending(p=>p.globalrate).Take(10).ToList();
		}
		public Doctor getDetailes(int id)
        {
            return DB.Doctors.Include(p=>p.user).Include(p=>p.Title).Include(d => d.Department).FirstOrDefault(d => d.ID == id);

        }
		public Doctor getprofile(int id)
		{
			return DB.Doctors.Include(p => p.user).Include(p => p.Title).Include(d => d.Department).FirstOrDefault(d => d.ID == id);

		}
		public void DeleteDoctor(int id)
        {
            var d = getDetailes(id);
            DB.Doctors.Remove(d);
            DB.SaveChanges();
        }
        public async void UpdateDoctor(int id, Doctor doctor)
        {
            Doctor doctor1 = DB.Doctors.Include(p=>p.user).FirstOrDefault(d => d.ID == id);
			var user = await userManager.FindByIdAsync(doctor1.userId);
            user.FirstName = doctor1.user.FirstName;
            user.LastName = doctor1.user.LastName;
            user.Gender = doctor.user.Gender;
			user.PhoneNumber = doctor.user.PhoneNumber;

            doctor1.titleid= doctor.Title.ID;
            doctor1.about=doctor.about;
            DB.Doctors.Update(doctor1);
			DB.SaveChanges();
			userManager.UpdateAsync(user);
        }
        public bool changephoto(int id, IFormFile photo)
        {
            try
            {
                Doctor doctor1 = DB.Doctors.Include(p => p.user).FirstOrDefault(d => d.ID == id);
                string fileName = string.Empty;
                if (photo != null)
                {
                    string uploads = Path.Combine(hosting.WebRootPath, "uploads");
                    fileName = photo.FileName;
                    string fullPath = Path.Combine(uploads, fileName);
                    //delete old image
                    string oldfilename = DB.Doctors.FirstOrDefault(d => d.ID == id).Image;
                    string fullOldPath = Path.Combine(uploads, oldfilename);
                    if (fullPath != fullOldPath)
                    {
                        // System.IO.File.Delete(fullOldPath);
                        photo.CopyTo(new FileStream(fullPath, FileMode.Create));

                    }
                    doctor1.Image = fileName;

                }
                else
                {
                    doctor1.Image = doctor1.Image;

                }
                DB.Doctors.Update(doctor1);
                DB.SaveChanges();
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }
        public List<Feedback> GetFeedback(int id)
        {
            return DB.Feedbacks.Include(f => f.patient).ThenInclude(p=>p.user).Where(f => f.DoctorId == id).ToList();
        }
        public bool updatepassword(ChangePassword p)
        {
            Doctor d = DB.Doctors.Include(p => p.user).FirstOrDefault(d => d.ID == p.Id);
            ApplicationUser user = new ApplicationUser()
            {
                Id = d.userId,

            };
            var result = userManager.ChangePasswordAsync(user, p.OldPassword, p.NewPassword);
            return true;
        }
        public List<searchresult> GetDoctors(int number, int page,string gender,string department)
        {
            if (department != "" && gender != "")
            {
                bool flag = Enum.TryParse(gender, out gender g);
                if (flag) {
                    var d = department.Split(',');
					return DB.Clinics.Where(p=>p.Doctor.user.Gender==g&&d.Contains(p.Doctor.DepartmentId.ToString())).Skip(number * page).Take(number).Select(p => new searchresult()
					{
						DoctorId = p.DoctorId,
						ID = p.ID,
						Name = p.Name,
						FirstName = p.Doctor.user.FirstName,
						LastName = p.Doctor.user.LastName,
						Title = p.Doctor.Title.Title,
						image = p.Doctor.Image,
						Department = p.Doctor.Department.Name,
						gender = p.Doctor.user.Gender,
						country = p.country.Name,
						Address = p.Address,
						cost = p.cost,
						Phone = p.Phone
					}).ToList();
				}
                return null;
            }
            else if(department == "" && gender != "")
			{
				bool flag = Enum.TryParse(gender, out gender g);
                if (flag)
                {
                    return DB.Clinics.Where(p => p.Doctor.user.Gender == g).Skip(number * page).Take(number).Select(p => new searchresult()
                    {
                        DoctorId = p.DoctorId,
                        ID = p.ID,
                        Name = p.Name,
                        FirstName = p.Doctor.user.FirstName,
                        LastName = p.Doctor.user.LastName,
                        Title = p.Doctor.Title.Title,
                        image = p.Doctor.Image,
                        Department = p.Doctor.Department.Name,
                        gender = p.Doctor.user.Gender,
                        country = p.country.Name,
                        Address = p.Address,
                        cost = p.cost,
                        Phone = p.Phone
                    }).ToList();
                }
                return null;
			}
            else
            {
				return DB.Clinics.Skip(number * page).Take(number).Select(p => new searchresult()
				{
					DoctorId = p.DoctorId,
					ID = p.ID,
					Name = p.Name,
					FirstName = p.Doctor.user.FirstName,
					LastName = p.Doctor.user.LastName,
					Title = p.Doctor.Title.Title,
					image = p.Doctor.Image,
					Department = p.Doctor.Department.Name,
					gender = p.Doctor.user.Gender,
					country = p.country.Name,
					Address = p.Address,
					cost = p.cost,
					Phone = p.Phone
				}).ToList();
			}
		}
    }
}
