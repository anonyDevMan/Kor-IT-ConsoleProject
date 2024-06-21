using Kor_IT_ConsoleProject.Options;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Kor_IT_ConsoleProject.SemanticKernel
{
	public class Step2_AddPlugins
	{
		private LLMOption _llmOption;
		private AzureOpenAIOption _azureOpenAIOption;
		private Kernel _kernel;

        public Step2_AddPlugins(IOptions<LLMOption> llmOption)
        {
			_llmOption = llmOption.Value;

			_azureOpenAIOption = _llmOption.AzureOpenAIOptions.FirstOrDefault();

			IKernelBuilder kernelBuilder = Kernel.CreateBuilder();
			kernelBuilder.AddAzureOpenAIChatCompletion(_azureOpenAIOption.DeployementName, _azureOpenAIOption.Endpoint, _azureOpenAIOption.Key);
			kernelBuilder.Plugins.AddFromType<TimeInformation>();
			kernelBuilder.Plugins.AddFromType<WidgetFactory>();
			//_kernel = Kernel.CreateBuilder().AddAzureOpenAIChatCompletion(_azureOpenAIOption.DeployementName, _azureOpenAIOption.Endpoint, _azureOpenAIOption.Key).Build();

			_kernel = kernelBuilder.Build();
		}

		public async Task Call()
		{
			Console.WriteLine("Start Call \n");

			try
			{
				#region Example 1

				//Console.WriteLine(await _kernel.InvokePromptAsync("How many days until Christmas?"));

				#endregion

				#region Example 2

				//Console.WriteLine(await _kernel.InvokePromptAsync("The current time is {{TimeInformation.GetCurrentUtcTime}}. How many days until Christmas?"));
				//Console.WriteLine(await _kernel.InvokePromptAsync("The current time is {{TimeInformation.GetCurrentUtcTime}}"));


				var result = await _kernel.InvokePromptAsync("The local current time is {{TimeInformation.GetCurrentLocalTime}}. Convert format is yyyy-mm-dd hh:mm:ss");

				Console.WriteLine(result);

				#endregion

				#region Example 3. Invoke the kernel with a prompt and allow the AI to automatically invoke functions

				OpenAIPromptExecutionSettings settings = new() { ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions };
				Console.WriteLine(await _kernel.InvokePromptAsync("What time is it local time?", new(settings)));
				Console.WriteLine(await _kernel.InvokePromptAsync("What time is it UTC time?", new(settings)));
				Console.WriteLine(await _kernel.InvokePromptAsync("How many days until Christmas? Explain your thinking.", new(settings)));

				#endregion

				#region Example 4. Invoke the kernel with a prompt and allow the AI to automatically invoke functions that use enumerations

				Console.WriteLine(await _kernel.InvokePromptAsync("Create a handy lime colored widget for me.", new(settings)));
				Console.WriteLine(await _kernel.InvokePromptAsync("Create a beautiful scarlet colored widget for me.", new(settings)));
				Console.WriteLine(await _kernel.InvokePromptAsync("Create an attractive maroon and navy colored widget for me.", new(settings)));

				#endregion
			}
			catch (Exception ex)
			{
			}

			Console.WriteLine("\n End Call");
		}
	}

	/// <summary>
	/// A plugin that returns the current time.
	/// </summary>
	public class TimeInformation
	{
		[KernelFunction]
		[Description("Retrieves the current time in UTC.")]
		public string GetCurrentUtcTime() => DateTime.UtcNow.ToString("R");

		[KernelFunction]
		[Description("Retrieves the current time in Local.")]
		public string GetCurrentLocalTime() => DateTime.Now.ToString("R");
	}

	/// <summary>
	/// A plugin that creates widgets.
	/// </summary>
	public class WidgetFactory
	{
		[KernelFunction]
		[Description("Creates a new widget of the specified type and colors")]
		public WidgetDetails CreateWidget([Description("The type of widget to be created")] WidgetType widgetType, [Description("The colors of the widget to be created")] WidgetColor[] widgetColors)
		{
			var colors = string.Join('-', widgetColors.Select(c => c.ToString()).ToArray());
			return new()
			{
				SerialNumber = $"{widgetType}-{colors}-{Guid.NewGuid()}",
				Type = widgetType,
				Colors = widgetColors
			};
		}
	}

	/// <summary>
	/// A <see cref="JsonConverter"/> is required to correctly convert enum values.
	/// </summary>
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum WidgetType
	{
		[Description("A widget that is useful.")]
		Useful,

		[Description("A widget that is decorative.")]
		Decorative
	}

	/// <summary>
	/// A <see cref="JsonConverter"/> is required to correctly convert enum values.
	/// </summary>
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum WidgetColor
	{
		[Description("Use when creating a red item.")]
		Red,

		[Description("Use when creating a green item.")]
		Green,

		[Description("Use when creating a blue item.")]
		Blue
	}

	public class WidgetDetails
	{
		public string SerialNumber { get; init; }
		public WidgetType Type { get; init; }
		public WidgetColor[] Colors { get; init; }
	}
}
