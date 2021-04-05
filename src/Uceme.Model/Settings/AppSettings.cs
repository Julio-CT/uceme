namespace Uceme.Model.Settings
{
    using Microsoft.AspNetCore.Identity;

    public class AppSettings : IdentityUser
    {
        public string Telephone { get; set; }

        public string ContactEmail { get; set; }

        public string Address { get; set; }

        public string BlogImagesDir { get; set; }
    }
}
