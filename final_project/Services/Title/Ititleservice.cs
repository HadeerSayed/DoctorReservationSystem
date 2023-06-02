using models;

namespace Services
{
    public interface Ititleservice
    {
        public bool create(title c);
        public bool update(title c);
        public bool delete(int id);
        public List<title> titlies();
        public bool isexisting(string name);
        public title GetTitle(int id);
    }
}
