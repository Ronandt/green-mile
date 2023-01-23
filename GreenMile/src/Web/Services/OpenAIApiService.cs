using OpenAI.GPT3.Interfaces;

namespace Web.Services
{
    public class OpenAIApiService
    {
        private readonly IOpenAIService _openAIService;
        public OpenAIApiService(IOpenAIService openAIService)
        {
            _openAIService = openAIService;
            _openAIService.SetDefaultModelId(OpenAI.GPT3.ObjectModels.Models.TextDavinciV3);
        }

        public async Task<string> GenerateImage(string prompt)
        {
           
            if(prompt is null)
            {
                throw new ArgumentNullException(nameof(prompt));
            }
            var imageResult = await _openAIService.Image.CreateImage(new OpenAI.GPT3.ObjectModels.RequestModels.ImageCreateRequest
            {
                Prompt = prompt,
                N = 1,
                Size = OpenAI.GPT3.ObjectModels.StaticValues.ImageStatics.Size.Size256,
                ResponseFormat = OpenAI.GPT3.ObjectModels.StaticValues.ImageStatics.ResponseFormat.Url,
                User = "TestUser"
            });


            if (imageResult.Successful)
            {
                return string.Join("\n", imageResult.Results[0].Url);
            }
            throw new InvalidProgramException(nameof(prompt));

        }
    }
}
