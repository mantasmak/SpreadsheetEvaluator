using SpreadsheetEvaluator.Application.Models;
using System.Collections.Generic;

namespace SpreadsheetEvaluator.Application.Interfaces
{
    public interface IFormulaStrategy
    {
        public List<Cell> Cells { get; set; }

        string Evaluate();
    }
}
