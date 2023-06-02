using models;
using final_project.Models.Data;

namespace final_project.Services.Country
{
    public class countryservice: Icountryservice
    {
        public identityContext DB { get; }
        public countryservice(identityContext DB)
        {
            this.DB = DB;
        }
        public bool create(country c)
        {
            try {
                c.Name=c.Name.ToLower();
                if (!DB.Countries.Any(p => p.Name == c.Name))
                {
                    DB.Countries.Add(c);
                    DB.SaveChanges();
                    return true;
                }
                return false;
                
            }
            catch(Exception e){
                return false;
            }

        }
        public bool update(country c)
        {
            try {
                country temp = DB.Countries.FirstOrDefault(p => p.Id == c.Id);
                if (temp != null)
                {
                    temp.Name = c.Name.ToLower();
                    if (DB.Countries.Count(p => p.Name == temp.Name)==0)
                    {
                        DB.Update(temp);
                        DB.SaveChanges();
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch(Exception e)
            {
                return false;
            }
        }
        public bool delete(int id)
        {
			country d = DB.Countries.FirstOrDefault(p => p.Id == id);
			if (d != null)
			{
				if (!DB.Clinics.Any(p => p.countryid == id))
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
        public List<country> countries()
        {
            return DB.Countries.ToList();
        }
        public country GetCountry(int id) {
            return DB.Countries.FirstOrDefault(p => p.Id == id);
        }
    }
}
