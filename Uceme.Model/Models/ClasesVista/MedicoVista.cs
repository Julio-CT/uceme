namespace Uceme.Model.Models.ClasesVista
{
    public class MedicoVista
    {
        public int IdUsuario { get; set; }

        public string Nombre { get; set; }

        public string Apellidos { get; set; }

        public string Foto { get; set; }

        public string Login { get; set; }

        public int IdCurriculum { get; set; }

        public int IdDatosContacto { get; set; }

        public string Telefono { get; set; }

        public string Linkedin { get; set; }

        public Curriculum Curriculum { get; set; }
    }
}