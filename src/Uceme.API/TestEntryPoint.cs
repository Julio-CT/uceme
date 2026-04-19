using Microsoft.AspNetCore.Builder;

namespace Uceme.API;

// Entry point class for testing
public class TestEntryPoint
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        Program.ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        Program.ConfigureMiddleware(app, builder.Environment, builder.Configuration);

        app.Run();
    }
}
