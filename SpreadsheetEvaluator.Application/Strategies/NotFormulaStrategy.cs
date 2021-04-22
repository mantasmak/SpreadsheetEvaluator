using SpreadsheetEvaluator.Application.Models;

namespace SpreadsheetEvaluator.Application.Strategies
{
    class NotFormulaStrategy : FormulaStrategy
    {
        public override Cell.Type ResultValueType { get; protected set; } = Cell.Type.Boolean;

        override protected string EvaluateFormula ()
        {
            var firstOperand = bool.Parse(Cells[0].Value);

            return firstOperand ? "false" : "true";
        }
    }
}
