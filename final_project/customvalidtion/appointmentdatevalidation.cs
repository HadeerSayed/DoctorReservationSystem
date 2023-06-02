using System.ComponentModel.DataAnnotations;

namespace customvalidtion
{
    public class appointmentdatevalidation:ValidationAttribute
    {
        public appointmentdatevalidation() { }
        public override bool IsValid(object value)
        {
            DateTime val = (DateTime)value;
            return val>DateTime.Now;
        }
        public override string FormatErrorMessage(string name)
        {
            return string.Format("Can't set schadule for pass date");
        }
    }
}
