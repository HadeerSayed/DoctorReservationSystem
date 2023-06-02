using final_project.Models.Data;
using Microsoft.EntityFrameworkCore;
using models;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services
{
    public class schaduleservice:Ischaduleservice
    {
        public identityContext DB { get; }
        public schaduleservice(identityContext DB)
        {
            this.DB = DB;
        }
        public bool setschadule(Schadule schadule)
        { 
            try
            {
                DB.Schadules.Add(schadule);
                DB.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool updateschadule(Schadule schadule)
        {
            try
            {
                Schadule oldschadule = DB.Schadules.FirstOrDefault(p => p.ID == schadule.ID);
                List<Appointment> appoint = DB.Appointments.Where(p => p.schaduleId == schadule.ID).ToList();
                int sub = subtime(oldschadule.starttime, schadule.starttime);
                foreach (Appointment appointment in appoint)
                {
                    DateTime temp = Convert.ToDateTime(appointment.Time);
                    DateTime newendtime = Convert.ToDateTime(schadule.endtime);
                    int total = temp.Hour * 60 + temp.Minute + sub;
                    int hour = total / 60;
                    int minute = (int)Math.Round(((((float)total / 60) - (total / 60)) * 60));


                    if (int.Parse(schadule.duration)>int.Parse(oldschadule.duration))
                    {
                        if (subtime((hour + ":" + minute), schadule.starttime) != 0)
                        {
                            if (hour%2!=0)
                            {
                                minute = minute + 2*(int.Parse(schadule.duration) - int.Parse(oldschadule.duration));
                            }
                            else
                            {
                                minute = minute + (int.Parse(schadule.duration) - int.Parse(oldschadule.duration));
                            }
                        }
                    }
                    else if (int.Parse(schadule.duration) < int.Parse(oldschadule.duration))
                    {
                        if (subtime((hour + ":" + minute), schadule.starttime) != 0)
                        {
                            if (hour % 2 != 0)
                            {
                                minute = minute - (int.Parse(oldschadule.duration) - int.Parse(schadule.duration));
                            }
                            else
                            {
                                minute = minute - 2*(int.Parse(oldschadule.duration) - int.Parse(schadule.duration));
                            }
                        }
                    }
                    
                    DateTime temp2 = new DateTime(temp.Year,temp.Month,temp.Day,hour, minute, temp.Second);

                    if (temp2.Hour< newendtime.Hour||temp2.Hour== newendtime.Hour&&temp2.Minute< newendtime.Minute)
                    {
                        appointment.Time = temp2.ToString("HH:mm");
                        appointment.State = true;
                    }
                    else
                    {
                        appointment.State = false;
                    }
                   
                }
                oldschadule.starttime = schadule.starttime;
                oldschadule.endtime = schadule.endtime;
                oldschadule.duration = schadule.duration;
                DB.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool deleteschadule(int schadule)
        {
            try
            {
                Schadule sch = DB.Schadules.FirstOrDefault(p => p.ID == schadule);
                List<Appointment> appoint = DB.Appointments.Where(p => p.schaduleId == schadule).ToList();
                foreach(var appointment in appoint) {
                    appointment.State = false;
                }
                DB.Schadules.Remove(sch);
                DB.SaveChanges();
                return true;
            }
            catch (Exception ex) { return false; }
        }
        public Schadule getschadule(int schadule)
        {
            return DB.Schadules.Include(p => p.Clinic).FirstOrDefault(p => p.ID == schadule);
        }

        public List<Schadule> getschadulelist(int clinic)
        {
            DateTime now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            return DB.Schadules.Where(p => p.ClinicId == clinic && p.date >= now).ToList();
        }
        public List<Schadule> getreappointschadulelist(int clinic)
        {
            DateTime now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            return DB.Schadules.Where(p => p.ClinicId == clinic && p.date > now).ToList();
        }
        public bool SchaduleExist(DateTime date, int ClinicId)
        {
            return DB.Schadules.Any(p => p.date == date && p.ClinicId == ClinicId);
        }
        public Schadule getschadulebydate(int clinic, DateTime date)
        {
            return DB.Schadules.FirstOrDefault(p => p.ClinicId == clinic && p.date == date);

        }
        public Schadule getschadulebyappoint(int appoint)
        {
            Appointment appointment = DB.Appointments.FirstOrDefault(p => p.ID == appoint);
            return DB.Schadules.FirstOrDefault(p => p.ID == appointment.schaduleId);
        }
        private int subtime(string date1,string date2)
        {
            if (date1.Contains(":") && date2.Contains(":"))
            {
                string[] d1 = date1.Split(':');
                string[] d2 = date2.Split(':');
                int h1 = int.Parse(d1[0]) * 60;
                int h2 = int.Parse(d2[0]) * 60;
                int m1 = int.Parse(d1[1]);
                int m2 = int.Parse(d2[1]);
                return (h2 + m2) - (h1 + m1);
            }
            return -1;
        }

    }
    }
