using System.Collections.Generic;

namespace SpreadsheetEvaluator.Application.Models
{
    public class Job
    {
        public string Id { get; set; }

        public List<List<Cell>> Data { get; set; } = new List<List<Cell>>();
    }
}
