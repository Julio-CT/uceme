﻿namespace Uceme.API.Data
{
    using IdentityServer4.EntityFramework.Options;
    using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using Uceme.API.Data.Models;
    using Uceme.Model.Models;

    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<webpages_OAuthMembership>()
                .HasKey(c => new { c.Provider, c.ProviderUserId });
        }

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
        public DbSet<sysdiagrams> sysdiagrams { get; set; }
        public DbSet<UserProfile> UserProfile { get; set; }
        public DbSet<webpages_Membership> webpages_Membership { get; set; }
        public DbSet<webpages_OAuthMembership> webpages_OAuthMembership { get; set; }
        public DbSet<webpages_Roles> webpages_Roles { get; set; }
    }
}
