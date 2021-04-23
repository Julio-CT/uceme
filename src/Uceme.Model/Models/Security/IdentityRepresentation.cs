namespace Uceme.Seguridad
{
    public class IdentityRepresentation
    {
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
