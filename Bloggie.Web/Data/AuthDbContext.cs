using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Seed the roles(User, Admin, SuperAdmin)
            string adminRoleId = "1e558804-58de-4d2e-9090-1ab1ff20963a";
            string superAdminRoleId = "0774bdd8-016a-496c-9ef1-8bc7cd39fc85";
            string userRoleId = "63a78414-378c-4bc6-8e24-f22e48dc244d";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name="Admin",
                    NormalizedName="Admin",
                    Id=adminRoleId,
                    ConcurrencyStamp=adminRoleId
                },
                new IdentityRole
                {
                    Name="SuperAdmin",
                    NormalizedName="SuperAdmin",
                    Id=superAdminRoleId,
                    ConcurrencyStamp=superAdminRoleId
                },
                new IdentityRole
                {
                    Name="User",
                    NormalizedName="User",
                    Id=userRoleId,
                    ConcurrencyStamp=userRoleId
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);

            //Seed SuperAdminUser
            var superAdminUserId = "a3eb0be9-6fdd-480b-bc7a-59824ad06168";
            var superAdminUser = new IdentityUser
            {
                UserName = "superadmin@bloggie.com",
                Email = "superadmin@bloggie.com",
                NormalizedEmail = "superadmin@bloggie.com".ToUpper(),
                NormalizedUserName = "superadmin@bloggie.com".ToUpper(),
                Id = superAdminUserId,
                ConcurrencyStamp = superAdminUserId
            };

            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(superAdminUser, "superadmin@123");
            builder.Entity<IdentityUser>().HasData(superAdminUser);

            //Add all roles to SuperAdminUser
            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId= adminRoleId,
                    UserId=superAdminUserId
                },
                new IdentityUserRole<string>
                {
                    RoleId=superAdminRoleId,
                    UserId=superAdminUserId
                },
                new IdentityUserRole<string>
                {
                    RoleId=userRoleId,
                    UserId=superAdminUserId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);    
        }
    }
}
