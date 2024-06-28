using Kor_IT_ConsoleProject.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace Kor_IT_ConsoleProject.SemanticKernel
{
	public class Step3_DependencyInjection : IStep
	{
		private LLMOption _llmOption;
		private AzureOpenAIOption _azureOpenAIOption;
		private ServiceProvider _serviceProvider;
		private Kernel _kernel;

		public Step3_DependencyInjection(IOptions<LLMOption> llmOption)
		{
			_llmOption = llmOption.Value;

			_azureOpenAIOption = _llmOption.AzureOpenAIOptions.FirstOrDefault();

			_serviceProvider = BuildServiceProvider();
			_kernel = _serviceProvider.GetRequiredService<Kernel>();
		}

		public async Task Call()
		{
			OpenAIPromptExecutionSettings settings = new() { ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions };

			KernelArguments arguments = new(settings) { { "topic", "sea" } };

			await foreach (var update in _kernel.InvokePromptStreamingAsync("What color is the {{$topic}}? Provide a detailed explanation.", arguments))
			{
				Console.Write(update);
			}

			Console.WriteLine();

			await foreach(var update in _kernel.InvokePromptStreamingAsync("What time is it now?", arguments))
			{
				Console.Write(update);
			}

			Console.WriteLine();
		}

		private ServiceProvider BuildServiceProvider()
		{
			var collection = new ServiceCollection();

			var kernelBuilder = collection.AddKernel();
			kernelBuilder.Services.AddAzureOpenAIChatCompletion(_azureOpenAIOption.DeployementName, _azureOpenAIOption.Endpoint, _azureOpenAIOption.Key);
			//kernelBuilder.Services.AddAzureOpenAITextGeneration(_azureOpenAIOption.DeployementName, _azureOpenAIOption.Endpoint, _azureOpenAIOption.Key); 구 버전 LLM 모델 사용시 가능
			kernelBuilder.Plugins.AddFromType<TimeInformation>();

			return collection.BuildServiceProvider();
		}
	}
}