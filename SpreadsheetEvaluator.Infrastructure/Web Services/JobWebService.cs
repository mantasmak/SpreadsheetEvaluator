using Newtonsoft.Json.Linq;
using SpreadsheetEvaluator.Infrastructure.Interfaces;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEvaluator.Infrastructure.Web_Services
{
    public class JobWebService : IJobWebService
    {
        private static readonly HttpClient client = new HttpClient();
        private string _hubUrl = "https://www.wix.com/_serverless/hiring-task-spreadsheet-evaluator/jobs";
        private string _submissionUrl;

        public async Task<string> GetJobsAsync()
        {
            var result = await client.GetStringAsync(_hubUrl);

            SaveSubmissionUrl(result);

            return result;
        }

        public async Task<string> PostResultsAsync(string results)
        {
            var content = new StringContent(results, Encoding.UTF8, "application/json");
            var result = await client.PostAsync(_submissionUrl, content);

            return result.ToString();
        }

        private void SaveSubmissionUrl(string result)
        {
            var parsedResult = JObject.Parse(result);

            _submissionUrl = parsedResult["submissionUrl"].ToString();
        }
    }
}
