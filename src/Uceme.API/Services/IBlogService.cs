namespace Uceme.API.Services
{
    using System.Collections.Generic;
    using Uceme.Model.Models;

    public interface IBlogService
    {
        IEnumerable<Blog> GetBlogSubset(int amount, int page = 1);
    }
}