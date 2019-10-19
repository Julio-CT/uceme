using System;
using System.Security.Principal;

namespace UCEME.Seguridad
{
    public interface ICustomIdentity : IIdentity
    {
        bool IsInRole(String rol);

        string ToJson();
    }
}