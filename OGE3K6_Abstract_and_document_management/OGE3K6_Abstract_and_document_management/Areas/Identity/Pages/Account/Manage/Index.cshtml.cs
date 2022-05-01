using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using OGE3K6_Abstract_and_document_management.Data;
using OGE3K6_Abstract_and_document_management.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OGE3K6_Abstract_and_document_management.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public string Username { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SealNumber { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string FullName { get; set; }

            [StringLength(25)]
            [Display(Name = "Title")]
            public string Title { get; set; }

            [Required]
            [StringLength(50)]
            [Display(Name = "First name")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(50)]
            [Display(Name = "Last name")]
            public string LastName { get; set; }

            [StringLength(11)]
            [Display(Name = "Seal number")]
            public string SealNumber { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var fullName = (user as SiteUser).FullName;
            var title = (user as SiteUser).Title;
            var firstName = (user as SiteUser).FirstName;
            var lastName = (user as SiteUser).LastName;
            var sealNumber = (user as SiteUser).SealNumber;
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;
            FullName = fullName;
            Title = title;
            FirstName = firstName;
            LastName = lastName;
            SealNumber = sealNumber;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FullName = fullName,
                Title = title,
                FirstName = firstName,
                LastName = lastName,
                SealNumber = sealNumber,
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var title = (user as SiteUser).Title;
            var firstName = (user as SiteUser).FirstName;
            var lastName = (user as SiteUser).LastName;
            var sealNumber = (user as SiteUser).SealNumber;

            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            if (Input.Title != title)
            {
                (user as SiteUser).Title = Input.Title;
            }
            if (Input.FirstName != firstName)
            {
                (user as SiteUser).FirstName = Input.FirstName;
            }
            if(Input.LastName != lastName)
            {
                (user as SiteUser).LastName = Input.LastName;
            }
            if (Input.SealNumber != sealNumber)
            {
                (user as SiteUser).SealNumber = Input.SealNumber;
            }

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
