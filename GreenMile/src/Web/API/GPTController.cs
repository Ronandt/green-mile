using System.Text;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using PexelsDotNetSDK.Api;

using Web.Models;
using Web.Services;

using static System.Net.Mime.MediaTypeNames;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class GPTController : Controller
    {
        private readonly OpenAIApiService _openAIApiService;
        private readonly PexelsClient _pexelsClient;
        private readonly CategoryService _categoryService;

        public GPTController(
            OpenAIApiService openAIApiService, PexelsClient pexelsClient, CategoryService categoryService
            ) { 
        _openAIApiService= openAIApiService;
            _pexelsClient= pexelsClient;
            _categoryService= categoryService;
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
            string unobjectified;
            List<string> categories = (await _categoryService.RetrieveCategories()).Select(x => x.ToString()).ToList();



            using (HttpClient client = new HttpClient())
            {
                //https://api.spoonacular.com/recipes/complexSearch?query=pasta&number=1&apiKey=07467c3cb8b94f3d9883404544dc8f1b
                HttpResponseMessage response = await client.GetAsync($"https://api.spoonacular.com/recipes/complexSearch?query={prompt.Response}&number=1&apiKey=07467c3cb8b94f3d9883404544dc8f1b&fillIngredients=true");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    try
                    {
                       Root recipeResponse = JsonConvert.DeserializeObject<Root>(content);

                        if (recipeResponse.results.Count > 0)
                        {
                            IEnumerable<string> missedIngredients = recipeResponse.results[0].missedIngredients.Select(x => System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.name));


                            List<string> categoryValues = (await _openAIApiService.GenerateDavinciPrompt($"Categorise these ingredients with the following categoryids ({String.Join(",", categories)}):　{String.Join(",", recipeResponse.results[0].missedIngredients.Select(x => System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.name)))}. Only output in this format: number,number,number")).Choices.FirstOrDefault().Text.Split(",").ToList();
                            List<GroceryFoodItem> items = recipeResponse.results[0].missedIngredients.Zip(categoryValues, (x, y) =>
                            {

                                return new GroceryFoodItem()
                                {
                                    HouseholdId = 0,
                                    CarbonFootprint = 0,
                                    Description = "No description",
                                    ImageFilePath = x.image,
                                    CategoryId = Int32.Parse(y),
                                    Name = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.name),
                                    Quantity = 1

                                };
                            }).ToList();


                            unobjectified = JsonConvert.SerializeObject(items, Formatting.None, new JsonSerializerSettings()
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            });
                            return unobjectified;
                        }
                    } catch(Exception ex)
                    {
                        goto a;
                    }
             



                }
            }

            a:
                string prePrompt = $"You will think up of maximum 2 ingredients that make up a {prompt.Response} in the JSON format of Id, Name, Quantity (set this to 1), ExtraNote, InBasket (set this to false), Description (It must be a description of that ingredient), HouseholdId, CategoryId (CategoryId must be an integer that the food follows and cannot be null. {String.Join(",", categories)}), CarbonFootprint (set as 0), ImageFilePath (Set as null). The two ingredients have to be part of one array. Your output cannot have any extra text above or below the json array";
                OpenAI.GPT3.ObjectModels.ResponseModels.CompletionCreateResponse promptResult = await _openAIApiService.GenerateDavinciPrompt(prePrompt + prompt.Response);
                var objectified = JsonConvert.DeserializeObject<List<GroceryFoodItem>>(promptResult.Choices.FirstOrDefault()?.Text);







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
