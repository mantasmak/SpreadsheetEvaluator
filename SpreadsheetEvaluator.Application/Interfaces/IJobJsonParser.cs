using SpreadsheetEvaluator.Application.Models;
using System.Collections.Generic;

namespace SpreadsheetEvaluator.Application.Parsers
{
    public interface IJobJsonParser
    {
        public IEnumerable<Job> Parse(string jsonString);
    }
}