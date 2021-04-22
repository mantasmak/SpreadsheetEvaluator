using SpreadsheetEvaluator.Application.Interfaces;
using SpreadsheetEvaluator.Application.Models;
using System.Linq;

namespace SpreadsheetEvaluator.Application.Strategies
{
    class IfFormulaStrategy : FormulaStrategy
    {
        public override Cell.Type ResultValueType { get; protected set; }

        internal IFormulaStrategy ConditionStrategy { get; set; }

        override protected string EvaluateFormula()
        {
            for (int i = 0; i < Cells.Count() - 2; i++)
                ConditionStrategy.Cells[i] = Cells[i];

            var cellIfTrue = Cells[Cells.Count() - 2];
            var cellIfFalse = Cells.Last();

            var conditionResult = ConditionStrategy.Evaluate();

            if (bool.Parse(conditionResult))
            {
                ResultValueType = cellIfTrue.ValueType;

                return cellIfTrue.Value;
            }
            else
            {
                ResultValueType = cellIfFalse.ValueType;

                return cellIfFalse.Value;
            }
        }
    }
}
