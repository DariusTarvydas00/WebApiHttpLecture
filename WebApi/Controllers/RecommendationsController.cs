﻿using Microsoft.AspNetCore.Mvc;
using Accord.Math.Distances;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class RecommendationsController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetRecommendations()
        {
            double[] a = { 1, 2 };
            double[] b = { 2, 4 };
            double[] c = { 2.5, 4 };
            double[] d = { 4.5, 5 };
            Cosine cosine = new Cosine();
            double distanceca = Double.Round(cosine.Distance(c, a),9);
            double distancecb = Double.Round(cosine.Distance(c, b),9);
            double distancecd = Double.Round(cosine.Distance(c, d),9);
            double distanceab = Double.Round(cosine.Distance(a, b),9);
            List<double> distances = new List<double> { distanceca, distancecb, distancecd, distanceab };
            return Ok(distances);

            //rasti useri kuris yra panasus i useri kuris yra prisijunges ir pasiulyti viena is jo rekomendaciju - user-based recommendation
            //the above is user-based recommendation, we can also do item-based recommendation


            //double[] item1 = {user1rating, user2rating, user3rating}; (order is important)
            //double[] item2 = {user1rating, user2rating, user3rating}; (order is important)
            //the above gives us vectors by which we can calculate the similarity between the two items
            //when we have the similarity between the two items we can recommend the item with the highest similarity to the logged in user
            //but this will not have qualitative features accounted for, i still want to recoment harry potter 2 to someone who liked harry potter 1, even if i dont know the ratings
        }
    }
}