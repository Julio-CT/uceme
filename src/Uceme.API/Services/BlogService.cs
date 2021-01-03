﻿namespace Uceme.API.Services
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

        public ApplicationDbContext DbContext { get; }

        public BlogService(ILogger<FotosService> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            this.DbContext = context;
        }

        public IEnumerable<Blog> GetBlogSubset(int amount, int page = 1)
        {
            try
            {
                var data = this.DbContext.Blog.Select(x => new Blog()
                {
                    idBlog = x.idBlog,
                    titulo = x.titulo,
                    fecha = x.fecha,
                    foto = x.foto,
                    texto = x.texto,
                    slug = x.slug,
                });

                data = data.OrderByDescending(x => x.fecha).Skip((page - 1) * 10).Take(amount);

                return data;
            }
            catch (Exception e)
            {
                logger.LogError($"Error retrieving Blogs {e.Message}");
                throw new DataException("Error retrieving Blogs", e);
            }
        }
    }
}
