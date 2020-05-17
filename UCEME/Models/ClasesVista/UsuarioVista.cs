namespace Uceme.Model.Models.ClasesVista
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UsuarioVista
    {
        public int IdUsuario { get; set; }

        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Apellidos")]
        public string Apellidos { get; set; }

        [Display(Name = "Nick")]
        public string Nick { get; set; }

        [Display(Name = "Login")]
        public string Login { get; set; }

        [Display(Name = "Foto")]
        public string Foto { get; set; }

        public DateTime? UltimoUpdate { get; set; }

        public int IdRol { get; set; }

        public int IdCurriculum { get; set; }

        public int IdDatosContacto { get; set; }

        public string Password { get; set; }

        [Display(Name = "Newsletter")]
        public bool? Newsletter { get; set; }

        public string Linkedin { get; set; }

        public virtual Curriculum Curriculum { get; set; }

        public virtual DatosContacto DatosContacto { get; set; }
    }
}