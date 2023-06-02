using final_project.Models.Data;
using models;

namespace Services
{
    public class titleservice:Ititleservice
    {
        public identityContext DB { get; }
        public titleservice(identityContext DB)
        {
            this.DB = DB;
        }
        public bool create(title c)
        {
            try {
                c.Title = c.Title.ToLower();
                if (!isexisting(c.Title))
                {

                    DB.Titles.Add(c);
                    DB.SaveChanges();
                    return true;
                }
                return false;
                
            }
            catch(Exception e) {
                return false;
            }

        }
        public bool update(title c)
        {
            try {
                title temp = DB.Titles.FirstOrDefault(p => p.ID == c.ID);
                if (temp != null)
                {
                    temp.Title = c.Title.ToLower();
                    if (DB.Titles.Count(p => p.Title == temp.Title) == 0)
                    {
                        DB.Update(temp);
                        DB.SaveChanges();
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch(Exception e) {
                return false;
            }
        }
        public bool delete(int id)
        {
            try {
                title temp = DB.Titles.FirstOrDefault(p => p.ID == id);
                if (temp != null)
                {
                    DB.Titles.Remove(temp);
                    DB.SaveChanges();
                    return true;
                }
                return false;
            }
            catch(Exception e) {
                return false;
            }
        }
        public List<title> titlies()
        {
            return DB.Titles.ToList();
        }
        public title GetTitle(int id)
        {
            return DB.Titles.FirstOrDefault(p => p.ID == id);
        }
        public bool isexisting(string name)
        {
            return DB.Titles.Any(p=>p.Title== name);
        }
    }
}
