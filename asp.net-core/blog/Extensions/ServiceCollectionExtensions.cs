using Blog.Services;
using Blog.Services.Interfaces;
using Blog.Repositories;
using Blog.Repositories.Interfaces;
using Blog.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IPostService, PostService>();

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
        return services;
    }
}