using Kor_IT_ConsoleProject.Options;
using Kor_IT_ConsoleProject.SemanticKernel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        IConfiguration config = builder.Build();

        IServiceCollection services = new ServiceCollection();
        services.Configure<LLMOption>(config.GetSection("LLMOptions"));

        services.AddTransient<Step1_CreateKernel>();
        services.AddTransient<Step2_AddPlugins>();

        var serviceProvider = services.BuildServiceProvider();

        var step1 = serviceProvider.GetService<Step2_AddPlugins>();

        try
        {
            Task.Run(async () => await step1.Call()).Wait();
        }
        catch (Exception ex)
        {
        }
    }
}