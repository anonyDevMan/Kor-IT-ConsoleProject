using Kor_IT_ConsoleProject.Options;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace Kor_IT_ConsoleProject.SemanticKernel
{
    public class Step1_CreateKernel
    {
        private LLMOption _llmOption;
        private AzureOpenAIOption _azureOpenAIOption;
        private Kernel _kernel;

        public Step1_CreateKernel(IOptions<LLMOption> llmOption)
        {
            _llmOption = llmOption.Value;

            _azureOpenAIOption = _llmOption.AzureOpenAIOptions.FirstOrDefault();

            _kernel = Kernel.CreateBuilder().AddAzureOpenAIChatCompletion(_azureOpenAIOption.DeployementName, _azureOpenAIOption.Endpoint, _azureOpenAIOption.Key).Build();
        }

        public async Task Call()
        {
            Console.WriteLine("Start Call \n");

            try
            {
                #region Step 1

                //var promptResult = await _kernel.InvokePromptAsync("What color is the sky?");

                //Console.WriteLine(promptResult);

                #endregion Step 1

                #region Step 2

                //KernelArguments kernelArguments = new KernelArguments() { { "topic", "sea" } };
                //var promptResult = await _kernel.InvokePromptAsync("What color is the {{$topic}}? Please, answer is Korean.", kernelArguments);

                //Console.WriteLine(promptResult);

                #endregion Step 2

                #region Step 3

                //KernelArguments kernelArguments = new KernelArguments() { { "topic", "sea" } };

                //await foreach(var stream in _kernel.InvokePromptStreamingAsync("What color is the {{$topic}}? Provide a detailed explantion. At least 500 characters. Please, answer is Korean.", kernelArguments))
                //{
                //    if (stream.Metadata["FinishReason"] is not null && stream.Metadata["FinishReason"].Equals("stop"))
                //        Console.WriteLine();
                //    else
                //        Console.Write(stream);
                //}

                #endregion Step 3

                #region Step 4

                //KernelArguments kernelArguments = new KernelArguments(new OpenAIPromptExecutionSettings { MaxTokens = 500, Temperature = 0.5 }) { { "topic", "dog" } };

                //var promptResult = await _kernel.InvokePromptAsync("Tell me story about {{$topic}}? Please, answer is Korean.", kernelArguments);

                //Console.WriteLine(promptResult);

                #endregion Step 4
            }
            catch (Exception ex)
            {
            }

            Console.WriteLine("\n End Call");
        }
    }
}