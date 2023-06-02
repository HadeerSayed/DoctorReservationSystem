using final_project.Models.Data;
using Microsoft.EntityFrameworkCore;
using models;
using Services;

namespace Services
{
    public class feedbackservice: Ifeedbackservice
    {
        public identityContext DB { get; }
        public feedbackservice(identityContext DB)
        {
            this.DB = DB;
        }

        public List<Feedback> getfeedbacks(int id)
        {
            return DB.Feedbacks.Include(p=>p.patient).ThenInclude(p=>p.user).Where(p=>p.DoctorId==id).ToList();
        }

        public void addfeedback(Feedback feedback)
        {
            Doctor doc=DB.Doctors.FirstOrDefault(p=>p.ID==feedback.DoctorId);
            doc.globalrate += int.Parse(feedback.Rate);
            DB.Doctors.Update(doc);
            DB.Feedbacks.Add(feedback);
            DB.SaveChanges();
        }

        public void removefeedback(int id)
        {
			Feedback feedback= DB.Feedbacks.Find(id);
			Doctor doc = DB.Doctors.FirstOrDefault(p => p.ID == feedback.DoctorId);
			doc.globalrate -= int.Parse(feedback.Rate);
			DB.Doctors.Update(doc);
			DB.Feedbacks.Remove(feedback);
            DB.SaveChanges();
        }

        public models.Feedback getfeedback(int id)
        {
            return DB.Feedbacks.FirstOrDefault(p => p.ID == id);
        }
    }
}
