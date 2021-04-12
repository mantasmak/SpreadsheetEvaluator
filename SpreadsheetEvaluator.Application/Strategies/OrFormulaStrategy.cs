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
            var result = false;

            foreach (var cell in Cells)
                result |= bool.Parse(cell.Value);

            return result ? "true" : "false";
        }
    }
}
