using Examen.Data;
using Examen.Data.Connection;
using Examen.Interfaces.DataServices;
using Examen.Interfaces.Services;
using Examen.Transversal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Examen.API
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
			services.AddControllers();
			services.AddSingleton<IConnection, Connection>();
			services.AddSingleton<IConfiguration>(Configuration);
			services.AddScoped<IActivityServicesDL, ActivityServices>();
			services.AddScoped<IPropertyServicesDL, PropertyServices>();
			services.AddScoped<IActivityServices, ActivityBusiness>();

			//Configuraciones de Swagger
			services.AddSwaggerGen();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseSwagger();
			app.UseSwaggerUI();
			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseAuthorization();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			//Configuración de middleware
			app.UseSwagger(config =>
			{
				config.SerializeAsV2 = true;
			});

			app.UseSwaggerUI(config =>
			{ 
				config.SwaggerEndpoint("/swagger/v1/swagger.json", "API SWAGGER!");
				config.RoutePrefix = string.Empty;
			});
		}
	}
}
