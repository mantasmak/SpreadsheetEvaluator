using SpreadsheetEvaluator.Application.Interfaces;
using SpreadsheetEvaluator.Application.Models;
using System.Collections.Generic;
using System.Linq;

namespace SpreadsheetEvaluator.Application.Strategies
{
    class IfFormulaStrategy : FormulaStrategy
    {
        public IfFormulaStrategy() : base()
        {

        }

        public IfFormulaStrategy(Cell cell) : base(cell) { }

        public IfFormulaStrategy(ICollection<Cell> cells) : base(cells) { }

        internal IFormulaStrategy ConditionStrategy { get; set; }

        override protected string EvaluateFormula()
        {
            var valueIfTrue = Cells.ElementAt(0).Value;
            var valueIfFalse = Cells.ElementAt(1).Value;

            var conditionResult = ConditionStrategy.Evaluate();

            if (bool.Parse(conditionResult))
                return valueIfTrue;
            else
                return valueIfFalse;
        }
    }
}
