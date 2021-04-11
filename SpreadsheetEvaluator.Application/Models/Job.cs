using Newtonsoft.Json;
using System.Collections.Generic;

namespace SpreadsheetEvaluator.Application.Models
{
    public class Job
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("data")]
        public ICollection<Cell> Data { get; set; }
    }
}
