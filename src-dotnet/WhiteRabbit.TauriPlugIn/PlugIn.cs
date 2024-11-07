namespace WhiteRabbit.TauriPlugIn;

using Microsoft.Extensions.DependencyInjection;
using TauriDotNetBridge.Contracts;

public class PlugIn : IPlugIn
{
    public void Initialize(IServiceCollection services)
    {
        services.AddSingleton<DataStore>();
        services.AddSingleton<ActivitiesController>();
        services.AddSingleton<ReportController>();
    }
}