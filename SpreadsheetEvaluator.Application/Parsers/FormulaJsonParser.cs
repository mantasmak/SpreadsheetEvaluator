using Newtonsoft.Json.Linq;
using SpreadsheetEvaluator.Application.Interfaces;
using SpreadsheetEvaluator.Application.Models;
using SpreadsheetEvaluator.Application.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace SpreadsheetEvaluator.Application.Parsers
{
    public class FormulaJsonParser : IFormulaJsonParser
    {
        public Formula Parse(string jsonFormula, IEnumerable<Job> jobs)
        {
            JObject json = JObject.Parse(jsonFormula);
            var formula = new Formula() { CellReferences = new List<Cell>() };

            var properties = JsonParserUtilities.GetCellProperty(json);

            switch(properties.Name)
            {
                case "value":
                    SetValue(JsonParserUtilities.GetCellProperty(properties.Value), formula);
                    break;
                case "reference":
                    SetFormulaCellByReference(properties.Value.ToString(), jobs, formula);
                    break;
                default:
                    SetFunction(properties, jobs, formula);
                    break;
            };

            return formula;
        }

        private void SetValue(JProperty property, Formula formula)
        {
            var valueType = JsonParserUtilities.StringToCellType(property.Name);
            var value = property.Value.ToString();
            formula.CellReferences.Add(new Cell(string.Empty, value, valueType));
        }

        private void SetFormulaCellByReference(string coordinates, IEnumerable<Job> jobs, Formula formula)
        {
            var cell = GetCellByCoordinates(coordinates, jobs);

            //Error
            if (cell == null)
                return;

            formula.CellReferences.Add(cell);
        }

        private Cell GetCellByCoordinates(string coordinates, IEnumerable<Job> jobs)
        {
            foreach (var job in jobs)
            {
                var cell = job.Cells.Where(c => c.Coordinates == coordinates)
                                    .Select(c => c)
                                    .FirstOrDefault();

                if (cell != null)
                    return cell;
            }

            return null;
        }

        private void SetFunction(JProperty property, IEnumerable<Job> jobs, Formula formula)
        {
            foreach (var jsonCoordinate in property.Value)
            {
                var coordinates = string.Empty;
                if (jsonCoordinate.Type != JTokenType.Property)
                {
                    coordinates = jsonCoordinate.First.ToObject<JProperty>().Value.ToString();
                }
                else
                {
                    coordinates = jsonCoordinate.ToObject<JProperty>().Value.ToString();
                }

                SetFormulaCellByReference(coordinates, jobs, formula);
            }
            

            switch (property.Name)
            {
                case "sum":
                    formula.Function = formula.Sum;
                    break;
                case "multiply":
                    formula.Function = formula.Multiply;
                    break;
                case "divide":
                    formula.Function = formula.Divide;
                    break;
                case "is_greater":
                    formula.Function = formula.IsGreater;
                    break;
                case "is_equal":
                    formula.Function = formula.IsEqual;
                    break;
                case "not":
                    formula.Function = formula.Not;
                    break;
                case "and":
                    formula.Function = formula.And;
                    break;
                case "or":
                    formula.Function = formula.Or;
                    break;
                case "if":
                    formula.Function = formula.IfFunction;
                    break;
                case "concat":
                    formula.Function = formula.Concat;
                    break;
            }
        }
    }
}
