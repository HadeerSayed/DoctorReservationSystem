using models;

namespace Services
{
    public interface Ireservationservice
    {
        public bool reservationExist(int schadule);
        public List<Appointment> reservation(int clinic, DateTime date);
        public bool cancelreservation(int id);
        public bool completedreservation(int id);
        public List<Appointment> freereservation(int clinic, DateTime date);
        public bool addreservation(Appointment appoint);
        public List<Appointment> getpatientreservation(int id);
        public bool cancelpatientreservation(int id);
        public bool bookappointment(Appointment appoint);
        public Schadule getschadulebydate(int clinic, DateTime date);
        public Appointment getappointment(int id);
        public bool updateappointment(Appointment newappoint, int appointment, int clinic, DateTime date);

    }
}
