using SpreadsheetEvaluator.Application.Models;
using System.Collections.Generic;
using System.Linq;

namespace SpreadsheetEvaluator.Application.Strategies
{
    class IsGreaterFormulaStrategy : FormulaStrategy
    {
        public IsGreaterFormulaStrategy() : base()
        {

        }

        public IsGreaterFormulaStrategy(Cell cell) : base(cell) { }

        public IsGreaterFormulaStrategy(ICollection<Cell> cells) : base(cells) { }

        override protected string EvaluateFormula()
        {
            var firstOperand = int.Parse(Cells.ElementAt(0).Value);
            var secondOperand = int.Parse(Cells.ElementAt(1).Value);

            return firstOperand > secondOperand ? "true" : "false";
        }
    }
}
