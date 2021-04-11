using SpreadsheetEvaluator.Application.Interfaces;
using SpreadsheetEvaluator.Application.Models;
using System;
using System.Collections.Generic;

namespace SpreadsheetEvaluator.Application.Strategies
{
    abstract class FormulaStrategy : IFormulaStrategy
    {
        public IEnumerable<Cell> Cells { get; set; }

        public FormulaStrategy()
        {
            Cells = new List<Cell>();
        }

        public FormulaStrategy(Cell cell)
        {
            Cells = new List<Cell>() { cell };
        }

        public FormulaStrategy(ICollection<Cell> cells)
        {
            Cells = cells;
        }

        public string Evaluate()
        {
            if (Cells is null)
            {
                throw new NullReferenceException(nameof(Cells));
            }

            return EvaluateFormula();
        }

        abstract protected string EvaluateFormula();
    }
}
