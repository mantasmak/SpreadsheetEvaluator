using SpreadsheetEvaluator.Application.Models;
using System.Collections.Generic;

namespace SpreadsheetEvaluator.Application.Parsers
{
    public interface IJobJsonParser
    {
        public Submission Parse(string jsonString);
    }
}