using Chatter.Areas.Identity.Data;
using Chatter.Data;
using Chatter.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;

namespace Chatter.Pages
{
    public class IndexModel : PageModel
    {

        private readonly ILogger<IndexModel> _logger;
        private readonly IPostGateway _gateway;
        private readonly SignInManager<ChatterUser> _signInManager;
        private readonly UserManager<ChatterUser> _userManager;
        private readonly ChatterMessageContext _messageContext;
        private readonly StringBuilder _stringBuilder;
        private readonly Random _random;

        [BindProperty]
        public Message NewOrChangedMessage { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Thread { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SubThread { get; set; }

        [BindProperty(SupportsGet = true)]
        public int MessageThread { get; set; }

        public ChatterUser MyUser { get; set; }

        public static ChatterUser StaticUser { get; set; }
        public ChatterUser SignInManager { get; set; }

        public List<Message> Messages;
        public List<Post> Posts;

        public List<Post> Threads;

        [BindProperty(SupportsGet = true)]
        public Post NewOrChangedItem { get; set; }

        [BindProperty(SupportsGet = true)]
        public int DeleteId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int ReportId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int EditId { get; set; }
        [BindProperty(SupportsGet = true)]

        static public IList<Post> PostSearch { get; set; }
        public string SearchString { get; set; }

        public IndexModel(ILogger<IndexModel> logger,
                          IPostGateway gateway,
                          SignInManager<ChatterUser> signInManager,
                          UserManager<ChatterUser> userManager,
                          Chatter.Data.ChatterMessageContext messageContext,
                          StringBuilder stringBuilder,
                          Random random)

        {

            _logger = logger;
            _gateway = gateway;
            _signInManager = signInManager;
            _userManager = userManager;
            _messageContext = messageContext;
            _stringBuilder = stringBuilder;
            _random = random;
        }

        public string Censor(string content)
        {


            _stringBuilder.Remove(0, _stringBuilder.Length);
            string[] badwords = { "Shit", "poop", "bastard", "idiot" };
            char[] delimiterChars = { ' ', ',', '.', ':', '\t', '!', '?' };


            string[] contentArray = content.Split(delimiterChars);

            contentArray = contentArray.Select(x => x.Replace("shit", "****")).ToArray();


            foreach (string word in contentArray)
            {
                _stringBuilder.AppendLine(word);
            }

            var CensorResult = _stringBuilder.ToString();
            content = CensorResult;


            return content;
        }

        public async Task<IActionResult> OnGetAsync()
        {



            StaticUser = await _userManager.GetUserAsync(User);
            MyUser = await _userManager.GetUserAsync(User);

            Messages = await _messageContext.Message.ToListAsync();

            if (DeleteId != 0)
            {
                await _gateway.DeletePost(DeleteId);
            }


            Posts = await _gateway.GetPosts();
            return Page();

        }

        public async Task<IActionResult> OnPostAsync()
        {
            MyUser = await _userManager.GetUserAsync(User);

            if (NewOrChangedItem.MessageThread == 0)
            {
                NewOrChangedItem.MessageThread = _random.Next(1, 9999999);
            }
            NewOrChangedItem.Thread = Thread;
            NewOrChangedItem.Subthread = SubThread;
            if (_signInManager.IsSignedIn(User))
            {
                NewOrChangedItem.Author = MyUser.ChatterName;
                NewOrChangedItem.UserImage = MyUser.Avatar;
            }
            else
            {
                NewOrChangedItem.Author = "Anonymous";
                NewOrChangedItem.UserImage = "anonymous.png";
            }

             NewOrChangedItem.Created = DateTime.Now;

            if (NewOrChangedItem.IsFlagged != true)
            {
                NewOrChangedItem.IsFlagged = false;
            }


            if (EditId != 0 || ReportId != 0)
            {
                if (EditId != 0)
                {
                    await _gateway.PutPost(EditId, NewOrChangedItem);
                }
                if (ReportId != 0)
                {
                    NewOrChangedItem.IsFlagged = true;
                    await _gateway.PutPost(ReportId, NewOrChangedItem);
                }
            }
            else
            {
                await _gateway.PostPost(NewOrChangedItem);
                _messageContext.Message.Add(NewOrChangedMessage);
                await _messageContext.SaveChangesAsync();
            }

            Posts = await _gateway.GetPosts();
            return Page();
        }
    }
}
