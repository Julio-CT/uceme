using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Uceme.UI.Areas.Identity.IdentityHostingStartup))]

namespace Uceme.UI.Areas.Identity;

using System;
using Microsoft.AspNetCore.Hosting;

public class IdentityHostingStartup : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.ConfigureServices((_, _) =>
        {
        });
    }
}
