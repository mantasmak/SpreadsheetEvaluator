using Newtonsoft.Json;
using SpreadsheetEvaluator.Application.Models;
using SpreadsheetEvaluator.Application.Strategies;
using System.Collections.Generic;
using System.IO;

namespace SpreadsheetEvaluator.Application.Parsers
{
    public class JobJsonParser : IJobJsonParser
    {
        private JsonTextReader _reader;

        public Submission Parse(string jsonString)
        {
            _reader = new JsonTextReader(new StringReader(jsonString));
            var url = string.Empty;
            var jobs = new List<Job>();
            
            while(_reader.Read())
            {
                switch(_reader.Value)
                {
                    case "submissionUrl":
                        url = _reader.ReadAsString();
                        break;

                    case "jobs":
                        jobs = ParseArrayOfJobs();
                        break;
                }    
            }

            return new Submission() { SubmissionUrl = url, Results = jobs};
        }

        private List<Job> ParseArrayOfJobs()
        {
            var parsedJobs = new List<Job>();

            while(_reader.TokenType != JsonToken.EndArray)
            {
                _reader.Read();

                if (_reader.TokenType == JsonToken.StartObject)
                    parsedJobs.Add(ParseJob());
            }

            return parsedJobs;
        }

        private Job ParseJob()
        {
            var newJobId = string.Empty;
            var newJobData = new List<List<Cell>>();

            while (_reader.TokenType != JsonToken.EndObject)
            {
                _reader.Read();

                switch (_reader.Value)
                {
                    case "id":
                        newJobId = GetJobId();
                        break;

                    case "data":
                        newJobData = ParseJobData();
                        break;
                }
            }

            return new Job() { Id = newJobId, Data = newJobData };
        }

        private string GetJobId()
        {
            return _reader.ReadAsString();
        }

        private List<List<Cell>> ParseJobData()
        {
            var jobData = new List<List<Cell>>();
            var rowIndex = -1;

            while (_reader.TokenType != JsonToken.EndArray)
            {
                if (_reader.TokenType == JsonToken.StartArray)
                {
                    _reader.Read();

                    if (_reader.TokenType == JsonToken.EndArray)
                        break;

                    rowIndex++;
                    jobData.Add(ParseJobDataRow(rowIndex));
                }

                _reader.Read();
            }

            return jobData;
        }

        private List<Cell> ParseJobDataRow(int rowIndex)
        {
            var jobDataRow = new List<Cell>();
            var columnIndex = -1;

            while (_reader.TokenType != JsonToken.EndArray)
            {
                if(_reader.TokenType == JsonToken.StartObject)
                {
                    columnIndex++;
                    jobDataRow.Add(ParseCell(rowIndex, columnIndex));
                }

                _reader.Read();
            }

            return jobDataRow;
        }

        private Cell ParseCell(int rowIndex, int columnIndex)
        {
            while (_reader.TokenType != JsonToken.EndObject)
            {
                _reader.Read();

                switch(_reader.Value)
                {
                    case "value":
                        return ParseValueCell(rowIndex, columnIndex);

                    case "formula":
                        return ParseFormulaCell(rowIndex, columnIndex);
                }
            }

            return null;
        }

        private Cell ParseValueCell(int rowIndex = -1, int columnIndex = -1)
        {
            var coordinates = IndexToCoordinates(rowIndex, columnIndex);
            var value = string.Empty;
            var valueType = Cell.Type.Error;
            
            while (_reader.TokenType != JsonToken.EndObject)
            {
                _reader.Read();

                switch (_reader.Value)
                {
                    case "number":
                        value = _reader.ReadAsString();
                        valueType = Cell.Type.Number;
                        break;

                    case "text":
                        value = _reader.ReadAsString();
                        valueType = Cell.Type.Text;
                        break;

                    case "boolean":
                        value = _reader.ReadAsString();
                        valueType = Cell.Type.Boolean;
                        break;
                }
            }

            return new Cell() 
            { 
                Coordinates = coordinates, 
                Value = value, 
                ValueType = valueType 
            };
        }

        private string IndexToCoordinates(int rowIndex, int columnIndex)
        {
            var asciiLetterAValue = 65;
            var columnCoordinate = (char)(asciiLetterAValue + (columnIndex));
            var rowCoordinate = (rowIndex + 1).ToString();

            return columnCoordinate + rowCoordinate;
        }

        private Cell ParseFormulaCell(int rowIndex = -1, int columnIndex = -1)
        {
            while(_reader.TokenType != JsonToken.EndObject)
            {
                _reader.Read();

                switch(_reader.Value)
                {
                    case "value":
                        return ParseValueCell(rowIndex, columnIndex);

                    case "reference":
                        return ParseReferenceFormulaCell(rowIndex, columnIndex);

                    case "sum":
                        return ParseMultiOperandFormula(rowIndex, columnIndex, new SumFormulaStrategy());

                    case "multiply":
                        return ParseMultiOperandFormula(rowIndex, columnIndex, new MultiplyFormulaStrategy());

                    case "divide":
                        return ParseMultiOperandFormula(rowIndex, columnIndex, new DivideFormulaStrategy());

                    case "is_greater":
                        return ParseMultiOperandFormula(rowIndex, columnIndex, new IsGreaterFormulaStrategy());

                    case "is_equal":
                        return ParseMultiOperandFormula(rowIndex, columnIndex, new IsEqualFormulaStrategy());

                    case "not":
                        return ParseSingleOperandFormula(rowIndex, columnIndex, new NotFormulaStrategy());

                    case "and":
                        return ParseMultiOperandFormula(rowIndex, columnIndex, new AndFormulaStrategy());

                    case "or":
                        return ParseMultiOperandFormula(rowIndex, columnIndex, new OrFormulaStrategy());

                    case "if":
                        return ParseIfFormula(rowIndex, columnIndex);

                    case "concat":
                        return ParseMultiOperandFormula(rowIndex, columnIndex, new ConcatFormulaStrategy());
                }
            }

            return new Cell() { Value = "Wrong formula format", ValueType = Cell.Type.Error };
        }

        private Cell ParseReferenceFormulaCell(int rowIndex, int columnIndex)
        {
            var cell = CreateEmptyCell(rowIndex, columnIndex);
            var referencedCell = ParseReferenceCell();

            cell.Formula = new ReferenceFormulaStrategy();
            cell.Formula.Cells.Add(referencedCell);

            return cell;
        }

        private Cell ParseMultiOperandFormula(int rowIndex, int columnIndex, FormulaStrategy strategy)
        {
            var cell = CreateEmptyCell(rowIndex, columnIndex);
            var arguments = ParseArrayOfCells();

            strategy.Cells = arguments;
            cell.Formula = strategy;
            cell.ValueType = strategy.ResultValueType;

            return cell;
        }

        private Cell ParseSingleOperandFormula(int rowIndex, int columnIndex, FormulaStrategy strategy)
        {
            var cell = CreateEmptyCell(rowIndex, columnIndex);
            var argument = ParseReferenceCell();

            strategy.Cells.Add(argument);
            cell.Formula = strategy;
            cell.ValueType = strategy.ResultValueType;

            return cell;
        }

        private Cell ParseIfFormula(int rowIndex, int columnIndex)
        {
            var cell = CreateEmptyCell(rowIndex, columnIndex);
            var conditionStrategy = ParseFormulaCell().Formula;
            var values = new List<Cell>();

            foreach (var conditionFormulaOperand in conditionStrategy.Cells)
            {
                values.Add(conditionFormulaOperand);
            }

            var cellIfTrue = ParseReferenceCell();
            var cellIfFalse = ParseReferenceCell();
            values.Add(cellIfTrue);
            values.Add(cellIfFalse);
            cell.Formula = new IfFormulaStrategy()
            {
                Cells = values,
                ConditionStrategy = conditionStrategy
            };

            return cell;
        }

        private Cell CreateEmptyCell(int rowIndex, int columnIndex)
        {
            var coordinates = IndexToCoordinates(rowIndex, columnIndex);

            return new Cell() { Coordinates = coordinates };
        }

        private Cell CreateEmptyCell(string coordinates)
        {
            return new Cell() { Coordinates = coordinates };
        }

        private List<Cell> ParseArrayOfCells()
        {
            var cells = new List<Cell>();

            while(_reader.TokenType != JsonToken.EndArray)
            {
                _reader.Read();

                if ((string)_reader.Value == "value")
                {
                    cells.Add(ParseValueCell());
                }

                if ((string)_reader.Value == "reference")
                {
                    cells.Add(ParseReferenceCell());
                }
            }

            return cells;
        }

        private Cell ParseReferenceCell()
        {
            do
            {
                if ((string)_reader.Value == "reference")
                {
                    return CreateEmptyCell(_reader.ReadAsString());
                }
            }
            while (_reader.Read());

            return null;
        }
    }
}
