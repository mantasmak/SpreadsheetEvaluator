using System.Threading.Tasks;

namespace SpreadsheetEvaluator.Infrastructure.Interfaces
{
    public interface IJobWebService
    {
        public Task<string> GetJobsAsync();

        public Task<string> PostResultsAsync(string results);
    }
}
