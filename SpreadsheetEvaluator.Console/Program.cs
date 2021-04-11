using System;
using System.Threading.Tasks;
using SpreadsheetEvaluator.Application;
using SpreadsheetEvaluator.Application.Json_Serializers;
using SpreadsheetEvaluator.Application.Parsers;
using SpreadsheetEvaluator.Infrastructure.Web_Services;

namespace SpreadsheetEvaluator.Ui
{
    class Program
    {
        static async Task Main()
        {
            /*using var client = new HttpClient();

            var content = await client.GetStringAsync("https://www.wix.com/_serverless/hiring-task-spreadsheet-evaluator/jobs");

            var parser = new JobJsonParser(new FormulaJsonParser());

            var jobs = parser.Parse(content);

            foreach (var job in jobs)
                foreach (var cell in job.Data)
                    Console.WriteLine(cell.Value);*/

            var service = new SpreadsheetEvaluatorService(new JobWebService(), new JobJsonParser(new FormulaJsonParser()), new SubmissionSerializer());

            Console.WriteLine(await service.EvaluateSpreadsheet());
        }
    }
}
