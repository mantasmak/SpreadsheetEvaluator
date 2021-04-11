using SpreadsheetEvaluator.Application.Interfaces;
using SpreadsheetEvaluator.Application.Models;
using System.Collections.Generic;

namespace SpreadsheetEvaluator.Application.Strategies
{
    class SumFormulaStrategy : FormulaStrategy
    {
        public SumFormulaStrategy() : base()
        {

        }

        public SumFormulaStrategy(Cell cell) : base(cell) { }

        public SumFormulaStrategy(ICollection<Cell> cells) : base(cells) { }

        override protected string EvaluateFormula()
        {
            var accumulatedSum = 0f;

            foreach (var cell in Cells)
            {
                accumulatedSum += float.Parse(cell.Value);
            }

            return accumulatedSum.ToString();
        }
    }
}
