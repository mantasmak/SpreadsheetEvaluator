using SpreadsheetEvaluator.Application.Models;
using System.Collections.Generic;

namespace SpreadsheetEvaluator.Application.Interfaces
{
    public interface IFormulaJsonParser
    {
        public Formula Parse(string jsonFormula, IEnumerable<Job> jobs);
    }
}
