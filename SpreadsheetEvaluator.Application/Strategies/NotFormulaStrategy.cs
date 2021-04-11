using SpreadsheetEvaluator.Application.Models;
using System.Collections.Generic;
using System.Linq;

namespace SpreadsheetEvaluator.Application.Strategies
{
    class NotFormulaStrategy : FormulaStrategy
    {
        public NotFormulaStrategy() : base()
        {

        }

        public NotFormulaStrategy(Cell cell) : base(cell) { }

        public NotFormulaStrategy(ICollection<Cell> cells) : base(cells) { }

        override protected string EvaluateFormula ()
        {
            var firstOperand = bool.Parse(Cells.ElementAt(0).Value);

            return firstOperand ? "false" : "true";
        }
    }
}
