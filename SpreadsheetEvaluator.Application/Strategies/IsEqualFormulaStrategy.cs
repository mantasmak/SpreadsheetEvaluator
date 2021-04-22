using SpreadsheetEvaluator.Application.Models;

namespace SpreadsheetEvaluator.Application.Strategies
{
    class IsEqualFormulaStrategy : FormulaStrategy
    {
        public override Cell.Type ResultValueType { get; protected set; } = Cell.Type.Boolean;

        override protected string EvaluateFormula()
        {
            var firstOperand = float.Parse(Cells[0].Value);
            var secondOperand = float.Parse(Cells[1].Value);

            return firstOperand == secondOperand ? "true" : "false";
        }
    }
}
