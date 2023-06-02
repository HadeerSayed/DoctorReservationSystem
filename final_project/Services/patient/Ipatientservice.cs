using models;

namespace Services
{
    public interface Ipatientservice
    {
        public int setpatient(string id);
        public int getpatient(string id);
        public Patient getpatientinfo(int id);
        public List<Patient> getall();
        public void Delete(int id);


    }
}
