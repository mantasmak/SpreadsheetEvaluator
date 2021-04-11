using SpreadsheetEvaluator.Application.Models;
using System.Collections.Generic;
using System.Linq;

namespace SpreadsheetEvaluator.Application.Strategies
{
    class DivideFormulaStrategy : FormulaStrategy
    {
        public DivideFormulaStrategy() : base()
        {

        }

        public DivideFormulaStrategy(Cell cell) : base(cell) { }

        public DivideFormulaStrategy(ICollection<Cell> cells) : base(cells) { }

        override protected string EvaluateFormula()
        {
            var firstValue = float.Parse(Cells.ElementAt(0).Value);
            var secondValue = float.Parse(Cells.ElementAt(1).Value);

            var quotient = firstValue / secondValue;

            return quotient.ToString();
        }
    }
}
