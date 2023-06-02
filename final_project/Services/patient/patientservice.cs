using final_project.Models.Data;
using Microsoft.EntityFrameworkCore;
using models;
namespace Services
{
    public class patientservice:Ipatientservice
    {
        public identityContext DB { get; }
        public patientservice(identityContext DB)
        {
            this.DB = DB;
        }
        public int setpatient(string id) {
            Patient p = new Patient() { userId = id };
            DB.Patients.Add(p);
            DB.SaveChanges();
            return p.ID;
        }
        public int getpatient(string id) {
            Patient p = DB.Patients.FirstOrDefault(p => p.userId == id);
            if (p != null)
            {
                return p.ID;
            }
            return -1;
        }
        public Patient getpatientinfo(int id)
        {
            return DB.Patients.Include(p => p.user).FirstOrDefault(p => p.ID == id);
        }
		public List<Patient> getall()
		{
			return DB.Patients.Include(p=>p.user).ToList();
		}
        public void Delete(int id)
        {
            var patient = DB.Patients.FirstOrDefault(p => p.ID == id);
            DB.Patients.Remove(patient);
            DB.SaveChanges();

        }
    }
}
