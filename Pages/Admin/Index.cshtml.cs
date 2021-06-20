using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chatter.Areas.Identity.Data;
using Chatter.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnvDemo2.Pages.Admin
{
    [Authorize(Roles ="Admin")]
    public class IndexModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string AddUserId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string RemoveUserId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Role { get; set; }
        public ChatterUser CurrentUser { get; set; }

        [BindProperty]
        public string RoleName { get; set; }

        public List<IdentityRole> Roles { get; set; }

        public bool isUser { get; set; }
        public bool isAdmin { get; set; }

        public IQueryable<ChatterUser> Users { get; set; }
        [BindProperty(SupportsGet = true)]
        public Post NewOrChangedItem { get; set; }

        public List<Post> Posts;

        [BindProperty(SupportsGet = true)]
        public int DeleteId { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool PostOverview { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool RoleAdmin { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool ThreadAdmin { get; set; }

        private readonly RoleManager<IdentityRole> _roleManager;
        public UserManager<ChatterUser> _userManager;
        private readonly IPostGateway _gateway;
        private readonly Random _random;

        public IndexModel(RoleManager<IdentityRole> roleManager,
                            UserManager<ChatterUser> userManager,
                            IPostGateway gateway,
                            Random random)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _gateway = gateway;
            _random = random;
        }


        public async Task<IActionResult> OnGetAsync()
        {
            Roles = _roleManager.Roles.ToList();
            Users = _userManager.Users;
            Posts = await _gateway.GetPosts();


            if (DeleteId != 0)
            {
                await _gateway.DeletePost(DeleteId);
                return Page();
            }

            if (AddUserId != null)
            {
                var alterUser = await _userManager.FindByIdAsync(AddUserId);
                var roleresult = await _userManager.AddToRoleAsync(alterUser, Role);
            }

            if (RemoveUserId != null)
            {
                var alterUser = await _userManager.FindByIdAsync(RemoveUserId);
                var roleresult = await _userManager.RemoveFromRoleAsync(alterUser, Role);
            }


            CurrentUser = await _userManager.GetUserAsync(User);
            //isUser = await _userManager.IsInRoleAsync(CurrentUser, "User");
            isAdmin = await _userManager.IsInRoleAsync(CurrentUser, "Admin");
            return Page();

        }


        public async Task<IActionResult> OnPostAsync()
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            if (RoleName != null)
            {
                await CreateRole(RoleName);
            }

            else
            {
                if (NewOrChangedItem.MessageThread == 0)
                {
                    NewOrChangedItem.MessageThread = _random.Next(1, 9999999);
                }
                NewOrChangedItem.Author = CurrentUser.ChatterName;
                NewOrChangedItem.UserImage = CurrentUser.Avatar;
                NewOrChangedItem.Created = DateTime.Now;
                NewOrChangedItem.IsFlagged = false;
                await _gateway.PostPost(NewOrChangedItem);
            }
            return RedirectToPage("./index");
        }


        public async Task CreateRole(string roleName)
        {
            bool exist = await _roleManager.RoleExistsAsync(roleName);

            if (!exist)
            {
                var role = new IdentityRole
                {
                    Name = roleName
                };

                await _roleManager.CreateAsync(role);
            }
        }




    }
}
