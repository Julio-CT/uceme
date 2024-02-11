namespace Uceme.Model.Data
{
    using System;
    using IdentityServer4.EntityFramework.Options;
    using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using Uceme.Model.Models;

#pragma warning disable CA1501
    public class ApplicationDbContext : ApiAuthorizationDbContext<Uceme.Model.Models.Security.ApplicationUser>, IApplicationDbContext
#pragma warning restore CA1501
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {
        }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public DbSet<Blog> Blog { get; set; }

        public DbSet<Cita> Cita { get; set; }

        public DbSet<Companias> Companias { get; set; }

        public DbSet<Curriculum> Curriculum { get; set; }

        public DbSet<DatosContacto> DatosContacto { get; set; }

        public DbSet<DatosProfesionales> DatosProfesionales { get; set; }

        public DbSet<Documento> Documento { get; set; }

        public DbSet<Faq> Faq { get; set; }

        public DbSet<Fotos> Fotos { get; set; }

        public DbSet<ItemCurriculum> ItemCurriculum { get; set; }

        public DbSet<Menu> Menu { get; set; }

        public DbSet<PaginaAmiga> PaginaAmiga { get; set; }

        public DbSet<Rol> Rol { get; set; }

        public DbSet<Servicio> Servicio { get; set; }

        public DbSet<Tecnica> Tecnica { get; set; }

        public DbSet<Termino> Termino { get; set; }

        public DbSet<Turno> Turno { get; set; }

        public DbSet<Usuario> Usuario { get; set; }

        public DbSet<Video> Video { get; set; }

        public DbSet<UserProfile> UserProfile { get; set; }

        public DbSet<webpages_Membership> WebpagesMembership { get; set; }

        public DbSet<webpages_OAuthMembership> WebpagesOAuthMembership { get; set; }

        public DbSet<webpages_Roles> WebpagesRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            base.OnModelCreating(builder);
            builder.Entity<webpages_OAuthMembership>()
                .HasKey(c => new { c.Provider, c.ProviderUserId });
        }
    }
}
