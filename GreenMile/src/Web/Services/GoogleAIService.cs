using System.Net;
using System.Runtime.InteropServices;

using Google.Cloud.Storage.V1;
using Google.Cloud.Vision.V1;

using static System.Net.Mime.MediaTypeNames;

namespace Web.Services
{
    public class GoogleAIService
    {
        private readonly IWebHostEnvironment _env;

        public GoogleAIService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<Google.Cloud.Vision.V1.Image> ConvertFileStreamToImage(IFormFile file)
        {

            byte[] imageBytes;
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                imageBytes = stream.ToArray();
            }

            return Google.Cloud.Vision.V1.Image.FromBytes(imageBytes);

            

        }

        public async Task<Google.Cloud.Vision.V1.Image> ConvertUriToImage(string uri)
        {
            byte[] imageBytes;
            if(uri.Contains("/uploads"))
            {
               var path =  Path.Combine(_env.ContentRootPath, "wwwroot", "uploads", uri.Split("/")[2]);
                imageBytes =await  System.IO.File.ReadAllBytesAsync(path);
            } else
            {
                using (WebClient client = new WebClient())
                {
                   imageBytes = await client.DownloadDataTaskAsync(uri);
                }
            
            }
            return Google.Cloud.Vision.V1.Image.FromBytes(imageBytes);
        }
        public async Task<string> IdentifyObject(Google.Cloud.Vision.V1.Image image)
        {
            ImageAnnotatorClient client = await ImageAnnotatorClient.CreateAsync();
            IReadOnlyList<LocalizedObjectAnnotation> annotations =await client.DetectLocalizedObjectsAsync(image);
            foreach (LocalizedObjectAnnotation annotation in annotations)
            {
                string poly = string.Join(" - ", annotation.BoundingPoly.NormalizedVertices.Select(v => $"({v.X}, {v.Y})"));
                return annotation.Name;
            }
            return "Unknown";
        }

        public async Task<string> IdentifyAverageImageColour(Google.Cloud.Vision.V1.Image image)
        {
            ImageAnnotatorClient client = await ImageAnnotatorClient.CreateAsync();
            ImageProperties properties = await client.DetectImagePropertiesAsync(image);
            ColorInfo dominantColor = properties.DominantColors.Colors.OrderByDescending(c => c.PixelFraction).First();
            var dominantColors = dominantColor.Color.Red;
            return $"rgb({dominantColor.Color.Red}, {dominantColor.Color.Green}, {dominantColor.Color.Blue})";
        }
    }
}
