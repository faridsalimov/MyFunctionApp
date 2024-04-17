using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MyFunctionApp
{
    public static class Function1
    {
        public static List<Player> Players { get; set; } = new List<Player>();

        [FunctionName("GetAll")]
        public static async Task<IActionResult> GetAll(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string responseMessage = "";

            foreach (var player in Players)
            {
                responseMessage += $"{player.Id} - {player.Name} - {player.Surname} - {player.Score} - {player.TeamId}\n";
            }

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("AddPlayer")]
        public static async Task<IActionResult> AddPlayer(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var newPlayer = JsonConvert.DeserializeObject<Player>(requestBody);

            Players.Add(newPlayer);

            return new OkObjectResult("Player added successfully.");
        }
    }
}
