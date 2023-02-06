using Microsoft.AspNetCore.Mvc;

using Web.Models;
using Web.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroceryListController : ControllerBase
    {
        private readonly IGroceryFoodService _foodService;
        public GroceryListController(IGroceryFoodService foodService) {
            _foodService = foodService;
        
        
        }
        // GET: api/<GroceryListController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<GroceryListController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<GroceryListController>
        [HttpPost]
        public void Post([FromBody] string value)

        {

        }

        // PUT api/<GroceryListController>/5
        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] Value value)
        {
            await _foodService.ChangeQuantity(await _foodService.RetrieveGroceryItem(id), value.value);
        }

        // DELETE api/<GroceryListController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}


