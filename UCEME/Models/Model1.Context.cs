﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UCEME.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class UCEMEDbEntities : DbContext
    {
        public UCEMEDbEntities()
            : base("name=UCEMEDbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
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