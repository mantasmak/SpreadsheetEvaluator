using Newtonsoft.Json.Linq;
using SpreadsheetEvaluator.Application.Interfaces;
using SpreadsheetEvaluator.Application.Models;
using SpreadsheetEvaluator.Application.Strategies;
using SpreadsheetEvaluator.Application.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace SpreadsheetEvaluator.Application.Parsers
{
    public class JobJsonParser : IJobJsonParser
    {
        private readonly IFormulaJsonParser _formulaJsonParser;

        public JobJsonParser(IFormulaJsonParser formulaJsonParser)
        {
            _formulaJsonParser = formulaJsonParser;
        }

        public IEnumerable<Job> Parse(string jsonString)
        {
            JObject json = JObject.Parse(jsonString);
            var jobs = new List<Job>();

            foreach (var job in json["jobs"])
            {
                var newJob = new Job() 
                { 
                    Id = job["id"].ToString(),
                };

                var rowIndex = -1;
                var previousRowIndex = rowIndex;

                foreach (var dataArray in job["data"])
                {
                    var collumnIndex = -1;
                    rowIndex++;

                    foreach (var cell in dataArray)
                    {
                        collumnIndex++;
                        var cellCoordinates = $"{IntToCollumnName(collumnIndex)}{rowIndex + 1}";

                        Cell newCell;
                        if (IsFormula(cell))
                        {
                            var formula = _formulaJsonParser.Parse(cell.ToString(), newJob);

                            if (formula.Cells.Count() == 0)
                            {
                                newCell = new Cell(cellCoordinates, "Reference not found.", Cell.Type.Error);
                            }
                            else
                            {
                                Cell.Type cellType;
                                if (formula is IsGreaterFormulaStrategy || formula is IsEqualFormulaStrategy)
                                    cellType = Cell.Type.Boolean;
                                else
                                    cellType = formula.Cells.First().ValueType;

                                newCell = new Cell(cellCoordinates, cellType, formula);
                            }
                        }
                        else
                        {
                            var property = JsonParserUtilities.GetCellProperty(cell);
                            var cellType = JsonParserUtilities.StringToCellType(property.Name);

                            newCell = new Cell(cellCoordinates, property.Value.ToString(), cellType);
                        }

                        if (rowIndex != previousRowIndex)
                        {
                            newJob.Data.Add(new List<Cell>());
                            previousRowIndex = rowIndex;
                        }

                        newJob.Data[rowIndex].Add(newCell);
                    }
                }

                jobs.Add(newJob);
            }

            return jobs;
        }

        private string IntToCollumnName(int index)
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            var value = "";

            if (index >= letters.Length)
                value += letters[index / letters.Length - 1];

            value += letters[index % letters.Length];

            return value;
        }

        private bool IsFormula(JToken cell)
        {
            return cell.ToObject<JObject>().Properties().First().Name == "formula" ? true : false;
        }
    }
}
