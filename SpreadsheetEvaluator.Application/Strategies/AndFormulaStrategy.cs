using SpreadsheetEvaluator.Application.Models;
using System.Collections.Generic;
using System.Linq;

namespace SpreadsheetEvaluator.Application.Strategies
{
    class AndFormulaStrategy : FormulaStrategy
    {
        public AndFormulaStrategy() : base()
        {

        }

        public AndFormulaStrategy(Cell cell) : base(cell) { }

        public AndFormulaStrategy(ICollection<Cell> cells) : base(cells) { }

        override protected string EvaluateFormula()
        {
            var result = true;

            foreach (var cell in Cells)
            { 
                result &= bool.Parse(cell.Value);
            }

            return result ? "true" : "false";
        }
    }
}
