using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace ADOpenIDConnect
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
            services.AddControllersWithViews();

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = OpenIdConnectDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            }).AddOpenIdConnect(option =>
            {
                
                option.Authority = "https://login.microsoftonline.com/[TenantId]";
                option.ClientId = "[Client Id]";
                //You must enable the implicit grant flow in azure ad under the register app -> authentication setting
                //for this to work as expected. 
                option.ResponseType = OpenIdConnectResponseType.IdToken;
                option.CallbackPath = "/auth/signin-callback";
                option.SignedOutRedirectUri = "https://localhost:4432";
                option.TokenValidationParameters.NameClaimType = "name";

            }).AddCookie(option =>
            {
                option.LoginPath = "/auth/login";
                option.LogoutPath = "/auth/logout";
            });

            services.AddMvc();
        }

        //client Id 4c753863-092d-44d9-943c-9c27e28b5099
        //tenant Id 74c6bfb9-08e2-4a30-a7d1-a8ef361cd197


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
