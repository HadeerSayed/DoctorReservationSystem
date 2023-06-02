using final_project.Models.Data;
using Microsoft.EntityFrameworkCore;
using models;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services
{
    public class clinicservice: Iclinicservice
    {
        public identityContext DB { get; }
        public clinicservice(identityContext DB)
        {
            this.DB = DB;
        }
        public List<Clinic> getAllClinics()
        {
            return DB.Clinics.Include(c => c.country).Include(c => c.Doctor).ThenInclude(p => p.user).ToList();
        }
        public Clinic GetClinic(int id)
        {
            try
            {
                return DB.Clinics.Include(p => p.Doctor).ThenInclude(p=>p.user).FirstOrDefault(p => p.ID == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<Clinic> GetClinicList(int doctor)
        {
            try
            {
                return DB.Clinics.Where(p => p.DoctorId == doctor).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public void AddDoctorClinic(Clinic clinic)
        {
            clinic.ID = 0;
            DB.Clinics.Add(clinic);
            DB.SaveChanges();
        }
        public List<Clinic> getDoctorClinics(int id)
        {
            return DB.Clinics.Include(p=>p.country).Where(c => c.DoctorId == id).ToList();
        }
        public Clinic GetSelectedClinic(int id)
        {
            return DB.Clinics.FirstOrDefault(c => c.ID == id);
        }
        public int? DeleteDoctorClinic(int id)
        {
            var c = GetSelectedClinic(id);
            DB.Clinics.Remove(c);
            DB.SaveChanges();
            return c.DoctorId;
        }
        public int? EditeDoctorClinic(int id, Clinic clinic)
        {
            var c = GetSelectedClinic(id);
            c.Name = clinic.Name;
            c.cost = clinic.cost;
            c.Phone = clinic.Phone;
            c.countryid = clinic.countryid;
            c.Address = clinic.Address;
            DB.SaveChanges();
            return c.DoctorId;
        }
        public List<searchresult> searchclinic(int? id, string? doctor, int number, int page,string? gender, string? department)
        {
            if (id != 0 && doctor != "")
            {
				if (gender != "" && department != "")
				{
                    return searchbyid_doc_gen_dept(id,doctor,number,page,gender,department);
				}
                else if (gender!=""&&department=="") {
					return searchbyid_doc_gen(id, doctor, number, page, gender);
				}
                else {
					return searchbyid_doc(id, doctor, number, page);
				}
			}
			if (id != 0 && doctor == "")
            {
				if (gender != "" && department != "")
				{
					return searchbyid_gen_dept(id, number, page, gender, department);
				}
				else if (gender != "" && department == "")
				{
					return searchbyid_gen(id, number, page, gender);
				}
				else
				{
					return searchbyid(id, number, page);
				}
			}
			if (id == 0 && doctor != "")
			{
				if (gender != "" && department != "")
				{
					return searchbydoc_gen_dept(doctor, number, page, gender, department);
				}
				else if (gender != "" && department == "")
				{
					return searchbydoc_gen(doctor, number, page, gender);
				}
				else
				{
					return searchbydoc(doctor, number, page);
				}
			}

            else
                return null;
		}

        private List<searchresult> searchbyid_doc_gen_dept(int? id, string? doctor, int number, int page, string gender, string department) {
			bool flag = Enum.TryParse<gender>(gender, out gender g);
            var d = department.Split(',');
			if (flag == true)
                return DB.Clinics.Where(p => p.countryid == id &&
                                           (p.Doctor.user.FirstName + " " + p.Doctor.user.LastName == doctor || p.Name == doctor) &&
                                           p.Doctor.user.Gender == g && d.Contains(p.Doctor.DepartmentId.ToString()))
                                                         .Skip(number * page).Take(number).Select(p => new searchresult()
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
            else
                return null;

		}
		private List<searchresult> searchbyid_doc_gen(int? id, string? doctor, int number, int page, string gender) {
			bool flag = Enum.TryParse<gender>(gender, out gender g);
            if (flag == true)
                return DB.Clinics.Where(p => p.countryid == id &&
                                           (p.Doctor.user.FirstName + " " + p.Doctor.user.LastName == doctor || p.Name == doctor) &&
                                           p.Doctor.user.Gender == g)
                                                         .Skip(number * page).Take(number).Select(p => new searchresult()
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
            else
                return null;
		}
		private List<searchresult> searchbyid_doc(int? id, string? doctor, int number, int page)
		{
				return DB.Clinics.Where(p => p.countryid == id &&
										   (p.Doctor.user.FirstName + " " + p.Doctor.user.LastName == doctor || p.Name == doctor))
								.Skip(number * page).Take(number).Select(p => new searchresult(){DoctorId = p.DoctorId,
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
                                    Phone = p.Phone}).ToList();
		}


		private List<searchresult> searchbyid_gen_dept(int? id,int number, int page, string gender, string department)
		{
			bool flag = Enum.TryParse<gender>(gender, out gender g);
			var d = department.Split(',');
			if (flag == true)
				return DB.Clinics.Where(p => p.countryid == id &&
										   (p.Doctor.user.Gender == g && d.Contains(p.Doctor.DepartmentId.ToString())))
														 .Skip(number * page).Take(number).Select(p => new searchresult()
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
			else
				return null;

		}
		private List<searchresult> searchbyid_gen(int? id, int number, int page, string gender)
		{
			bool flag = Enum.TryParse<gender>(gender, out gender g);
			if (flag == true)
				return DB.Clinics.Where(p => p.countryid == id  &&
										   p.Doctor.user.Gender == g)
														 .Skip(number * page).Take(number).Select(p => new searchresult()
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
			else
				return null;
		}
		private List<searchresult> searchbyid(int? id, int number, int page)
		{
			return DB.Clinics.Where(p => p.countryid == id )
							.Skip(number * page).Take(number).Select(p => new searchresult()
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

		private List<searchresult> searchbydoc_gen_dept(string? doctor, int number, int page, string gender, string department)
		{
			bool flag = Enum.TryParse<gender>(gender, out gender g);
			var d = department.Split(',');
			if (flag == true)
				return DB.Clinics.Where(p => (p.Doctor.user.FirstName + " " + p.Doctor.user.LastName == doctor || p.Name == doctor) &&
										   p.Doctor.user.Gender == g && d.Contains(p.Doctor.DepartmentId.ToString()))
														 .Skip(number * page).Take(number).Select(p => new searchresult()
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
			else
				return null;

		}
		private List<searchresult> searchbydoc_gen(string? doctor, int number, int page, string gender)
		{
			bool flag = Enum.TryParse<gender>(gender, out gender g);
			if (flag == true)
				return DB.Clinics.Where(p =>(p.Doctor.user.FirstName + " " + p.Doctor.user.LastName == doctor || p.Name == doctor) &&
										   p.Doctor.user.Gender == g)
														 .Skip(number * page).Take(number).Select(p => new searchresult()
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
			else
				return null;
		}
		private List<searchresult> searchbydoc(string? doctor, int number, int page)
		{
			return DB.Clinics.Where(p =>(p.Doctor.user.FirstName + " " + p.Doctor.user.LastName == doctor || p.Name == doctor))
							.Skip(number * page).Take(number).Select(p => new searchresult()
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
