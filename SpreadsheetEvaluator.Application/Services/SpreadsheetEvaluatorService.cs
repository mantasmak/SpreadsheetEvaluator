using SpreadsheetEvaluator.Application.Interfaces;
using SpreadsheetEvaluator.Application.Models;
using SpreadsheetEvaluator.Application.Parsers;
using SpreadsheetEvaluator.Infrastructure.Interfaces;
using System.Threading.Tasks;

namespace SpreadsheetEvaluator.Application
{
    public class SpreadsheetEvaluatorService : ISpreadsheetEvaluatorService
    {
        IJobWebService _jobWebService;
        IJobJsonParser _jobJsonParser;
        ISubmissionSerializer _submissionSerializer;

        public SpreadsheetEvaluatorService(IJobWebService jobWebService, IJobJsonParser jobJsonParser, ISubmissionSerializer submissionSerializer)
        {
            _jobWebService = jobWebService;
            _jobJsonParser = jobJsonParser;
            _submissionSerializer = submissionSerializer;
        }

        public async Task<string> EvaluateSpreadsheet()
        {
            var jobString = await _jobWebService.GetJobsAsync();
            var parsedJobs = _jobJsonParser.Parse(jobString);

            var submission = new Submission()
            {
                Email = "htrs@gmail.com",
                Results = parsedJobs
            };

            var serializedSubmission = _submissionSerializer.Serialize(submission);

            //var response = await _jobWebService.PostResultsAsync(serializedSubmission);

            return serializedSubmission;
        }
        
    }
}
