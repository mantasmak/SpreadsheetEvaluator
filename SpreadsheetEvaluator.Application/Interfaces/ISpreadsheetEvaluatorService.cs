using System.Threading.Tasks;

namespace SpreadsheetEvaluator.Application.Interfaces
{
    interface ISpreadsheetEvaluatorService
    {
        Task<string> EvaluateSpreadsheet();
    }
}
