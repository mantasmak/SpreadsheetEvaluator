using System.Collections.Generic;

namespace SpreadsheetEvaluator.Application.Models
{
    public class Job
    {
        public string Id { get; set; }
        public List<Cell> Cells { get; } = new List<Cell>();
    }
}
