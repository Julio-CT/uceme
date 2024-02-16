namespace Uceme.Library.Services;

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
    private readonly ILogger<BlogService> logger;

    private readonly ApplicationDbContext context;

    public BlogService(
        ILogger<BlogService> logger,
        IApplicationDbContext context)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.context = (ApplicationDbContext)context ?? throw new ArgumentNullException(nameof(context));
    }

    public IEnumerable<Blog> GetBlogSubset(int amount, int page = 1)
    {
        try
        {
            IQueryable<Blog> data = this.context.Blog.Select(x => new Blog()
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
            this.logger.LogError("Error retrieving Blogs {EMessage}", e.Message);
            throw new DataException("Error retrieving Blogs", e);
        }
    }

    public IEnumerable<Blog> GetAllPosts()
    {
        try
        {
            IOrderedQueryable<Blog> data = this.context.Blog.Select(x => new Blog()
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
            this.logger.LogError("Error retrieving Posts {EMessage}", e.Message);
            throw new DataException("Error retrieving Posts", e);
        }
    }

    public Blog GetPost(string slug)
    {
        if (string.IsNullOrEmpty(slug) || string.IsNullOrEmpty(slug.Trim()))
        {
            throw new ArgumentNullException(nameof(slug));
        }

        try
        {
            Blog data = this.context.Blog.Select(x => new Blog()
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
            this.logger.LogError("Error retrieving Blogs {EMessage}", e.Message);
            throw new DataException("Error retrieving Blogs", e);
        }
    }

    public bool DeletePost(int postId)
    {
        try
        {
            Blog post = this.context.Blog.First(post => post.idBlog == postId);
            this.context.Blog.Remove(post);
            this.context.SaveChanges();

            return true;
        }
        catch (Exception e)
        {
            this.logger.LogError("Error deleting post {EMessage}", e.Message);
            throw new DataException("Error deleting post", e);
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
            this.CheckUniqueSlug(blog);

            Blog post = this.context.Blog.First(post => post.idBlog == blog.IdBlog);

            post.titulo = blog.Titulo;
            post.fecha = string.IsNullOrEmpty(blog.Fecha) ? DateTime.Now : DateTime.Parse(blog.Fecha, CultureInfo.InvariantCulture);
            post.foto = blog.Foto;
            post.texto = blog.Texto;
            post.slug = blog.Slug;
            post.seoTitle = blog.SeoTitle;
            post.metaDescription = blog.MetaDescription;

            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Blog> updatedPost = this.context.Blog.Update(post);
            if (updatedPost.Entity.idBlog == post.idBlog)
            {
                int result = this.context.SaveChanges();

                return result == 1;
            }

            throw new DataException("Updated post Id does not match the original one - it was not saved");
        }
        catch (Exception e)
        {
            this.logger.LogError("Error updating post {EMessage}", e.Message);
            throw new DataException("Error updating post", e);
        }
    }

    public bool AddPost(PostRequest blog)
    {
        if (blog is null)
        {
            throw new ArgumentNullException(nameof(blog));
        }

        try
        {
            this.CheckUniqueSlug(blog);

            Blog post = new Blog
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
            int result = this.context.SaveChanges();

            return result == 1;
        }
        catch (Exception e)
        {
            this.logger.LogError("Error adding post {EMessage}", e.Message);
            throw new DataException("Error adding post", e);
        }
    }

    public string GetNextPostImage()
    {
        int lastPhoto = this.context.Blog.OrderByDescending(post => post.idBlog).First().idBlog;
        return (lastPhoto + 1).ToString(CultureInfo.InvariantCulture);
    }

    private void CheckUniqueSlug(PostRequest blog)
    {
        if (this.context.Blog.Any(x => x.slug == blog.Slug))
        {
            blog.Slug = "Mas " + blog.Slug;
            this.CheckUniqueSlug(blog);
        }
    }
}
