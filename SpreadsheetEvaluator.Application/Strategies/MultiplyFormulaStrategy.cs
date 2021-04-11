using SpreadsheetEvaluator.Application.Models;
using System.Collections.Generic;

namespace SpreadsheetEvaluator.Application.Strategies
{
    class MultiplyFormulaStrategy : FormulaStrategy
    {
        public MultiplyFormulaStrategy() : base()
        {

        }

        public MultiplyFormulaStrategy(Cell cell) : base(cell) { }

        public MultiplyFormulaStrategy(ICollection<Cell> cells) : base(cells) { }

        override protected string EvaluateFormula()
        {
            var accumulatedProduct = 1f;

            foreach (var cell in Cells)
            {
                accumulatedProduct *= float.Parse(cell.Value);
            }

            return accumulatedProduct.ToString();
        }
    }
}
