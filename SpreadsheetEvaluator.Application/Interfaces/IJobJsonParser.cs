using SpreadsheetEvaluator.Application.Models;
using System.Collections.Generic;

namespace SpreadsheetEvaluator.Application.Parsers
{
    internal interface IJobJsonParser
    {
        public IEnumerable<Job> Parse(string jsonString);
    }
}