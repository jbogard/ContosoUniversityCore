namespace ContosoUniversityCore
{
    using AutoMapper;
    using FluentValidation.AspNetCore;
    using HtmlTags;
    using Infrastructure;
    using Infrastructure.Tags;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc(opt =>
                {
                    opt.Filters.Add(typeof(DbContextTransactionFilter));
                    opt.Filters.Add(typeof(ValidatorActionFilter));
                    opt.ModelBinderProviders.Insert(0, new EntityModelBinderProvider());
                })
                .AddFeatureFolders()
                .AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup>(); });

            services.AddAutoMapper();

            Mapper.AssertConfigurationIsValid();

            services.AddMediatR();
            services.AddScoped(_ => new SchoolContext(Configuration["Data:DefaultConnection:ConnectionString"]));
            services.AddHtmlTags(new TagConventions());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}