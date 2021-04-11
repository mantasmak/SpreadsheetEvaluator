using SpreadsheetEvaluator.Application.Interfaces;
using System;

namespace SpreadsheetEvaluator.Application.Models
{
    public class Cell
    {
        public string Coordinates { get; set; }

        public string Value { get; set; }

        public IFormulaStrategy Formula { get; set; }

        public Type ValueType { get;}
        public enum Type
        {
            Number,
            Text,
            Boolean,
            Error
        }

        public Cell(string coordinates, string value, Type type)
        {
            Coordinates = coordinates;
            Value = value;
            ValueType = type;
        }

        public Cell(string coordinates, Type type, IFormulaStrategy formula)
        {
            try
            {
                Formula = formula;
                Coordinates = coordinates;
                ValueType = type;
                Value = formula.Evaluate();
            }
            catch (Exception e)
            {
                ValueType = Type.Error;
                Value = "Could not evaluate formula!";
            }
        }
    }
}
