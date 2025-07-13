using ExtendedValidation.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExtendedValidation.AspNet;

public static class DIRegister
{
    public static IServiceCollection AddExtendedValidation(this IServiceCollection services)
    {
        services.AddSingleton<ValidateService>();

        services.Configure<MvcOptions>(ex =>
            {
                ex.Filters.Add<RequestValidatorMiddlware>();
            }
        );
        
        return services;
    }

    public static IServiceProvider RegisterValitor<T>(this IServiceProvider provider)
    {
        var validator = provider.GetRequiredService<ValidateService>();
        
        var validatorInstance = ActivatorUtilities.GetServiceOrCreateInstance(provider, typeof(T));
        
        validator.addValidator((dynamic)validatorInstance);
        
        return provider;
    }
}