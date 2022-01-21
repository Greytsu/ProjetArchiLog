using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProjetArchiLog.API.Data;
using ProjetArchiLog.Library.Config;
using Serilog;

//Initialize Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting uuuuup");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers(options =>
    {
        options.Conventions.Add(new SwaggerConvention());
    });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        var titlebase = "API Version ";
        var desc = "Documentation de l'API";
        var termsofservice = new Uri("https://stupefied-poitras-d219ae.netlify.app");
        var license = new OpenApiLicense()
        {
            Name = "CORTA"
        };
        var contact = new OpenApiContact()
        {
            Name = "Olivier & Lucas",
            Email = "corta.app@gmail.com",
            Url = new Uri("https://stupefied-poitras-d219ae.netlify.app")
        };
        options.SwaggerDoc("v1", new OpenApiInfo
        {

            Version = "v1",
            Title = titlebase + "1",
            Description = desc,
            Contact = contact,
            License = license,
            TermsOfService = termsofservice
        });
        options.SwaggerDoc("v2", new OpenApiInfo
        {

            Version = "v2",
            Title = titlebase + "2",
            Description = desc,
            Contact = contact,
            License = license,
            TermsOfService = termsofservice
        });
    });
    builder.Services.AddDbContext<ArchiDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("Archi"))
    );

    builder.Services.AddMvcCore();

    //Wire Serilog into the WebApplicationBuilder
    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console()
        .ReadFrom.Configuration(ctx.Configuration));

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("v1/swagger.json", "V1"); 
            options.SwaggerEndpoint("v2/swagger.json", "V2"); 
        });
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.UseSerilogRequestLogging();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}