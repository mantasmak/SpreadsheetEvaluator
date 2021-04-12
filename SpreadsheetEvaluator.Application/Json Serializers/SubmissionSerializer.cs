using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpreadsheetEvaluator.Application.Interfaces;
using SpreadsheetEvaluator.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpreadsheetEvaluator.Application.Json_Serializers
{
    public class SubmissionSerializer : ISubmissionSerializer
    {
        public string Serialize(Submission submission)
        {
            /*var jobs = submission.Results.ToList();
            var values = jobs.GroupBy(
                j => j.Id,
                j => j.Data,
                (key, v) => new {JobId = key, Data = v.Where(c => c.Where( d => d.Value)})*/

            /*var jobs = submission.Results.ToList();
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var jobsWithGroupedCellsByRows = new List<Job>();

            foreach (var job in jobs)
            {
                var previousRow = string.Empty;
                var jobRows = new List<List<Cell>>();

                foreach (var cell in job.Data)
                {
                    var currentRowString = String.Concat(cell.Coordinates.Except(letters));
                    var currentRowIndex = int.Parse(currentRowString) - 1;
                    if(currentRowString == previousRow)
                    {
                        jobRows[currentRowIndex].Add(cell);
                    }
                    else
                    {
                        jobRows.Add(new List<Cell>());
                        jobRows[currentRowIndex].Add(cell);
                    }
                }

                jobsWithGroupedCellsByRows.Add(new Job { Id = job.Id, Data})
            }*/

            JObject jsonObject = JObject.FromObject(new
            {
                email = submission.Email,
                results = submission.Results.Select(job => new
                {
                    id = job.Id,
                    data = job.Data.Select(data => data.Select(cell => new
                    {
                        value = cell.ValueType != Cell.Type.Error ? new
                        {
                            number = cell.ValueType == Cell.Type.Number ? float.Parse(cell.Value) : (float?) null,
                            text = cell.ValueType == Cell.Type.Text ? cell.Value : null,
                            boolean = cell.ValueType == Cell.Type.Boolean ? bool.Parse(cell.Value) : (bool?) null
                        } : null,

                        error = cell.ValueType == Cell.Type.Error ? cell.Value : null
                    }))
                })
            }, new JsonSerializer { NullValueHandling = NullValueHandling.Ignore } );

            return jsonObject.ToString();
        }
    }
}
