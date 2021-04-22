using SpreadsheetEvaluator.Application.Interfaces;
using SpreadsheetEvaluator.Application.Parsers;
using SpreadsheetEvaluator.Infrastructure.Interfaces;
using System.Threading.Tasks;

namespace SpreadsheetEvaluator.Application
{
    public class SpreadsheetEvaluatorService : ISpreadsheetEvaluatorService
    {
        readonly IJobWebService _jobWebService;
        readonly IJobJsonParser _jobJsonParser;
        readonly IEvaluator _formulaEvaluator;
        readonly ISubmissionSerializer _submissionSerializer;

        public SpreadsheetEvaluatorService(IJobWebService jobWebService, IJobJsonParser jobJsonParser, IEvaluator formulaEvaluator, ISubmissionSerializer submissionSerializer)
        {
            _jobWebService = jobWebService;
            _jobJsonParser = jobJsonParser;
            _formulaEvaluator = formulaEvaluator;
            _submissionSerializer = submissionSerializer;
        }

        public async Task<string> EvaluateSpreadsheet(string emailAddress)
        {
            var jobString = await _jobWebService.GetJobsAsync();
            var submission = _jobJsonParser.Parse(jobString);
            submission.Email = emailAddress;

            foreach (var job in submission.Results)
                _formulaEvaluator.Evaluate(job.Data);

            var serializedSubmission = _submissionSerializer.Serialize(submission);

            var response = await _jobWebService.PostResultsAsync(serializedSubmission, submission.SubmissionUrl);

            return response;
        }
        
    }
}
