using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

using SweetHome.Core.Entities;
using SweetHome.Core.Entities.Identity;


namespace SweetHome.Data
{
    public class DataDbContext : IdentityDbContext<AppUser, AppRole, long, AspNetUserLogin, AspNetRole, AspNetUserClaim>
    {
        public DataDbContext()
            : base("DataDbContext")
        {
        }

        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<Message> Messages { get; set; }

        public static DataDbContext Create()
        {
            return new DataDbContext();
        }
    }
}
