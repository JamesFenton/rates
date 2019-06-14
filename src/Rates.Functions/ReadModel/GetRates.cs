﻿using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace Rates.Functions.ReadModel
{
    public class GetRates
    {
        private readonly Database _database;

        public GetRates(Database database)
        {
            _database = database;
        }

        [FunctionName("GetRates")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "rates")] HttpRequestMessage req,
            [Blob("lookups/rates.json", FileAccess.Read)] List<Rate> rateLookupsJson,
            ILogger log)
        {
            // get all rates
            TableContinuationToken continuationToken = null;
            var query = new TableQuery<RateRm>();
            var rates = await _database.RatesRm.ExecuteQuerySegmentedAsync(query, continuationToken);

            // create the ordered DTOs
            //var rateLookups = JsonConvert.DeserializeObject<List<Rate>>(rateLookupsJson);
            var orderedRates = rateLookupsJson
                .Select(rate => rates.FirstOrDefault(r => r.Ticker == rate.Ticker))
                .Where(r => r != null)
                .ToList();

            var updateTime = orderedRates.Max(r => r.Timestamp).ToUnixTimeMilliseconds();

            // create response
            var response = new
            {
                rates = orderedRates,
                updateTime = updateTime
            };
            return req.CreateResponse(HttpStatusCode.OK, response, Constants.JsonFormatter);
        }
    }
}
