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
        services.AddTransient<Step3_DependencyInjection>();
        services.AddTransient<Step4_ChatPrompt>();
        services.AddTransient<Step5_ResponsibleAI>();

        var serviceProvider = services.BuildServiceProvider();

        var service = serviceProvider.GetService<Step5_ResponsibleAI>();

        try
        {
            Task.Run(async () => await service.Call()).Wait();
        }
        catch (Exception ex)
        {
        }
    }
}