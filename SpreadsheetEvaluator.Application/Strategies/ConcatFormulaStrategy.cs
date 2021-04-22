using SpreadsheetEvaluator.Application.Models;
using System.Text;

namespace SpreadsheetEvaluator.Application.Strategies
{
    class ConcatFormulaStrategy : FormulaStrategy
    {
        public override Cell.Type ResultValueType { get; protected set; } = Cell.Type.Text;

        override protected string EvaluateFormula()
        {
            var stringBuilder = new StringBuilder();

            foreach (var value in Cells)
                stringBuilder.Append(value.Value);

            return stringBuilder.ToString();
        }
    }
}
