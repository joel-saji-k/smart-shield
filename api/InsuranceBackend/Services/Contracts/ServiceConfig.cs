namespace InsuranceBackend.Services.Contracts
{
    public static class ServiceConfig
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {            
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IAgentService, AgentService>();
            services.AddScoped<IClientService, ClientService>();            

            return services;
        }
    }
}
