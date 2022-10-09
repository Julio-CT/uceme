namespace Uceme.Model.Data
{
    using IdentityServer4.EntityFramework.Interfaces;
    using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Uceme.Model.Models;

    public interface IApplicationDbContext
    {
        DbSet<Blog> Blog { get; set; }

        DbSet<Cita> Cita { get; set; }

        DbSet<Companias> Companias { get; set; }

        DbSet<Curriculum> Curriculum { get; set; }

        DbSet<DatosContacto> DatosContacto { get; set; }

        DbSet<DatosProfesionales> DatosProfesionales { get; set; }

        DbSet<Documento> Documento { get; set; }

        DbSet<Faq> Faq { get; set; }

        DbSet<Fotos> Fotos { get; set; }

        DbSet<ItemCurriculum> ItemCurriculum { get; set; }

        DbSet<Menu> Menu { get; set; }

        DbSet<PaginaAmiga> PaginaAmiga { get; set; }

        DbSet<Rol> Rol { get; set; }

        DbSet<Servicio> Servicio { get; set; }

        DbSet<Tecnica> Tecnica { get; set; }

        DbSet<Termino> Termino { get; set; }

        DbSet<Turno> Turno { get; set; }

        DbSet<Usuario> Usuario { get; set; }

        DbSet<Video> Video { get; set; }

        DbSet<sysdiagrams> Sysdiagrams { get; set; }

        DbSet<UserProfile> UserProfile { get; set; }

        DbSet<webpages_Membership> WebpagesMembership { get; set; }

        DbSet<webpages_OAuthMembership> WebpagesOAuthMembership { get; set; }

        DbSet<webpages_Roles> WebpagesRoles { get; set; }
    }
}
