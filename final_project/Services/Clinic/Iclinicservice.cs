using models;

namespace Services
{
    public interface Iclinicservice
    {
        public Clinic GetClinic(int id);
        public List<Clinic> getAllClinics();
        public List<Clinic> GetClinicList(int doctor);
        public void AddDoctorClinic(Clinic clinic);
        public List<Clinic> getDoctorClinics(int id);
        public Clinic GetSelectedClinic(int id);
        public int? DeleteDoctorClinic(int id);
        public int? EditeDoctorClinic(int id, Clinic clinic);
        public List<searchresult> searchclinic(int? id, string? doctor, int number, int page, string? gender, string? department);

	}
}
