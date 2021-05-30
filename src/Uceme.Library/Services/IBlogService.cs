namespace Uceme.Library.Services
{
    using System.Collections.Generic;
    using Uceme.Model.DataContracts;
    using Uceme.Model.Models;

    public interface IBlogService
    {
        IEnumerable<Blog> GetBlogSubset(int amount, int page = 1);

        Blog GetPost(string slug);

        IEnumerable<Blog> GetAllPosts();

        bool DeletePost(int postId);

        Blog UpdatePost(Blog blog);

        bool UpdatePost(PostRequest blog);

        bool AddPost(PostRequest blog);

        string GetNextPostImage();
    }
}
