using Kor_IT_ConsoleProject.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.Diagnostics;

namespace Kor_IT_ConsoleProject.SemanticKernel
{
	public class Step1_CreateKernel
	{
		private IConfiguration _configuration;
		private LLMOption _llmOption;

		public Step1_CreateKernel(IOptions<LLMOption> option)
        {
			//_configuration = configuration;
			_llmOption = option.Value;
        }

        //Kernel kernel = Kernel.CreateBuilder().AddAzureOpenAIChatCompletion("");


        public void Call()
		{
			//Console.WriteLine(_configuration["LLMOptions:AzureOpenAI"].ToString());
			_llmOption.AzureOpenAIOptions.ForEach(option =>
			{
				Console.WriteLine(option.DeployementName);
			});
		}

	}
}
