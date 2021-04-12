using SpreadsheetEvaluator.Application.Models;
using System.Collections.Generic;
using System.Linq;

namespace SpreadsheetEvaluator.Application.Strategies
{
    class ReferenceFormulaStrategy : FormulaStrategy
    {
        public ReferenceFormulaStrategy() : base()
        {

        }

        public ReferenceFormulaStrategy(Cell cell) : base(cell) { }

        public ReferenceFormulaStrategy(ICollection<Cell> cells) : base(cells) { }

        override protected string EvaluateFormula()
        {
            var cell = Cells.First();

            while (cell != null)
            {
                if (cell.Value != string.Empty)
                {
                    return cell.Value;
                }

                if (cell.Formula.Cells.Count() != 0)
                    cell = cell.Formula.Cells.First();
                else
                    return string.Empty;
            }

            return string.Empty;
        }
    }
}
