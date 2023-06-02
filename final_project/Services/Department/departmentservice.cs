using final_project.Models.Data;
using models;

namespace Services
{
    public class departmentservice : Idepartmentservice
    {
        public identityContext DB { get; }
        public departmentservice(identityContext DB)
        {
            this.DB = DB;
        }
        public List<Department> getAllDepartment()
        {
            return DB.Departments.ToList();

        }
        public bool createdepartment(Department dept)
        {
            if (isexist(dept.Name) == false)
            {
                dept.Name = dept.Name.ToLower();
                DB.Departments.Add(dept);
                DB.SaveChanges();
                return true;
            }
            return false;
        }
        public bool isexist(string name)
        {
            if (DB.Departments.FirstOrDefault(p => p.Name == name) != null)
            {
                return true;
            }
            return false;
        }
        public bool edit(Department dept)
        {
            Department d = DB.Departments.FirstOrDefault(p=>p.ID==dept.ID);
            if (d != null)
            {
                d.Name=dept.Name;
                if (DB.Departments.Count(p=>p.Name== d.Name)==0) {
                    DB.Update(d);
                    DB.SaveChanges();
                    return true;
                }
                return false;
            }
            return false;
        }
        public Department get(int id)
        {
            Department d= DB.Departments.FirstOrDefault(p => p.ID == id);
            if (d != null)
                return d;
            else
                return null;
        }
        public bool delete(int id)
        {
            Department d = DB.Departments.FirstOrDefault(p=>p.ID==id);
            if (d != null)
            {
                if (!DB.Doctors.Any(p => p.DepartmentId == id))
                {
                    DB.Remove(d);
                    DB.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            return false;
        }
        
    }
}
