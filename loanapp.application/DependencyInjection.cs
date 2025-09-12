using loanapp.application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace loanapp.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register services here
            services.AddScoped<ILoanApplicationService, LoanApplicationService>();

            return services;
        }
    }
}
