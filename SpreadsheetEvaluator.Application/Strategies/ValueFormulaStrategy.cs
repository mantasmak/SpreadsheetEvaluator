using SpreadsheetEvaluator.Application.Models;
using System.Collections.Generic;
using System.Linq;

namespace SpreadsheetEvaluator.Application.Strategies
{
    class ValueFormulaStrategy : FormulaStrategy
    {
        public ValueFormulaStrategy() : base()
        {

        }

        public ValueFormulaStrategy(Cell cell) : base(cell) { }

        public ValueFormulaStrategy(ICollection<Cell> cells) : base(cells) { }

        protected override string EvaluateFormula()
        {
            return Cells.ElementAt(0).Value;
        }
    }
}
