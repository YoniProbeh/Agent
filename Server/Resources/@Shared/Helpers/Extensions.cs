using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Server.Resources.Shared.Models;

namespace Server.Resources.Shared.Helpers
{
    public static class Extensions
    {
        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration Configuration)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value));
            var tokenParams = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                IssuerSigningKey = securityKey
            };
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(
                    options =>
                    {
                        options.TokenValidationParameters = tokenParams;
                    });
        }

        public static void AddPagination<T>(this HttpResponse response, PagedList<T> pagination)
        {
            Pagination pagedheader = new Pagination(pagination.CurrentPage, pagination.PageSize, pagination.TotalCount, pagination.TotalPages);
            JsonSerializerSettings formatting = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            response.Headers.Add("Pagination", JsonConvert.SerializeObject(pagedheader, formatting));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}