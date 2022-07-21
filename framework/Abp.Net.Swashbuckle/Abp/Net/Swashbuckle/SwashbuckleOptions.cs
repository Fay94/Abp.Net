namespace Abp.Net.Swashbuckle
{
    public class SwashbuckleOptions
    {
        public bool IsEnable { get; set; }

        public string CheckUrl { get; set; }

        public string SubmitUrl { get; set; }
        public string UserName { get; set; }

        public string Password { get; set; }

        public SwashbuckleOptions()
        {
            IsEnable = false;
            CheckUrl = "/Home/CheckUrl";
            SubmitUrl = "/Home/SubmitUrl";
            UserName = "admin";
            Password = "admin";
        }
    }
}
