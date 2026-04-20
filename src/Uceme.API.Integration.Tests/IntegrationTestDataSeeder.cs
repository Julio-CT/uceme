namespace Uceme.API.Integration.Tests;

using System;
using Uceme.Model.Data;
using Uceme.Model.Models;

internal static class IntegrationTestDataSeeder
{
    internal static void SeedBlog(ApplicationDbContext db)
    {
        db.Blog.AddRange(
            new Blog
            {
                idBlog = 1001,
                titulo = "Snapshot post oldest",
                fecha = new DateTime(2020, 1, 10, 0, 0, 0, DateTimeKind.Utc),
                foto = "f1",
                texto = "body1",
                slug = "snap-oldest",
                seoTitle = "seo1",
                metaDescription = "meta1",
                idUsuario = 16,
            },
            new Blog
            {
                idBlog = 1002,
                titulo = "Snapshot post middle",
                fecha = new DateTime(2020, 2, 10, 0, 0, 0, DateTimeKind.Utc),
                foto = "f2",
                texto = "body2",
                slug = "snap-middle",
                seoTitle = "seo2",
                metaDescription = "meta2",
                idUsuario = 16,
            },
            new Blog
            {
                idBlog = 1003,
                titulo = "Snapshot post newest",
                fecha = new DateTime(2020, 3, 10, 0, 0, 0, DateTimeKind.Utc),
                foto = "f3",
                texto = "body3",
                slug = "snap-newest",
                seoTitle = "seo3",
                metaDescription = "meta3",
                idUsuario = 16,
            });
        db.SaveChanges();
    }

    internal static void SeedHospitalTechniqueHome(ApplicationDbContext db)
    {
        db.DatosProfesionales.Add(
            new DatosProfesionales
            {
                idDatosPro = 7001,
                nombre = "Snapshot Hospital",
                telefono = "900000001",
                email = "hospital@example.test",
                direccion = "Test Street 1",
                texto = "hospital text",
                foto = "h.jpg",
                activo = true,
            });
        db.Tecnica.Add(
            new Tecnica
            {
                idTecnica = 5001,
                titulo = "Technique snapshot",
                fecha = new DateTime(2019, 5, 1, 0, 0, 0, DateTimeKind.Utc),
                foto = "t.jpg",
                texto = "technique body",
                nombre = "TechniqueName",
            });
        db.Usuario.Add(
            new Usuario
            {
                idUsuario = 9001,
                nombre = "Doc",
                apellidos = "Snapshot",
                nick = "ds",
                login = "doc.snap",
                foto = "doc.jpg",
                idCurriculum = 1,
                idDatosContacto = 1,
                idRol = 2,
                display_order = 1,
            });
        db.Fotos.Add(
            new Foto
            {
                idFoto = 6001,
                nombre = "featured",
                texto = "caption",
                destacada = true,
                posicion = 1,
            });
        db.SaveChanges();
    }

    internal static void SeedTurnosForAppointment(ApplicationDbContext db)
    {
        db.DatosProfesionales.Add(
            new DatosProfesionales
            {
                idDatosPro = 7101,
                nombre = "Appt Hospital",
                activo = true,
            });
        db.Turno.Add(
            new Turno
            {
                idTurno = 8001,
                dia = 3,
                inicio = 9.0m,
                fin = 10.0m,
                paralelas = 1,
                porhora = 1,
                idHospital = 7101,
            });
        db.SaveChanges();
    }

    internal static void SeedBlogForMutation(ApplicationDbContext db)
    {
        db.Blog.Add(
            new Blog
            {
                idBlog = 2001,
                titulo = "To delete",
                fecha = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                foto = "x",
                texto = "x",
                slug = "to-delete",
                seoTitle = "x",
                metaDescription = "x",
                idUsuario = 16,
            });
        db.SaveChanges();
    }
}
