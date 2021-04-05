namespace Uceme.Library.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Microsoft.Extensions.Logging;
    using Uceme.Model.Data;
    using Uceme.Model.Models;

    public class BlogService : IBlogService
    {
        private readonly ILogger<FotosService> logger;

        private readonly ApplicationDbContext context;

        public BlogService(ILogger<FotosService> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public IEnumerable<Blog> GetBlogSubset(int amount, int page = 1)
        {
            try
            {
                var data = this.context.Blog.Select(x => new Blog()
                {
                    idBlog = x.idBlog,
                    titulo = x.titulo,
                    fecha = x.fecha,
                    foto = x.foto,
                    texto = x.texto,
                    slug = x.slug,
                    metaDescription = x.metaDescription,
                });

                data = data.OrderByDescending(x => x.fecha).Skip(((page - 1) * 10) + (Math.Max(page - 2, 0) * 2)).Take(amount);

                return data;
            }
            catch (Exception e)
            {
                this.logger.LogError($"Error retrieving Blogs {e.Message}");
                throw new DataException("Error retrieving Blogs", e);
            }
        }

        public Blog GetPost(string slug)
        {
            try
            {
                var data = this.context.Blog.Select(x => new Blog()
                {
                    idBlog = x.idBlog,
                    titulo = x.titulo,
                    fecha = x.fecha,
                    foto = x.foto,
                    texto = x.texto,
                    slug = x.slug,
                    metaDescription = x.metaDescription,
                }).First(x => x.slug == slug);

                return data;
            }
            catch (Exception e)
            {
                this.logger.LogError($"Error retrieving Blogs {e.Message}");
                throw new DataException("Error retrieving Blogs", e);
            }
        }
    }
}
