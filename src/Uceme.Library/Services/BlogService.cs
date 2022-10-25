namespace Uceme.Library.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using Microsoft.Extensions.Logging;
    using Uceme.Model.Data;
    using Uceme.Model.DataContracts;
    using Uceme.Model.Models;

    public class BlogService : IBlogService
    {
        private readonly ILogger<FotosService> logger;

        private readonly ApplicationDbContext context;

        public BlogService(ILogger<FotosService> logger, IApplicationDbContext context)
        {
            this.logger = logger;
            this.context = (ApplicationDbContext)context;
        }

        public IEnumerable<Blog> GetBlogSubset(int amount, int page = 1)
        {
            try
            {
                var data = this.context.Blog.Select(x => new Blog()
                {
                    idBlog = x.idBlog,
                    titulo = x.titulo,
                    fecha = x.fecha.Date,
                    foto = x.foto,
                    texto = x.texto,
                    slug = x.slug,
                    seoTitle = x.seoTitle,
                    metaDescription = x.metaDescription,
                });

                data = data.Where(x => x.fecha <= DateTime.Now.Date).OrderByDescending(x => x.fecha).Skip(((page - 1) * 10) + (Math.Max(page - 2, 0) * 2)).Take(amount);

                return data;
            }
            catch (Exception e)
            {
                this.logger.LogError($"Error retrieving Blogs {e.Message}");
                throw new DataException("Error retrieving Blogs", e);
            }
        }

        public IEnumerable<Blog> GetAllPosts()
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
                    seoTitle = x.seoTitle,
                    metaDescription = x.metaDescription,
                }).OrderByDescending(x => x.fecha);

                return data;
            }
            catch (Exception e)
            {
                this.logger.LogError($"Error retrieving Posts {e.Message}");
                throw new DataException("Error retrieving Posts", e);
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
                    seoTitle = x.seoTitle,
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

        public bool DeletePost(int postId)
        {
            try
            {
                var post = this.context.Blog.First(post => post.idBlog == postId);
                this.context.Blog.Remove(post);
                this.context.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                this.logger.LogError($"Error deleting post {e.Message}");
                throw new DataException("Error deleting post", e);
            }
        }

        public Blog UpdatePost(Blog blog)
        {
            if (blog is null)
            {
                throw new ArgumentNullException(nameof(blog));
            }

            try
            {
                var post = this.context.Blog.First(post => post.idBlog == blog.idBlog);

                post.titulo = blog.titulo;
                post.fecha = blog.fecha;
                post.foto = blog.foto;
                post.texto = blog.texto;
                post.slug = blog.slug;
                post.seoTitle = blog.seoTitle;
                post.metaDescription = blog.metaDescription;

                var result = this.context.Blog.Update(post);
                this.context.SaveChanges();

                return result.Entity;
            }
            catch (Exception e)
            {
                this.logger.LogError($"Error updating post {e.Message}");
                throw new DataException("Error updating post", e);
            }
        }

        public bool UpdatePost(PostRequest blog)
        {
            if (blog is null)
            {
                throw new ArgumentNullException(nameof(blog));
            }

            try
            {
                var post = this.context.Blog.First(post => post.idBlog == blog.IdBlog);

                post.titulo = blog.Titulo;
                post.fecha = string.IsNullOrEmpty(blog.Fecha) ? DateTime.Now : DateTime.Parse(blog.Fecha, CultureInfo.InvariantCulture);
                post.foto = blog.Foto;
                post.texto = blog.Texto;
                post.slug = blog.Slug;
                post.seoTitle = blog.SeoTitle;
                post.metaDescription = blog.MetaDescription;

                var updatedPost = this.context.Blog.Update(post);
                if (updatedPost.Entity.idBlog == post.idBlog)
                {
                    var result = this.context.SaveChanges();

                    return result == 1;
                }

                throw new DataException("Updated post Id doesn´t match the original one - it was not saved");
            }
            catch (Exception e)
            {
                this.logger.LogError($"Error updating post {e.Message}");
                throw new DataException("Error updating post", e);
            }
        }

        public bool AddPost(PostRequest blog)
        {
            if (blog == null)
            {
                throw new ArgumentNullException(nameof(blog));
            }

            try
            {
                var post = new Blog
                {
                    titulo = blog.Titulo,
                    fecha = string.IsNullOrEmpty(blog.Fecha) ? DateTime.Now : DateTime.Parse(blog.Fecha, CultureInfo.InvariantCulture),
                    foto = blog.Foto,
                    texto = blog.Texto,
                    slug = blog.Slug,
                    seoTitle = blog.SeoTitle,
                    metaDescription = blog.MetaDescription,
                    idUsuario = 16,
                };

                this.context.Blog.Add(post);
                var result = this.context.SaveChanges();

                return result == 1;
            }
            catch (Exception e)
            {
                this.logger.LogError($"Error adding post {e.Message}");
                throw new DataException("Error adding post", e);
            }
        }

        public string GetNextPostImage()
        {
            var lastPhoto = this.context.Blog.OrderByDescending(post => post.idBlog).First().idBlog;
            return (lastPhoto + 1).ToString(CultureInfo.InvariantCulture);
        }
    }
}
