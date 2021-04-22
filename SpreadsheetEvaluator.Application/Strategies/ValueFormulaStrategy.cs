using SpreadsheetEvaluator.Application.Models;

namespace SpreadsheetEvaluator.Application.Strategies
{
    class ValueFormulaStrategy : FormulaStrategy
    {
        public override Cell.Type ResultValueType { get; protected set; }

        protected override string EvaluateFormula()
        {
            ResultValueType = Cells[0].ValueType;

            return Cells[0].Value;
        }
    }
}
