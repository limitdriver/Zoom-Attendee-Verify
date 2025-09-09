namespace ZoomAttendeeVerifyApp
{
    public class Attendee
    {
        public string Error { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Company { get; set; }
        public bool InSalesforce { get; set; } = false;
        public bool InZoom { get; set; } = false;


        public Attendee(string email, string fullName, string company = "", string error = "", bool inSalesforce = false, bool inZoom = false)
        {
            Error = error;
            Email = email;
            FullName = fullName;
            Company = company;
            InSalesforce = inSalesforce;
            InZoom = inZoom;
        }
    }
}