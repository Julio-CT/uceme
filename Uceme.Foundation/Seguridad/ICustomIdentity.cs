using System.Security.Principal;

namespace UCEME.Seguridad
{
    public interface ICustomIdentity : IIdentity
    {
        bool IsInRole(string rol);

        string ToJson();
    }
}