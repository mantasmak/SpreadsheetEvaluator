using SpreadsheetEvaluator.Application.Models;
using System.Collections.Generic;
using System.Linq;

namespace SpreadsheetEvaluator.Application.Strategies
{
    class OrFormulaStrategy : FormulaStrategy
    {
        public OrFormulaStrategy() : base()
        {

        }

        public OrFormulaStrategy(Cell cell) : base(cell) { }

        public OrFormulaStrategy(ICollection<Cell> cells) : base(cells) { }

        override protected string EvaluateFormula()
        {
            var firstOperand = bool.Parse(Cells.ElementAt(0).Value);
            var secondOperand = bool.Parse(Cells.ElementAt(1).Value);

            return firstOperand || secondOperand ? "true" : "false";
        }
    }
}
