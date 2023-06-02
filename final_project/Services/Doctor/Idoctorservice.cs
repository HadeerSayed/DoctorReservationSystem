using models;

namespace Services
{
    public interface Idoctorservice
    {
        public int insertDoctor(Doctor doctor);
        public List<Doctor> GetAllDoctor();
        public Doctor getDetailes(int id);
        public void DeleteDoctor(int id);
        public Doctor getprofile(int id);
        public List<Doctor> GetAllDoctorwfeedback();

        public bool changephoto(int id, IFormFile photo);

        public void UpdateDoctor(int id, Doctor doctor);
        public List<Feedback> GetFeedback(int id);
        public bool updatepassword(ChangePassword p);
        public List<searchresult> GetDoctors(int number, int page, string gender, string department);
		//public bool DeleteDoctor(string id);
		//public bool UpdateDoctor(string id,Doctor doctor);
	}
}
