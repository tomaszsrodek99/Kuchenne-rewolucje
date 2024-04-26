using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Kuchenne_rewolucje.Helpers
{
    public class JwtAuthorizationMiddleware(RequestDelegate next, IConfiguration config)
    {
        private readonly RequestDelegate _next = next;
        private readonly IConfiguration _config = config;

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Cookies["JWTToken"];

            if (string.IsNullOrEmpty(token))
            {
                if (context.Request.Path.StartsWithSegments("/Auth"))
                {
                    await _next(context);
                    return;
                }
                else
                {
                    context.Response.Redirect("/Auth/ProfilesView");
                    return;
                }
            }
            else
            {
                if (IsTokenValid(token))
                {
                    AttachUserToContext(context, token);
                    if (context.Request.Path == "/" || context.Request.Path.StartsWithSegments("/Auth") && context.Request.Path != "/Auth/Logout" && context.Request.Path != "/Auth/RegisterView" && context.Request.Path != "/Auth/Register")
                    {
                        context.Response.Redirect("/Article/Index");
                        return;
                    }
                    else
                    {
                        await _next(context);
                        return;
                    }
                }
                else
                {
                    context.Response.Redirect("/Auth/ProfilesView");
                    return;
                }
            }
        }

        private void AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
                var parameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _config["Jwt:Audience"],
                    ValidateLifetime = false
                };

                var principal = tokenHandler.ValidateToken(token, parameters, out var validatedToken);
                context.User = principal;
            }
            catch
            {
                context.Response.Redirect("/Auth/ProfilesView");
            }
        }

        private bool IsTokenValid(string token)
        {
            if (token == null)
                return false;

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var parameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _config["Jwt:Audience"],
                    ValidateLifetime = false
                };

                var principal = tokenHandler.ValidateToken(token, parameters, out SecurityToken validatedToken);
                return true;
            }
            catch (SecurityTokenExpiredException)
            {
                return false;
            }
        }
    }

    public static class JwtAuthorizationExtensions
    {
        public static IApplicationBuilder UseJwtAuthorization(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtAuthorizationMiddleware>();
        }
    }
}
