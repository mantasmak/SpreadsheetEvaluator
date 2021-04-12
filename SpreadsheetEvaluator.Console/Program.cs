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
            var service = new SpreadsheetEvaluatorService(new JobWebService(), new JobJsonParser(new FormulaJsonParser()), new SubmissionSerializer());

            Console.WriteLine(await service.EvaluateSpreadsheet());
        }
    }
}
