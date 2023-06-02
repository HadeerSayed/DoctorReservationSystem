namespace models
{
    public class dclinic
    {

        public Doctor doctor { get; set; }

        public List<Clinic> clinic { get; set; }=new List<Clinic>();        
    }
}
