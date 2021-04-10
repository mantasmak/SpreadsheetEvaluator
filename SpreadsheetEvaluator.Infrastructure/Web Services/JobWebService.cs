using SpreadsheetEvaluator.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SpreadsheetEvaluator.Infrastructure.Web_Services
{
    public class JobWebService : IJobWebService
    {
        private static readonly HttpClient client = new HttpClient();
        private string _hubUrl = "https://www.wix.com/_serverless/hiring-task-spreadsheet-evaluator/jobs";

        public async Task<string> GetJobsAsync()
        {
            return await client.GetStringAsync(_hubUrl);
        }

        public async Task<string> PostResultsAsync(string submissionUrl, string results)
        {
            var values = new Dictionary<string, string>
            {
                { "result", results }
            };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync(submissionUrl, content);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
