namespace UCEME.Seguridad
{
    public class IdentityRepresentation
    {
        //la vamos a usar para serializar los datos de usuario.
        public bool IsAuthenticated
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public string Roles { get; set; }
    }
}