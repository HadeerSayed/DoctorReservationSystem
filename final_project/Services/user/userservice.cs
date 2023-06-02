using final_project.Models.Data;
using Microsoft.AspNetCore.Http;
using models;

namespace Services
{
    public class userservice: Iuserservice
    {
        private readonly IHttpContextAccessor context;
        public identityContext DB { get; }
        public userservice(IHttpContextAccessor _context,identityContext db)
        {
            context = _context;
            DB = db;
        }
        public int getpatient(string id)
        {
            Patient p = DB.Patients.FirstOrDefault(p => p.userId == id);
            if (p != null)
            {
                return p.ID;
            }
            return -1;
        }
        public int getdoctor(string id)
        {
            Doctor p = DB.Doctors.FirstOrDefault(p => p.userId == id);
            if (p != null)
            {
                return p.ID;
            }
            return -1;
        }
        public int getadmin(string id)
        {
            Admin p = DB.Admins.FirstOrDefault(p => p.userId == id);
            if (p != null)
            {
                return p.ID;
            }
            return -1;
        }
        public string getuserid()
        {
            return context.HttpContext.Session.GetString("UserId");
        }
        public int getid()
        {
            if(context.HttpContext.Session.GetInt32("Id") != null)
            return (int)context.HttpContext.Session.GetInt32("Id");
            return -1;
        }

        public void setuser(string userid, int id)
        {
            if (id != null && userid != null)
            {
                context.HttpContext.Session.SetString("UserId", userid);
                context.HttpContext.Session.SetInt32("Id", id);
            }
            else
            {
                context.HttpContext.Session.SetString("UserId","");
                context.HttpContext.Session.SetInt32("Id", -1);
            }
        }
        public List<int> getcounts()
        {
            List<int> count= new List<int>();
            count.Add(DB.Doctors.Count());
            count.Add(DB.Patients.Count());
            count.Add(DB.Appointments.Where(p=>p.completed==true).Count());
            count.Add(DB.Appointments.Count());
            return count;
        }
    }
}
