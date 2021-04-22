using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpreadsheetEvaluator.Application.Interfaces;
using SpreadsheetEvaluator.Application.Models;
using System.Linq;

namespace SpreadsheetEvaluator.Application.Json_Serializers
{
    public class SubmissionSerializer : ISubmissionSerializer
    {
        public string Serialize(Submission submission)
        {
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
