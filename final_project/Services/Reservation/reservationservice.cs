using final_project.Models.Data;
using Microsoft.EntityFrameworkCore;
using models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services
{
    public class reservationservice : Ireservationservice
    {
        public identityContext DB { get; }
        public Ischaduleservice Ischaduleservice { get; }
        public reservationservice(identityContext DB, Ischaduleservice ischaduleservice)
        {
            this.DB = DB;
            Ischaduleservice = ischaduleservice;
        }
        public List<Appointment> getpatientreservation(int id)
        {
            return DB.Appointments.Include(p => p.Doctor).ThenInclude(p=>p.user)
                                  .Where(p => p.PatientId == id)
                                  .ToList();
        }
        public bool reservationExist(int schadule)
        {
            return DB.Appointments.Any(p => p.schaduleId == schadule);
        }
        public List<Appointment>reservation(int clinic,DateTime date)
        {
            try
            {
                Schadule sch = DB.Schadules.FirstOrDefault(p => p.ClinicId == clinic && p.date == date);
                if(sch != null) { 
                    return DB.Appointments.Include(p=>p.Patient).ThenInclude(p=>p.user).Where(p =>  p.schaduleId == sch.ID&&p.State==true).ToList();
                }
                else
                {
                    return new List<Appointment>() { };
                }
            }
            catch (Exception e)
            {
                return new List<Appointment>() { };
            }
        }
        public bool bookappointment(Appointment appoint)
        {
            try
            {
                DB.Appointments.Add(appoint);
                DB.SaveChanges();
                return true;
            }
            catch (Exception e) { return false; }
        }
        public bool cancelreservation(int id)
        {
            Appointment appoint=DB.Appointments.FirstOrDefault(p=>p.ID== id);
            if(appoint != null)
            {
                DB.Appointments.Remove(appoint);
                DB.SaveChanges();
                return true;
            }
            return false;
        }
        public bool cancelpatientreservation(int id)
        {
            Appointment appoint = DB.Appointments.FirstOrDefault(p => p.ID == id);
            if (appoint != null)
            {
                appoint.State = false;
                appoint.PatientId = null;
                DB.SaveChanges();
                return true;
            }
            return false;
        }
        public bool completedreservation(int id)
        {
            Appointment appoint=DB.Appointments.FirstOrDefault(p=>p.ID== id);
            if(appoint != null)
            {
                appoint.completed = true;
                DB.SaveChanges();
                return true;
            }
            return false;
        }
        public List<Appointment> freereservation(int clinic1, DateTime date)
        {
            Schadule sch = DB.Schadules.FirstOrDefault(p => p.ClinicId == clinic1 && p.date == date);
            List<Appointment> freeappoints = new List<Appointment>();
            if (sch != null)
            {

                List<Appointment> busyappoints = DB.Appointments.Where(p => p.schaduleId == sch.ID).ToList();
                int h1 =int.Parse(sch.starttime.Substring(0,2));
                int h2 = int.Parse(sch.endtime.Substring(0, 2)); 
                int m1 = int.Parse(sch.starttime.Substring(3,2)); 
                int m2 = int.Parse(sch.endtime.Substring(3, 2)); 
                int count =((h2*60+m2)-(h1*60+m1))/int.Parse(sch.duration);
                for (int i=0;i<count; i++)
                {
                    int time= h1 * 60 + m1 + int.Parse(sch.duration) * i;
                    int h = time / 60;
                    int m = (int)((((float)time / 60) - (time / 60)) * 60);
                    DateTime d = DateTime.Now;
                    string time1=new DateTime(d.Year,d.Month,d.Day,h,m,0).ToString("HH:mm");
                    if (busyappoints.FindIndex(i => i.Time ==time1)==-1)
                    {
                        freeappoints.Add(new Appointment() { Time = time1,schaduleId=sch.ID, Date =sch.date});
                    }
                }
            }
            return freeappoints;
        }
        public bool addreservation(Appointment appoint)
        {
            try
            {
                if (appoint.Date == new DateTime())
                {
                    DateTime date = DB.Schadules.FirstOrDefault(p => p.ID == appoint.schaduleId).date;
                    appoint.Date = date;
                }
                DB.Appointments.Add(appoint);
                DB.SaveChanges();
                return true;
            }catch(Exception ex)
            {
                return false;
            }

        }
        public Appointment getappointment(int id)
        {
            try
            {
                return DB.Appointments.FirstOrDefault(p => p.ID == id);

            }
            catch (Exception e)
            {
                return null;
            }
        }
        public Schadule getschadulebydate(int clinic, DateTime date)
        {
            return DB.Schadules.FirstOrDefault(p => p.ClinicId == clinic && p.date == date);

        }
        public bool updateappointment(Appointment newappoint, int appointment, int clinic, DateTime date)
        {
            try
            {
                Schadule sch = getschadulebydate(clinic, date);
                newappoint.schaduleId = sch.ID;
                DB.Appointments.Add(newappoint);
                DB.Appointments.Remove(getappointment(appointment));
                DB.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }


        }
    }
}
