using Kor_IT_ConsoleProject.Options;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;

namespace Kor_IT_ConsoleProject.SemanticKernel
{
	public class Step4_ChatPrompt : IStep
	{
		private LLMOption _llmOption;
		private AzureOpenAIOption _azureOpenAIOption;
		private Kernel _kernel;

		public Step4_ChatPrompt(IOptions<LLMOption> llmOption)
		{
			_llmOption = llmOption.Value;

			_azureOpenAIOption = _llmOption.AzureOpenAIOptions.FirstOrDefault();

			IKernelBuilder kernelBuilder = Kernel.CreateBuilder();
			kernelBuilder.AddAzureOpenAIChatCompletion(_azureOpenAIOption.DeployementName, _azureOpenAIOption.Endpoint, _azureOpenAIOption.Key);

			_kernel = kernelBuilder.Build();
		}

		public async Task Call()
		{
			// Invoke the kernel with a chat prompt and display the result
			string chatPrompt = """
            <message role="user">What is Seattle?</message>
            <message role="system">Respond with JSON.</message>
            """;

			Console.WriteLine(await _kernel.InvokePromptAsync(chatPrompt));
		}
	}
}