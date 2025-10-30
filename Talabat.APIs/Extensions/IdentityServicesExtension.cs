using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;
using Talabat.Repository.Identity.Config;
using Microsoft.IdentityModel.Tokens;
using Talabat.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

namespace Talabat.APIs.Extensions
{
    public static class IdentityServicesExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services,IConfiguration configuration)
        {

            services.AddScoped(typeof(IAuthService), typeof(AuthService));

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {

            }).AddEntityFrameworkStores<AppIdentityDbContext>();

            services.AddAuthentication(/*JwtBearerDefaults.AuthenticationScheme)*/ options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; //schema that Authorize end-point default challnge
            })

              .AddJwtBearer(options =>
              {

                  //configure authemtication handler
                  options.TokenValidationParameters = new TokenValidationParameters()
                  {
                      ValidateAudience = true,
                      ValidateIssuer = true,
                      ValidAudience = configuration["JWT:ValidAudience"],
                      ValidIssuer = configuration["JWT:Issure"],
                      ValidateIssuerSigningKey = true,
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"])),
                      ValidateLifetime = true,
                      ClockSkew = TimeSpan.FromDays(double.Parse(configuration["JWT:Duration"]))


                  };
              });
            

            return services;
        }
    }
}
