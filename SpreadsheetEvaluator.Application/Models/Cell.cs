using System.Linq;

namespace SpreadsheetEvaluator.Application.Models
{
    public class Cell
    {
        public string Coordinates { get; set; }
        public string Value { get; set; }
        public Formula Formula { get; set; }
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

        public Cell(string coordinates, Type type, Formula formula)
        {
            Coordinates = coordinates;
            ValueType = type;
            Formula = formula;
            var formulaValues = formula.CellReferences.Select(c => c.Value);
            Value = formula.Function(formulaValues);
        }

        public Cell(string coordinates, string value, Type type, Formula formula)
        {
            Coordinates = coordinates;
            Value = value;
            ValueType = type;
            Formula = formula;
        }
    }
}
