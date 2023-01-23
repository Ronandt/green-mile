using System.Diagnostics.Metrics;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Web.Services;

namespace Web.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerateImageController : ControllerBase
    {
        private readonly OpenAIApiService _openAIApiService;
        public  GenerateImageController(OpenAIApiService openAIApiService)
        {
            _openAIApiService = openAIApiService;
        }
        // GET: api/generateimage/50000
        [HttpGet("{prompt}")]
        public async Task<ActionResult<string>> GenerateImage(string prompt)
        {
            try
            {
                string image = await _openAIApiService.GenerateImage(prompt);
                return Ok(image);
            }
            catch (Exception)
            {
                return BadRequest("Technical Error");
            }
        }
    }
}
