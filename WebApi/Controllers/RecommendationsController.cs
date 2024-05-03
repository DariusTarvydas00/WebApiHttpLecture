using Accord.Math.Distances;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApi.ServiceLayer.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationsController : ControllerBase
    {
        private readonly IRecommendationsService _recommendationService;
        private readonly IUserService _userService;

        public RecommendationsController(IRecommendationsService recommendationsService, IUserService userService)
        {
            _recommendationService = recommendationsService;
            _userService = userService;
        }

        [HttpGet("RecommendationTest")]
        public IActionResult GetRecommendationsTEST()
        {
            Cosine cosine = new Cosine();

            double[] a = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            double[] b = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            double distanceab = Double.Round(cosine.Distance(a, b), 9);

            double[] c = { 1, 0, 3, 0, 5, 0, 7, 8, 9 };
            double[] d = { 1, 2, 0, 4, 0, 6, 7, 8, 9 };
            double distancecd = Double.Round(cosine.Distance(c, d), 9);

            double[] e = { 1, 2, 0, 3, 0, 4, 0, 5, 0, 6, 0, 7, 8, 9 };
            double[] f = { 1, 0, 2, 0, 3, 0, 4, 0, 5, 0, 6, 7, 8, 9 };
            double distanceef = Double.Round(cosine.Distance(e, f), 9);

            List<double> distances = [];
            distances.Add(distanceab);
            distances.Add(distancecd);
            distances.Add(distanceef);

            List<double> mydistances = [];
            mydistances.Add(Double.Round(1 - MyCosineFormula(a, b), 9));
            mydistances.Add(Double.Round(1 - MyCosineFormula(c, d), 9));
            mydistances.Add(Double.Round(1 - MyCosineFormula(e, f), 9));

            return Ok(new { distances, mydistances });

            //rasti useri kuris yra panasus i useri kuris yra prisijunges ir pasiulyti viena is jo rekomendaciju - user-based recommendation
            
            //double[] item1 = {user1rating, user2rating, user3rating}; (order is important)
            //double[] item2 = {user1rating, user2rating, user3rating}; (order is important)
            //the above gives us vectors by which we can calculate the similarity between the two items
            //when we have the similarity between the two items we can recommend the item with the highest similarity to the logged in user
            //but this will not have qualitative features accounted for, i still want to recoment harry potter 2 to someone who liked harry potter 1, even if i dont know the ratings
        }

        private double MyCosineFormula(double[] a, double[] b)
        {
            double dotProduct = 0;
            double normA = 0;
            double normB = 0;
            for (int i = 0; i < a.Length; i++)
            {
                dotProduct += a[i] * b[i];
                normA += a[i] * a[i];
                normB += b[i] * b[i];
            }
            return dotProduct / (Math.Sqrt(normA) * Math.Sqrt(normB));
        }


        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetRecommendations([FromQuery] string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Username is required");
            }
            if ((await _userService.GetByUserName(username)) is null)
            {
                return NotFound("User not found");
            }

            try
            {
                DateTime start = DateTime.Now;
                var results = await _recommendationService.GetRecommendations(username);
                DateTime end = DateTime.Now;
                return Ok(new { results, started = start, ended = end});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
