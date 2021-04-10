using SpreadsheetEvaluator.Application.Interfaces;
using SpreadsheetEvaluator.Application.Parsers;
using SpreadsheetEvaluator.Infrastructure.Interfaces;
using System;
using System.Threading.Tasks;

namespace SpreadsheetEvaluator.Application
{
    class SpreadsheetEvaluatorService : ISpreadsheetEvaluatorService
    {
        IJobWebService _jobWebService;
        IJobJsonParser _jobJsonParser;

        public SpreadsheetEvaluatorService(IJobWebService jobWebService, IJobJsonParser jobJsonParser)
        {
            _jobWebService = jobWebService;
            _jobJsonParser = jobJsonParser;
        }

        public async Task<string> EvaluateSpreadsheet()
        {
            var jobString = await _jobWebService.GetJobsAsync();
            var parsedJson = _jobJsonParser.Parse(jobString);

            throw new NotImplementedException();
        }
        //get json

        //parse json
        //add cells to spreadsheet
        
    }
}
