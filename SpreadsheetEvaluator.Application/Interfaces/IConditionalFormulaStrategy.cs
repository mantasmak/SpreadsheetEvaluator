namespace SpreadsheetEvaluator.Application.Interfaces
{
    interface IConditionalFormulaStrategy : IFormulaStrategy
    {
        string Evaluate();
    }
}
