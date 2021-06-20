using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Chatter.Areas.Identity.Data;
using Chatter.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chatter.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ChatterUser> _userManager;
        private readonly SignInManager<ChatterUser> _signInManager;

        [BindProperty]
        public IFormFile UploadedImage { get; set; }

        public IndexModel(
            UserManager<ChatterUser> userManager,
            SignInManager<ChatterUser> signInManager,
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        public string Avatar { get; set; }


        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Chatter name")]
            public string ChatterName { get; set; }


            [Display(Name = "Avatar")]
            [DataType(DataType.Text)]
            public string Avatar { get; set; }

            [Display(Name = "About me")]
            [DataType(DataType.Text)]
            public string AboutMe { get; set; }

            [Display(Name = "Day of Birth")]
            [DataType(DataType.DateTime)]
            public DateTime DayOfBirth { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(ChatterUser user)
        {


            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                Avatar = user.Avatar,
                ChatterName = user.ChatterName,
                PhoneNumber = phoneNumber,
                AboutMe = user.AboutMe,
                DayOfBirth = user.DayOfBirth

            };

            //  user.Avatar = UploadedImage != null ? UploadedImage.FileName : "";
            await _userManager.UpdateAsync(user);
        }



        public async Task<IActionResult> OnGetAsync()
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Avatar = user.Avatar;
            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (UploadedImage != null)
            {
                var file = "./wwwroot/img/" + UploadedImage.FileName;
                using (var filestream = new FileStream(file, FileMode.Create))
                {
                    await UploadedImage.CopyToAsync(filestream);
                }
            }

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
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
            if (Input.ChatterName != user.ChatterName)
            {
                user.ChatterName = Input.ChatterName;
            }
            if (UploadedImage != null)
            {
                if (Input.Avatar != UploadedImage.ToString())
                {
                    user.Avatar = UploadedImage.FileName;
                }
            }
            if (Input.AboutMe != user.AboutMe)
            {
                user.AboutMe = Input.AboutMe;
            }
            if (Input.DayOfBirth != user.DayOfBirth)
            {
                user.DayOfBirth = Input.DayOfBirth;
            }

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
