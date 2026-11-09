using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly UserManager<IdentityUser> userManager;

        public AdminUsersController(IUserRepository userRepository, UserManager<IdentityUser> userManager)
        {
            this.userRepository = userRepository;
            this.userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var users = await userRepository.GetAllAsync();
            var userViewModel = new UserViewModel();
            userViewModel.users = new List<User>();
            foreach (var useritems in users)
            {
                userViewModel.users.Add(new Models.ViewModels.User
                {
                    Id = Guid.Parse(useritems.Id),
                    Username = useritems.UserName,
                    EmailAddress = useritems.Email
                });
            }
            return View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> List(UserViewModel request)
        {
            var identityUser = new IdentityUser
            {
                UserName = request.Username,
                Email = request.email
            };
            var identityresult = await userManager.CreateAsync(identityUser, request.Password);
            if (identityresult.Succeeded && identityresult != null)
            {
                var roles = new List<string> { "User" };
                if (request.AdminRoleChkBox)
                {
                    roles.Add("Admin");
                }
                var roleIdentityResult = await userManager.AddToRolesAsync(identityUser, roles);
                if (roleIdentityResult.Succeeded)
                {
                    return RedirectToAction("List", "AdminUsers");
                }
            }
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var user = await userManager.FindByIdAsync(Id.ToString());
            if (user != null) 
            { 
                var userDeleteResult  = await userManager.DeleteAsync(user);
                if (userDeleteResult.Succeeded && userDeleteResult != null) 
                {
                    return RedirectToAction("List", "AdminUsers");
                }
            }
            return View();
            
        }
    }
}
