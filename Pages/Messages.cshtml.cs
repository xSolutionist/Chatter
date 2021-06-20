using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chatter.Areas.Identity.Data;
using Chatter.Data;
using Chatter.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Chatter.Pages
{
    public class MessagesModel : PageModel
    {

        [BindProperty]
        public Message NewOrChangedMessage { get; set; }
        public ChatterUser MyUser { get; set; }

        public List<Message> Messages { get; set; }

        [BindProperty(SupportsGet = true)]
        public int DeleteId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int EditId { get; set; }

        private readonly UserManager<ChatterUser> _userManager;
        private readonly StringBuilder _stringBuilder;
        private readonly ChatterMessageContext _messageContext;

        public MessagesModel(Chatter.Data.ChatterMessageContext context,
                            UserManager<ChatterUser> userManager,
                            StringBuilder stringBuilder
                            )
        {
            _userManager = userManager;
            _stringBuilder = stringBuilder;
            _messageContext = context;


        }

        public async Task<IActionResult> OnGetAsync()
        {
            MyUser = await _userManager.GetUserAsync(User);
            Messages = await _messageContext.Message.ToListAsync();
            if (DeleteId != 0)
            {
                Models.Message message = await _messageContext.Message.FindAsync(DeleteId);
                if (message != null)
                {
                 _messageContext.Message.Remove(message);
                 await _messageContext.SaveChangesAsync();
                    return RedirectToPage("./messages");

                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            MyUser = await _userManager.GetUserAsync(User);

            NewOrChangedMessage.Created = DateTime.Now;
            NewOrChangedMessage.UserImage = MyUser.Avatar;
            NewOrChangedMessage.SendingUser = MyUser.ChatterName;
            _messageContext.Message.Add(NewOrChangedMessage);
            await _messageContext.SaveChangesAsync();
            return Page();
        }
    }
}
