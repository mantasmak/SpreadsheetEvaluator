using SpreadsheetEvaluator.Application.Models;

namespace SpreadsheetEvaluator.Application.Strategies
{
    class SumFormulaStrategy : FormulaStrategy
    {
        public override Cell.Type ResultValueType { get; protected set; } = Cell.Type.Number;

        override protected string EvaluateFormula()
        {
            var accumulatedSum = 0f;

            foreach (var cell in Cells)
            {
                accumulatedSum += float.Parse(cell.Value);
            }

            return accumulatedSum.ToString();
        }
    }
}
