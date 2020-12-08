using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Uceme.UI.Areas.Identity.IdentityHostingStartup))]
namespace Uceme.UI.Areas.Identity
{
    using System;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Uceme.Model.Data;
    using Uceme.Model.Models;

    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ConfigureServices((context, services) => {
            });
        }
    }
}