using Microsoft.AspNet.Identity.EntityFramework;

namespace SweetHome.Core.Entities.Identity
{
    public class AppUser : IdentityUser<long, AspNetUserLogin, AspNetRole, AspNetUserClaim>
    {
        public string PhotoUri { get; set; }
        public string Gender { get; set; }
        public int ZipCode { get; set; }
        public string City { get; set; }
        public bool IsAdmin { get; set; }
    }
}
