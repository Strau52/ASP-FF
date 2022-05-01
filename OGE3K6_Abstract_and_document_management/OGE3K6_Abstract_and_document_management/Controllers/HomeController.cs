using OGE3K6_Abstract_and_document_management.Data;
using OGE3K6_Abstract_and_document_management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace OGE3K6_Abstract_and_document_management.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext context;

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.context = context;
            this._logger = logger;
        }
        /*
         * UploadAbstract ->GET POST
         * AbstractManagement -> LIST
         * UserManagement ->LIST
         */


        public async Task<IActionResult> Init()
        {
            var first = userManager.Users.First();

            IdentityRole adminRole = new IdentityRole()
            {
                Name = "admin"
            };

            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(adminRole);

                await userManager.AddToRoleAsync(first, "admin");
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult UploadAbstract()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult UploadAbstract(Abstract newAbstract)
        {
            var uploader = this.User;
            var uploader_obj = userManager.GetUserAsync(uploader).Result;

            var oldAbstract = this.context.Abstracts.FirstOrDefault(t => t.Uploader == uploader_obj);
            DateTime creationDate = DateTime.Now;
            AbstractStatus status = AbstractStatus.Uploaded;
            newAbstract.Uploader = uploader_obj;
            newAbstract.Status = status;

            if (oldAbstract == null)
            {
                newAbstract.UID = Guid.NewGuid().ToString();
                newAbstract.UploadTime = creationDate;

                this.context.Abstracts.Add(newAbstract);
            }
            else
            {
                oldAbstract.AbstractTitle = newAbstract.AbstractTitle;
                oldAbstract.AbstractText = newAbstract.AbstractText;
                oldAbstract.UploadTime = creationDate;
                oldAbstract.Uploader = newAbstract.Uploader;
                oldAbstract.Status = newAbstract.Status;
                oldAbstract.Reviewer = newAbstract.Reviewer;
                oldAbstract.ReviewedAt = newAbstract.ReviewedAt;
            }
            this.context.SaveChanges();

            return RedirectToAction(nameof(UploadAbstract));
        }

        [Authorize(Roles = "admin,reviewer")]
        public IActionResult AbstractManagement()
        {
            return View(this.context.Abstracts.OrderBy(t => t.UploadTime).AsEnumerable());
        }

        [Authorize(Roles = "admin")]
        public IActionResult DeleteAbstract(string id)
        {
            var abs = this.context.Abstracts.FirstOrDefault(t => t.UID == id);
            if (abs != null)
            {
                this.context.Abstracts.Remove(abs);
                this.context.SaveChanges();
            }
            return RedirectToAction(nameof(AbstractManagement));
        }

        [Authorize(Roles = "admin,reviewer")]
        public IActionResult AbstractDetails(string id)
        {
            var reviewedAbstract = this.context.Abstracts.FirstOrDefault(t => t.UID == id);
            return View(reviewedAbstract);
        }

        [Authorize(Roles = "admin,reviewer")]
        public IActionResult ReviewAbstract(string id, int decision)
        {
            var reviewedAbstract = this.context.Abstracts.FirstOrDefault(t => t.UID == id);
            if (reviewedAbstract != null)
            {
                var reviewer = this.User;
                var reviewer_obj = userManager.GetUserAsync(reviewer);

                reviewedAbstract.Reviewer = reviewer_obj.Result;
                reviewedAbstract.ReviewedAt = DateTime.Now;
                reviewedAbstract.Status = (AbstractStatus)decision;
                this.context.SaveChanges();
            }
            return RedirectToAction(nameof(AbstractManagement));
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateRole(IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role_obj = new IdentityRole
                {
                    Name = role.Name,
                };
                IdentityResult result = await roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(RoleManagement));
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(role);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult RoleManagement()
        {
            var roles = roleManager.Roles.OrderBy(t => t.Name);
            return View(roles);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }

            var model = new IdentityRole
            {
                Id = role.Id,
                Name = role.Name,
            };

            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> EditRole(IdentityRole role)
        {
            var actualRole = await roleManager.FindByIdAsync(role.Id);

            if (actualRole == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {role.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                actualRole.Name = role.Name;
                var result = await roleManager.UpdateAsync(actualRole);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(RoleManagement));
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(role);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(RoleManagement));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(nameof(RoleManagement));
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            var actualRole = await roleManager.FindByIdAsync(roleId);
            ViewBag.roleId = roleId;
            ViewBag.roleName = actualRole.Name;

            if (actualRole == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            var model = new List<IdentityUser>();
            foreach (var user in userManager.Users)
            {
                var userToDisplay = new IdentityUser
                {
                    Id = user.Id,
                    UserName = user.UserName,
                };

                model.Add(userToDisplay);
            }

            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> EditUserInRole(string userId, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            var user = await userManager.FindByIdAsync(userId);

            IdentityResult result = null;

            if (!(await userManager.IsInRoleAsync(user, role.Name)))
            {
                result = await userManager.AddToRoleAsync(user, role.Name);
            }
            else
            {
                result = await userManager.RemoveFromRoleAsync(user, role.Name);
            }

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(EditRole), new { Id = roleId });
            }
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = userManager.Users.OrderBy(t => t.UserName);
            return View(users);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(ListUsers));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(nameof(ListUsers));
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
