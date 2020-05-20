namespace UCEME.Seguridad
{
    using System.Security.Principal;

    public interface ICustomIdentity : IIdentity
    {
        bool IsInRole(string rol);

        string ToJson();
    }
}