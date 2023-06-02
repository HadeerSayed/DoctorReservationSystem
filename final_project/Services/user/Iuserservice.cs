namespace Services
{
    public interface Iuserservice
    {
        public void setuser(string userid,int id);
        public int getpatient(string id);
        public int getdoctor(string id);
        public int getadmin(string id);
        public int getid();
        public string getuserid();
        public List<int> getcounts();
    }
}
