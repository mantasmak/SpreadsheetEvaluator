using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadsheetEvaluator.Application.Models
{
    public class Formula
    {
        public ICollection<Cell> CellReferences { get; set; }
        public Func<IEnumerable<string>, string> Function { get; set; }

        internal string Sum(IEnumerable<string> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var accumulatedSum = 0f;

            foreach(var value in values)
            {
                accumulatedSum += float.Parse(value);
            }

            return accumulatedSum.ToString();
        }

        internal string Multiply(IEnumerable<string> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var accumulatedProduct = 0f;

            foreach (var value in values)
            {
                accumulatedProduct *= float.Parse(value);
            }

            return accumulatedProduct.ToString();
        }

        internal string Divide(IEnumerable<string> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var firstValue = float.Parse(values.ElementAt(0));
            var secondValue = float.Parse(values.ElementAt(1));

            var quotient = firstValue / secondValue;

            return quotient.ToString();
        }

        internal string IsGreater(IEnumerable<string> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var firstOperand = int.Parse(values.ElementAt(0));
            var secondOperand = int.Parse(values.ElementAt(1));

            return firstOperand > secondOperand ? "true" : "false";
        }

        internal string IsEqual(IEnumerable<string> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var firstOperand = int.Parse(values.ElementAt(0));
            var secondOperand = int.Parse(values.ElementAt(1));

            return firstOperand == secondOperand ? "true" : "false";
        }

        internal string Not(IEnumerable<string> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var firstOperand = bool.Parse(values.ElementAt(0));

            return firstOperand ? "false" : "true";
        }

        internal string And(IEnumerable<string> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var firstOperand = bool.Parse(values.ElementAt(0));
            var secondOperand = bool.Parse(values.ElementAt(1));

            return firstOperand && secondOperand ? "true" : "false";
        }

        internal string Or(IEnumerable<string> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var firstOperand = bool.Parse(values.ElementAt(0));
            var secondOperand = bool.Parse(values.ElementAt(1));

            return firstOperand || secondOperand ? "true" : "false";
        }

        internal string IfFunction(IEnumerable<string> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var conditionName = values.ElementAt(0);
            var operands = values.Skip(1).SkipLast(2).ToList();
            var valueIfTrue = values.ElementAt(values.Count() - 2);
            var valueIfFalse = values.Last();
            var conditionResult = String.Empty;

            switch (conditionName)
            {
                case "is_greater":
                    conditionResult = IsGreater(operands);
                    break;

                case "is_equal":
                    conditionResult = IsEqual(operands);
                    break;

                case "not":
                    conditionResult = Not(operands);
                    break;

                case "and":
                    conditionResult = And(operands);
                    break;

                case "or":
                    conditionResult = Or(operands);
                    break;
            }

            if (bool.Parse(conditionResult))
                return valueIfTrue;
            else
                return valueIfFalse;
        }

        internal string Concat(IEnumerable<string> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var stringBuilder = new StringBuilder();

            foreach(var value in values)
                stringBuilder.Append(value);

            return stringBuilder.ToString();
        }
    }
}
