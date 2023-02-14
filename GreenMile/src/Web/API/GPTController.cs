using System.Text;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using PexelsDotNetSDK.Api;

using Web.Models;
using Web.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class GPTController : Controller
    {
        private readonly OpenAIApiService _openAIApiService;
        private readonly PexelsClient _pexelsClient;
        public GPTController(
            OpenAIApiService openAIApiService, PexelsClient pexelsClient
            ) { 
        _openAIApiService= openAIApiService;
            _pexelsClient= pexelsClient;
        }
        // GET: api/<GPTController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<GPTController>/5
        [HttpPost]
        public async Task<string> Post(Prompt prompt)
        {
            string prePrompt = $"You will think up of maximum 3 ingredients that make up a {prompt.Response} in the JSON format of Id, Name, Quantity (set this to 1), ExtraNote, InBasket (set this to false), Description (It must be a description of that ingredient), HouseholdId, CategoryId (CategoryId must be an integer that the food follows. 1 - Fruits, 2- Vegetables, 3- Meat, 4- Dairy, 5- Others), CarbonFootprint (set as 0), ImageFilePath (Set as null). The two ingredients have to be part of one array. Your output cannot have any extra text above or below the json array";
            OpenAI.GPT3.ObjectModels.ResponseModels.CompletionCreateResponse promptResult = await _openAIApiService.GenerateDavinciPrompt(prePrompt + prompt.Response);
            var objectified = JsonConvert.DeserializeObject<List<GroceryFoodItem>>(promptResult.Choices.FirstOrDefault()?.Text);






            string unobjectified;
            var modifiedObjects = new List<GroceryFoodItem>();
            foreach (var x in objectified)
            {
                PexelsDotNetSDK.Models.PhotoPage photoPage = await _pexelsClient.SearchPhotosAsync(x.Name, pageSize: 2);
                string photoUrl = photoPage.photos[0].source.original;
                x.ImageFilePath = photoUrl;
                modifiedObjects.Add(x);
            }
            unobjectified = JsonConvert.SerializeObject(modifiedObjects, Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return unobjectified;
        }

      

        // PUT api/<GPTController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GPTController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
