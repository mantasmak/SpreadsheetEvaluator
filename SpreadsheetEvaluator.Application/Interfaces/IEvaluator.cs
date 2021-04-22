using SpreadsheetEvaluator.Application.Models;
using System.Collections.Generic;

namespace SpreadsheetEvaluator.Application.Interfaces
{
    public interface IEvaluator
    {
        void Evaluate(List<List<Cell>> spreadsheet);
    }
}
