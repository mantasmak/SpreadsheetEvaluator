using SpreadsheetEvaluator.Application.Interfaces;
using SpreadsheetEvaluator.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpreadsheetEvaluator.Application.Evaluators
{
    public class FormulaEvaluator : IEvaluator
    {
        public void Evaluate(List<List<Cell>> spreadsheet)
        {
            var allCells = FlattenSpreadsheet(spreadsheet);
            var formulaCells = GetAllFormulaCells(allCells);

            EvaluateReferencedValues(allCells, formulaCells);

            EvaluateFormulas(formulaCells);
        }

        private List<Cell> FlattenSpreadsheet(List<List<Cell>> spreadsheet)
        {
            var listOfAllCells = new List<Cell>();

            foreach(var row in spreadsheet)
            {
                foreach(var cell in row)
                {
                    listOfAllCells.Add(cell);
                }
            }

            return listOfAllCells;
        }

        private List<Cell> GetAllFormulaCells(List<Cell> allCells)
        {
            var formulaCells = new List<Cell>();

            foreach (var cell in allCells)
            {
                if (cell.Formula != null)
                {
                    formulaCells.Add(cell);
                }
            }

            return formulaCells;
        }

        private void EvaluateReferencedValues(List<Cell> allCells, List<Cell> formulaCells)
        {
            foreach (var formulaCell in formulaCells)
            {
                var formulaOperands = formulaCell.Formula.Cells;

                foreach (var formulaOperand in formulaOperands)
                {
                    if (formulaOperand.Value == null)
                    {
                        var evaluatedOperand = EvaluateReferences(formulaOperand, allCells);

                        if (evaluatedOperand != null)
                        {
                            formulaOperand.Value = evaluatedOperand.Value;
                            formulaOperand.ValueType = evaluatedOperand.ValueType;
                        }
                        else
                        {
                            formulaCell.ValueType = Cell.Type.Error;
                            formulaOperand.Value = "Could not evaluate value";
                        }
                    }
                }
            }
        }

        private Cell EvaluateReferences(Cell formulaOperand, List<Cell> allCells)
        {
            var referencedCell = allCells.Where(cell => cell.Coordinates == formulaOperand.Coordinates)
                                                 .FirstOrDefault();

            if (referencedCell.Value != null)
            {
                return referencedCell;
            }

            if (referencedCell.Formula != null)
            {
                referencedCell.Value = referencedCell.Formula.Evaluate();
                referencedCell.ValueType = referencedCell.Formula.Cells.First().ValueType;

                if (referencedCell.Value != null)
                    return referencedCell;

                return EvaluateReferences(referencedCell.Formula.Cells.First(), allCells);
            }

            return null;
        }

        private void EvaluateFormulas(List<Cell> formulaCells)
        {
            foreach(var formulaCell in formulaCells)
            {
                try
                {
                    formulaCell.Value = formulaCell.Formula.Evaluate();
                    if (formulaCell.ValueType == Cell.Type.Error)
                        formulaCell.ValueType = formulaCell.Formula.Cells.First().ValueType;
                }
                catch(Exception)
                {
                    formulaCell.ValueType = Cell.Type.Error;
                    formulaCell.Value = "Failed to evaluate value";
                }
            }
        }
    }
}
