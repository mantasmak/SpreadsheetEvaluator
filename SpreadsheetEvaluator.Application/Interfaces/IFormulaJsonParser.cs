using SpreadsheetEvaluator.Application.Models;
using System.Collections.Generic;

namespace SpreadsheetEvaluator.Application.Interfaces
{
    public interface IFormulaJsonParser
    {
        public IFormulaStrategy Parse(string jsonFormula, Job job);
    }
}
