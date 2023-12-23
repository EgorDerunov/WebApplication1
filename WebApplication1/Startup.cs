using Contracts;
using Entities.DataTransferObjects.Book;
using Entities.DataTransferObjects.Employee;
using LoggerService;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Repository.DataShaping;
using WebApplication1.ActionFilter;
using WebApplication1.Extensions;

namespace WebApplication1;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(),
            "/nlog.config"));
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
        services.ConfigureCors();
        services.ConfigureIISIntegration();
        services.ConfigureLoggerService();
        services.ConfigureVersioning();

        services.ConfigureSqlContext(Configuration);
        services.ConfigureRepositoryManager();

        services.AddAuthentication();
        services.ConfigureIdentity();

        services.AddScoped<ValidationFilterAttribute>();
        services.AddScoped<ValidateCompanyExistsAttribute>();
        services.AddScoped<ValidateEmployeeForCompanyExistsAttribute>();
        services.AddScoped<ValidateAuthorExistsAttribute>();
        services.AddScoped<ValidateBookForAuthorExistsAttribute>();
        services.AddScoped<IDataShaper<EmployeeDto>, DataShaper<EmployeeDto>>();
        services.AddScoped<IDataShaper<BookDto>, DataShaper<BookDto>>();
        services.AddAutoMapper(typeof(Startup));
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddControllers(config =>
        {
            config.RespectBrowserAcceptHeader = true;
            config.ReturnHttpNotAcceptable = true;
        }).AddXmlDataContractSerializerFormatters()
        .AddCustomCSVFormatter();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerManager loggerManager) 
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.ConfigureExceptionHandler(loggerManager);
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseCors("CorsPolicy");
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.All
        });

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}