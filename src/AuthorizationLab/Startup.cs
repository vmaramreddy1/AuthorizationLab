﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AuthorizationLab
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddAuthorization(options =>
			{
				options.AddPolicy("AdministratorOnly", policy => policy.RequireRole("Administrator"));
				options.AddPolicy("EmployeeId", policy => policy.RequireClaim("EmployeeId", "123", "456"));
				options.AddPolicy("Over21Only", policy => policy.Requirements.Add(new MinimumAgeRequirement(21)));
				options.AddPolicy("BuildingEntry", policy => policy.Requirements.Add(new OfficeEntryRequirement()));
			});
			services.AddMvc(config =>
				{
					var policy = new AuthorizationPolicyBuilder()
									 .RequireAuthenticatedUser()
									 .Build();
					config.Filters.Add(new AuthorizeFilter(policy));
				});
			services.AddSingleton<IAuthorizationHandler, HasBadgeHandler>();
			services.AddSingleton<IAuthorizationHandler, HasTemporaryPassHandler>();
			services.AddSingleton<IDocumentRepository, DocumentRepository>();
			services.AddSingleton<IAuthorizationHandler, DocumentEditHandler>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			app.UseCookieAuthentication(new CookieAuthenticationOptions
			{
				AuthenticationScheme = "Cookie",
				LoginPath = new PathString("/Account/Login"),
				AccessDeniedPath = new PathString("/Account/Forbidden"),
				AutomaticAuthenticate = true,
				AutomaticChallenge = true
			});


			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "Dafault",
					template:"{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
