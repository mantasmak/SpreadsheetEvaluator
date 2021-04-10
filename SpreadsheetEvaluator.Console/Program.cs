using System;
using System.Net.Http;
using System.Threading.Tasks;
using SpreadsheetEvaluator.Application.Parsers;

namespace SpreadsheetEvaluator.Ui
{
    class Program
    {
        static async Task Main()
        {
            using var client = new HttpClient();

            var content = await client.GetStringAsync("https://www.wix.com/_serverless/hiring-task-spreadsheet-evaluator/jobs");

            var parser = new JobJsonParser(new FormulaJsonParser());

            var jobs = parser.Parse(content);

            foreach (var job in jobs)
                foreach (var cell in job.Cells)
                    Console.WriteLine(cell.Coordinates);
        }
    }
}
