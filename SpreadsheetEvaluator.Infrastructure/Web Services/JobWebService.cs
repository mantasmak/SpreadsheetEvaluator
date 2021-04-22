using SpreadsheetEvaluator.Infrastructure.Interfaces;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEvaluator.Infrastructure.Web_Services
{
    public class JobWebService : IJobWebService
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly string _hubUrl = "https://www.wix.com/_serverless/hiring-task-spreadsheet-evaluator/jobs";

        public async Task<string> GetJobsAsync()
        {
            var result = await client.GetStringAsync(_hubUrl);

            return result;
        }

        public async Task<string> PostResultsAsync(string results, string submissionUrl)
        {
            var content = new StringContent(results, Encoding.UTF8, "application/json");
            var result = await client.PostAsync(submissionUrl, content);

            return await result.Content.ReadAsStringAsync();
        }
    }
}
