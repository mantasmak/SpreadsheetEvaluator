using SpreadsheetEvaluator.Application.Interfaces;
using SpreadsheetEvaluator.Application.Models;
using System;
using System.Collections.Generic;

namespace SpreadsheetEvaluator.Application.Strategies
{
    abstract class FormulaStrategy : IFormulaStrategy
    {
        public List<Cell> Cells { get; set; } = new List<Cell>();
        virtual public Cell.Type ResultValueType { get; protected set; }

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
