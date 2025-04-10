using System.Text.Json.Serialization;
using Coterie.Api.DTOs;
using Coterie.Api.ExceptionHelpers;
using Coterie.Api.Handlers;
using Coterie.Api.Interfaces;
using Coterie.Api.Models.Configs;
using Coterie.Api.Services;
using Coterie.Api.Services.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Coterie.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Coterie.Api", Version = "v1"});
            });

            services.AddScoped<ITestService, TestService>();
            services.AddScoped<IQuoteService, QuoteService>();
            services.AddScoped<IQuoteHandler, QuoteHandler>();
            services.Configure<QuoteFactorConfigs>(
                Configuration.GetSection("QuoteFactorConfigs"));
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<QuoteRequestDtoValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(appBuilder => appBuilder.UseMiddleware<ErrorHandlerMiddleware>());
            
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Coterie.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}