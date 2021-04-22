using System;
using System.Threading.Tasks;
using SpreadsheetEvaluator.Application;
using SpreadsheetEvaluator.Application.Json_Serializers;
using SpreadsheetEvaluator.Application.Parsers;
using SpreadsheetEvaluator.Infrastructure.Web_Services;
using SpreadsheetEvaluator.Application.Evaluators;
using System.Linq;

namespace SpreadsheetEvaluator.Ui
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Count() != 1)
                throw new ArgumentException("Wrong number of arguments");

            var emailAddress = args[0];

            var service = new SpreadsheetEvaluatorService(new JobWebService(), new JobJsonParser(), new FormulaEvaluator(), new SubmissionSerializer());
            var response = await service.EvaluateSpreadsheet(emailAddress);

            Console.WriteLine(response);
        }
    }
}
