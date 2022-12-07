using Microsoft.AspNetCore.Builder;

namespace api.rebel_wings.Extensions
{
    public static class ExceptionExtensions
    {
        public static void ConfigureGlobalException(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandler.ExceptionHandler>();
        }
    }
}
