using SpreadsheetEvaluator.Application.Models;

namespace SpreadsheetEvaluator.Application.Strategies
{
    class DivideFormulaStrategy : FormulaStrategy
    {
        public override Cell.Type ResultValueType { get; protected set; } = Cell.Type.Number;

        override protected string EvaluateFormula()
        {
            var firstValue = float.Parse(Cells[0].Value);
            var secondValue = float.Parse(Cells[1].Value);

            var quotient = firstValue / secondValue;

            return quotient.ToString();
        }
    }
}
