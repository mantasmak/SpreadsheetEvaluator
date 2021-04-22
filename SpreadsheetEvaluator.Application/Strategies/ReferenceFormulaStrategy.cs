using SpreadsheetEvaluator.Application.Models;
using System.Linq;

namespace SpreadsheetEvaluator.Application.Strategies
{
    class ReferenceFormulaStrategy : FormulaStrategy
    {
        public override Cell.Type ResultValueType { get; protected set; }

        override protected string EvaluateFormula()
        {
            var cell = Cells.First();

            while (cell != null)
            {
                if (cell.Value != null)
                {
                    ResultValueType = cell.ValueType;

                    return cell.Value;
                }

                if (cell.Formula != null && cell.Formula.Cells.Count() != 0)
                    cell = cell.Formula.Cells.First();
                else
                    return null;
            }

            return null;
        }
    }
}
