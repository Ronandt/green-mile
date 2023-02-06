using Microsoft.AspNetCore.Mvc;

using Web.Models;

namespace Web.UiState
{
    public class GroceryListUiState
    {


        [BindProperty]
        public string? DeleteId { get; set; }


    }
}
