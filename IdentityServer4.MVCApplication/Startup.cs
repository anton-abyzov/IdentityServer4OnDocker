using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IdentityServer4.MVCApplication
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            services.Configure<AppSettings>(Configuration);
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

            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationScheme = "Cookies"
            });

            var identityUrl = Configuration.GetValue<string>("IdentityUrl");

            //app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions()
            //{
            //    AuthenticationScheme = "oidc",
            //    SignInScheme = "Cookies",
            //    Authority = identityUrl,
            //    RequireHttpsMetadata = false,

            //    ClientId = "mvc",
            //    SaveTokens = true
            //});

            app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions
            {
                AuthenticationScheme = "oidc",
                SignInScheme = "Cookies",

                Authority = identityUrl,
                RequireHttpsMetadata = false,

                ClientId = "mvc",
                ClientSecret = "topsecret",

                //SignedOutCallbackPath = new PathString(""),

                ResponseType = "code id_token",
                Scope = { "api1", "offline_access" },

                GetClaimsFromUserInfoEndpoint = true,
                SaveTokens = true,
                Events = new OpenIdConnectEvents()
                {
                    OnAuthorizationCodeReceived = context =>
                    {
                        return Task.FromResult(0);
                    },

                    OnTicketReceived = context =>
                    {
                        return Task.FromResult(0);
                    },

                    OnAuthenticationFailed = context =>
                    {
                        return Task.FromResult(0);
                    },

                    OnTokenValidated = TokenValidatedContext =>
                    {

                        //var newClaimsIdentity = new ClaimsIdentity(
                        //        TokenValidatedContext.Ticket.AuthenticationScheme, "given_name", "role"
                        //    );
                        //TokenValidatedContext.Ticket = new AuthenticationTicket(
                        //    new ClaimsPrincipal(newClaimsIdentity),
                        //    TokenValidatedContext.Ticket.Properties,
                        //    TokenValidatedContext.Ticket.AuthenticationScheme
                        //    );

                        return Task.FromResult(0);
                    },

                    OnUserInformationReceived = UserInformationReceivedContext =>
                    {
                        return Task.FromResult(0);
                    }
                }
            });

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();

        }
    }
}
