using System.Collections.Generic;

namespace SpreadsheetEvaluator.Application.Models
{
    public class Submission
    {
        public string Email { get; set; }

        public IEnumerable<Job> Results { get; set; }
    }
}
