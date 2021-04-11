using Newtonsoft.Json.Linq;
using SpreadsheetEvaluator.Application.Interfaces;
using SpreadsheetEvaluator.Application.Models;
using SpreadsheetEvaluator.Application.Strategies;
using SpreadsheetEvaluator.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpreadsheetEvaluator.Application.Parsers
{
    public class FormulaJsonParser : IFormulaJsonParser
    {
        public IFormulaStrategy Parse(string jsonFormula, IEnumerable<Job> jobs)
        {
            JObject json = JObject.Parse(jsonFormula);
            IFormulaStrategy formulaStrategy;

            var properties = JsonParserUtilities.GetCellProperty(json);

            switch(properties.Name)
            {
                case "value":
                    formulaStrategy = CreateValueFormulaStrategyByValue(JsonParserUtilities.GetCellProperty(properties.Value));
                    break;
                case "reference":
                    formulaStrategy = CreateValueFormulaStrategyByReference(properties.Value.ToString(), jobs);
                    break;
                case "if":
                    formulaStrategy = CreateIfFormulaStrategy(properties, jobs);
                    break;
                default:
                    formulaStrategy = CreateFormulaStrategy(properties, jobs);
                    break;
            };

            return formulaStrategy;
        }

        private ValueFormulaStrategy CreateValueFormulaStrategyByValue(JProperty property)
        {
            var valueType = JsonParserUtilities.StringToCellType(property.Name);
            var value = property.Value.ToString();

            var cell = new Cell("", value, valueType);

            return new ValueFormulaStrategy(cell);
        }

        private ValueFormulaStrategy CreateValueFormulaStrategyByReference(string coordinates, IEnumerable<Job> jobs)
        {
            var cell = GetCellByCoordinates(coordinates, jobs);

            if(cell != null)
                return new ValueFormulaStrategy(cell);

            return new ValueFormulaStrategy();
        }

        private Cell GetCellByCoordinates(string coordinates, IEnumerable<Job> jobs)
        {
            foreach (var job in jobs)
            {
                var cell = job.Data.Where(c => c.Coordinates == coordinates)
                                    .Select(c => c)
                                    .FirstOrDefault();

                if (cell != null)
                    return cell;
            }

            return null;
        }

        private IFormulaStrategy CreateIfFormulaStrategy(JProperty properties, IEnumerable<Job> jobs)
        {
            var conditionProperty = properties.Value.First().First.ToObject<JProperty>();

            var conditionFormulaStrategy = CreateFormulaStrategy(conditionProperty, jobs);

            RemoveConditionFromIfStatement(properties);

            var formulaStrategy = (IfFormulaStrategy) CreateFormulaStrategy(properties, jobs);
            formulaStrategy.ConditionStrategy = conditionFormulaStrategy;

            return formulaStrategy;
        }

        private void RemoveConditionFromIfStatement(JProperty ifStatementContent)
        {
            var jArray = ifStatementContent.Value.ToObject<JArray>();
            jArray.RemoveAt(0);
            ifStatementContent.Value = jArray;
        }

        private IFormulaStrategy CreateFormulaStrategy(JProperty property, IEnumerable<Job> jobs)
        {
            var formulaStrategy = ChooseFormulaStrategy(property.Name);

            formulaStrategy.Cells = ParseStrategyCells(property.Value, jobs);

            return formulaStrategy;
        }

        private IFormulaStrategy ChooseFormulaStrategy(string formulaName)
        {
            switch (formulaName)
            {
                case "sum":
                    return new SumFormulaStrategy();

                case "multiply":
                    return new MultiplyFormulaStrategy();

                case "divide":
                    return new DivideFormulaStrategy();

                case "is_greater":
                    return new IsGreaterFormulaStrategy();

                case "is_equal":
                    return new IsEqualFormulaStrategy();

                case "not":
                    return new NotFormulaStrategy();

                case "and":
                    return new AndFormulaStrategy();

                case "or":
                    return new OrFormulaStrategy();

                case "if":
                    return new IfFormulaStrategy();

                case "concat":
                    return new ConcatFormulaStrategy();

                default:
                    throw new ArgumentException();
            }
        }

        private IEnumerable<Cell> ParseStrategyCells(JToken jsonOperands, IEnumerable<Job> jobs)
        {
            var cells = new List<Cell>();

            if (jsonOperands.Type == JTokenType.Array)
            {
                foreach (var operand in jsonOperands)
                {
                    if (operand.First.ToObject<JProperty>().Name == "value")
                    {
                        cells.Add(CreteValueCell(operand));
                    }
                    else
                    {
                        var coordinates = operand["reference"].ToString();
                        var cell = GetCellByCoordinates(coordinates, jobs);

                        if (cell != null)
                            cells.Add(cell);
                    }
                }

                return cells;
            }
            else
            {
                var coordinates = jsonOperands["reference"].ToString();
                var cell = GetCellByCoordinates(coordinates, jobs);

                if (cell != null)
                    cells.Add(cell);

                return cells;
            }
        }

        private Cell CreteValueCell(JToken jsonValue)
        {
            var jsonCellProperty = JsonParserUtilities.GetCellProperty(jsonValue);
            var cellValue = jsonCellProperty.Value.ToString();
            var cellTypeString = jsonCellProperty.Name;
            var cellType = JsonParserUtilities.StringToCellType(cellTypeString);

            return new Cell("", cellValue, cellType);
        }
    }
}
