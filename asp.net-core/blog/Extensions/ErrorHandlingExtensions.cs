namespace Blog.Extensions;

public static class ErrorHandlingExtensions
{
    public static IServiceCollection AddErrorHandling(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}