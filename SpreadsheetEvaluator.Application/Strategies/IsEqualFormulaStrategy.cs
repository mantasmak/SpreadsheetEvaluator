using SpreadsheetEvaluator.Application.Models;
using System.Collections.Generic;
using System.Linq;

namespace SpreadsheetEvaluator.Application.Strategies
{
    class IsEqualFormulaStrategy : FormulaStrategy
    {
        public IsEqualFormulaStrategy() : base()
        {

        }

        public IsEqualFormulaStrategy(Cell cell) : base(cell) { }

        public IsEqualFormulaStrategy(ICollection<Cell> cells) : base(cells) { }

        override protected string EvaluateFormula()
        {
            var firstOperand = float.Parse(Cells.ElementAt(0).Value);
            var secondOperand = float.Parse(Cells.ElementAt(1).Value);

            return firstOperand == secondOperand ? "true" : "false";
        }
    }
}
