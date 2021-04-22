using SpreadsheetEvaluator.Application.Models;

namespace SpreadsheetEvaluator.Application.Strategies
{
    class MultiplyFormulaStrategy : FormulaStrategy
    {
        public override Cell.Type ResultValueType { get; protected set; } = Cell.Type.Number;

        override protected string EvaluateFormula()
        {
            var accumulatedProduct = 1f;

            foreach (var cell in Cells)
            {
                accumulatedProduct *= float.Parse(cell.Value);
            }

            return accumulatedProduct.ToString();
        }
    }
}
