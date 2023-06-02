using models;

namespace Services
{
    public interface Ifeedbackservice
    {
        public List<Feedback> getfeedbacks(int id);
        public void addfeedback(Feedback feedback);
        public void removefeedback(int id);
        public Feedback getfeedback(int id);

    }
}
