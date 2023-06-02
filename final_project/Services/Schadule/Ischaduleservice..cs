using models;

namespace Services
{
    public interface Ischaduleservice
    {
        public bool setschadule(Schadule schadule);
        public bool updateschadule(Schadule schadule);
        public bool deleteschadule(int schadule);
        public Schadule getschadule(int schadule);
        public List<Schadule> getschadulelist(int clinic);
        public Schadule getschadulebydate(int clinic, DateTime date);
        public Schadule getschadulebyappoint(int appoint);
        public bool SchaduleExist(DateTime date, int ClinicId);
        public List<Schadule> getreappointschadulelist(int clinic);
    }
}
