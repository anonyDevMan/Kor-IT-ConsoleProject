using Kor_IT_ConsoleProject.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kor_IT_ConsoleProject.SemanticKernel
{
	public class Step5_ResponsibleAI : IStep
	{
		private LLMOption _llmOption;
		private AzureOpenAIOption _azureOpenAIOption;
		private Kernel _kernel;

		public Step5_ResponsibleAI(IOptions<LLMOption> llmOption)
		{
			_llmOption = llmOption.Value;

			_azureOpenAIOption = _llmOption.AzureOpenAIOptions.FirstOrDefault();

			IKernelBuilder kernelBuilder = Kernel.CreateBuilder();
			kernelBuilder.AddAzureOpenAIChatCompletion(_azureOpenAIOption.DeployementName, _azureOpenAIOption.Endpoint, _azureOpenAIOption.Key);

			// Add prompt filter to the kernel
#pragma warning disable SKEXP0001 // 형식은 평가 목적으로 제공되며, 이후 업데이트에서 변경되거나 제거될 수 있습니다. 계속하려면 이 진단을 표시하지 않습니다.
			kernelBuilder.Services.AddSingleton<IPromptRenderFilter, PromptFilter>();
#pragma warning restore SKEXP0001 // 형식은 평가 목적으로 제공되며, 이후 업데이트에서 변경되거나 제거될 수 있습니다. 계속하려면 이 진단을 표시하지 않습니다.

			_kernel = kernelBuilder.Build();
		}

		public async Task Call()
		{
			KernelArguments arguments = new() { { "card_number", "4444 3333 2222 1111" } };

			var result = await _kernel.InvokePromptAsync("Tell me some useful information about this credit card number {{$card_number}}?", arguments);

			Console.WriteLine(result);

			// Output: Sorry, but I can't assist with that.
		}

#pragma warning disable SKEXP0001 // 형식은 평가 목적으로 제공되며, 이후 업데이트에서 변경되거나 제거될 수 있습니다. 계속하려면 이 진단을 표시하지 않습니다.
		private class PromptFilter() : IPromptRenderFilter
#pragma warning restore SKEXP0001 // 형식은 평가 목적으로 제공되며, 이후 업데이트에서 변경되거나 제거될 수 있습니다. 계속하려면 이 진단을 표시하지 않습니다.
		{
			/// <summary>
			/// Method which is called asynchronously before prompt rendering.
			/// </summary>
			/// <param name="context">Instance of <see cref="PromptRenderContext"/> with prompt rendering details.</param>
			/// <param name="next">Delegate to the next filter in pipeline or prompt rendering operation itself. If it's not invoked, next filter or prompt rendering won't be invoked.</param>
#pragma warning disable SKEXP0001 // 형식은 평가 목적으로 제공되며, 이후 업데이트에서 변경되거나 제거될 수 있습니다. 계속하려면 이 진단을 표시하지 않습니다.
			public async Task OnPromptRenderAsync(PromptRenderContext context, Func<PromptRenderContext, Task> next)
#pragma warning restore SKEXP0001 // 형식은 평가 목적으로 제공되며, 이후 업데이트에서 변경되거나 제거될 수 있습니다. 계속하려면 이 진단을 표시하지 않습니다.
			{
				if (context.Arguments.ContainsName("card_number"))
				{
					context.Arguments["card_number"] = "**** **** **** ****";
				}

				await next(context);

				context.RenderedPrompt += " NO SEXISM, RACISM OR OTHER BIAS/BIGOTRY";

				Console.WriteLine(context.RenderedPrompt);
			}
		}
	}
}
