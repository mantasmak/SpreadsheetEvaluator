using SpreadsheetEvaluator.Application.Interfaces;
using System;

namespace SpreadsheetEvaluator.Application.Models
{
    public class Cell
    {
        public string Coordinates { get; set; }

        public string Value { get; set; }

        public IFormulaStrategy Formula { get; set; }

        public Type ValueType { get; set; } = Type.Error;
        public enum Type
        {
            Number,
            Text,
            Boolean,
            Error
        }
    }
}
