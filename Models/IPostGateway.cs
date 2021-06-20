using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatter.Models
{
    public interface IPostGateway
    {

        Task<List<Models.Post>> GetPosts();

        Task<Models.Post> GetPost(int postid, Models.Post post);

        Task<Models.Post> PostPost(Models.Post post);

        Task PutPost(int editId, Models.Post post);

        Task<Models.Post> DeletePost(int deleteId);
    }
}
