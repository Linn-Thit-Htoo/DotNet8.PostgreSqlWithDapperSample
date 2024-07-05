using DotNet8.PostgreSqlWithDapperSample.Features.Blog;
using Microsoft.Extensions.DependencyInjection;

namespace DotNet8.PostgreSqlWithDapperSample
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddFeatures(this IServiceCollection services)
        {
            return services.AddDataAccessService().AddBusinessLogicService().AddCustomService();
        }

        private static IServiceCollection AddDataAccessService(this IServiceCollection services)
        {
            return services.AddScoped<DA_Blog>();
        }

        private static IServiceCollection AddBusinessLogicService(this IServiceCollection services)
        {
            return services.AddScoped<BL_Blog>();
        }

        private static IServiceCollection AddCustomService(this IServiceCollection services)
        {
            return services.AddScoped<DapperService>();
        }
    }
}
