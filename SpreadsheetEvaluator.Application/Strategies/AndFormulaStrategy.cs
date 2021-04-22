using SpreadsheetEvaluator.Application.Models;

namespace SpreadsheetEvaluator.Application.Strategies
{
    class AndFormulaStrategy : FormulaStrategy
    {
        public override Cell.Type ResultValueType { get; protected set; } = Cell.Type.Boolean;

        override protected string EvaluateFormula()
        {
            var result = true;

            foreach (var cell in Cells)
            { 
                result &= bool.Parse(cell.Value);
            }

            return result ? "true" : "false";
        }
    }
}
