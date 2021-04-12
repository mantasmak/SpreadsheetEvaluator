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
        public IFormulaStrategy Parse(string jsonFormula, Job job)
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
                    formulaStrategy = CreateValueFormulaStrategyByReference(properties.Value.ToString(), job);
                    break;
                case "if":
                    formulaStrategy = CreateIfFormulaStrategy(properties, job);
                    break;
                default:
                    formulaStrategy = CreateFormulaStrategy(properties, job);
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

        private ReferenceFormulaStrategy CreateValueFormulaStrategyByReference(string coordinates, Job job)
        {
            var cell = GetCellByCoordinates(coordinates, job);

            if(cell != null)
                return new ReferenceFormulaStrategy(cell);

            return new ReferenceFormulaStrategy();
        }

        private Cell GetCellByCoordinates(string coordinates, Job job)
        {
            var allCells = job.Data.SelectMany(d => d);

            var cell = allCells.Where(c => c.Coordinates == coordinates)
                                .Select(c => c)
                                .FirstOrDefault();

            if (cell != null)
                return cell;

            return null;
        }

        private IFormulaStrategy CreateIfFormulaStrategy(JProperty properties, Job job)
        {
            var conditionProperty = properties.Value.First().First.ToObject<JProperty>();

            var conditionFormulaStrategy = CreateFormulaStrategy(conditionProperty, job);

            RemoveConditionFromIfStatement(properties);

            var formulaStrategy = (IfFormulaStrategy) CreateFormulaStrategy(properties, job);
            formulaStrategy.ConditionStrategy = conditionFormulaStrategy;

            return formulaStrategy;
        }

        private void RemoveConditionFromIfStatement(JProperty ifStatementContent)
        {
            var jArray = ifStatementContent.Value.ToObject<JArray>();
            jArray.RemoveAt(0);
            ifStatementContent.Value = jArray;
        }

        private IFormulaStrategy CreateFormulaStrategy(JProperty property, Job job)
        {
            var formulaStrategy = ChooseFormulaStrategy(property.Name);

            formulaStrategy.Cells = ParseStrategyCells(property.Value, job);

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

        private IEnumerable<Cell> ParseStrategyCells(JToken jsonOperands, Job job)
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
                        var cell = GetCellByCoordinates(coordinates, job);

                        if (cell != null)
                            cells.Add(cell);
                    }
                }

                return cells;
            }
            else
            {
                var coordinates = jsonOperands["reference"].ToString();
                var cell = GetCellByCoordinates(coordinates, job);

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
