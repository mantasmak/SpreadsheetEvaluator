using SpreadsheetEvaluator.Application.Models;
using System.Collections.Generic;
using System.Text;

namespace SpreadsheetEvaluator.Application.Strategies
{
    class ConcatFormulaStrategy : FormulaStrategy
    {
        public ConcatFormulaStrategy() : base()
        {

        }

        public ConcatFormulaStrategy(Cell cell) : base(cell) { }

        public ConcatFormulaStrategy(ICollection<Cell> cells) : base(cells) { }

        override protected string EvaluateFormula()
        {
            var stringBuilder = new StringBuilder();

            foreach (var value in Cells)
                stringBuilder.Append(value.Value);

            return stringBuilder.ToString();
        }
    }
}
