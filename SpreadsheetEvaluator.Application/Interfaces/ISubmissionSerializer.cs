using SpreadsheetEvaluator.Application.Models;

namespace SpreadsheetEvaluator.Application.Interfaces
{
    public interface ISubmissionSerializer
    {
        public string Serialize(Submission submission);
    }
}
