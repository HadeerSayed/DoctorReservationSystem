using models;

namespace final_project.Services.Country
{
    public interface Icountryservice
    {
        public bool create(country c);
        public bool update(country c);
        public bool delete(int id);
        public List<country> countries();
        public country GetCountry(int id);
    }
}
