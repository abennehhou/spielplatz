using System;
using System.IO;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Playground.Repositories;
using Playground.Services;
using PlaygroundApi.Mapping;
using PlaygroundApi.Middlewares;
using PlaygroundApi.Validation;
using Swashbuckle.AspNetCore.Swagger;

namespace PlaygroundApi
{
    public class Startup
    {
        public static readonly string ApiXmlComments = "PlaygroundApi.xml";
        public static readonly string DtoXmlComments = "Playground.Dto.xml";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Versioning and versioned documentation using swagger
            services.AddMvcCore().AddVersionedApiExplorer(o =>
            {
                o.GroupNameFormat = "F";
                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                o.SubstituteApiVersionInUrl = true;
            });
            services.AddMvc().AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<ItemDtoValidator>();
                fv.ImplicitlyValidateChildProperties = true;
            });

            services.AddCors();

            services.AddApiVersioning(
                o =>
                {
                    o.AssumeDefaultVersionWhenUnspecified = true;
                    o.DefaultApiVersion = new ApiVersion(1, 0);
                    o.ReportApiVersions = true;
                });

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerDoc(description.GroupName, new Info()
                        {
                            Title = $"Playground {description.ApiVersion}",
                            Version = description.ApiVersion.ToString()
                        });
                }

                // Set the comments path for the Swagger JSON and UI.
                var basePath = AppContext.BaseDirectory;
                c.IncludeXmlComments(Path.Combine(basePath, ApiXmlComments));
                c.IncludeXmlComments(Path.Combine(basePath, DtoXmlComments));
            });

            // To display swagger at startup, change "launchUrl" to "swagger" in launchSettings.json (ignored by .gitignore)

            // Dependency injection
            services.AddTransient<IItemsRepository, ItemsRepository>();
            services.AddTransient<IItemsService, ItemsService>();

            var mapper = AutoMapperConfig.Configure();
            services.AddTransient<IMapper, IMapper>(c => mapper);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            app.UseMiddleware(typeof(LoggingMiddleware));
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });
        }
    }
}
