using System.Text.Json.Serialization;

using Newtonsoft.Json;

namespace Web.Models
{
    public class Result
    {
        public int id { get; set; }
        public int usedIngredientCount { get; set; }
        public int missedIngredientCount { get; set; }
        public List<MissedIngredient> missedIngredients { get; set; }
        public int likes { get; set; }
        public List<object> usedIngredients { get; set; }
        public List<object> unusedIngredients { get; set; }
        public string title { get; set; }
        public string image { get; set; }
        public string imageType { get; set; }
    }
}
