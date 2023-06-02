using models;

namespace Services
{
    public interface Idepartmentservice
    {
        public List<Department> getAllDepartment();
        public Department get(int id);
        public bool createdepartment(Department dept);
        public bool edit(Department dept);
        public bool delete(int id);
        public bool isexist(string name);
    }
}
