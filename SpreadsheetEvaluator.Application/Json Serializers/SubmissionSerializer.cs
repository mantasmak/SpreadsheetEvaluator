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
                    data = job.Data.Select(data => new
                    {
                        value = data.ValueType != Cell.Type.Error ? new
                        {
                            number = data.ValueType == Cell.Type.Number ? data.Value : null,
                            text = data.ValueType == Cell.Type.Text ? data.Value : null,
                            boolean = data.ValueType == Cell.Type.Boolean ? data.Value : null
                        } : null,

                        error = data.ValueType == Cell.Type.Error ? data.Value : null
                    })
                })
            }, new JsonSerializer { NullValueHandling = NullValueHandling.Ignore } );

            return jsonObject.ToString();
        }
    }
}
