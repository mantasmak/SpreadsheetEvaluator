using Newtonsoft.Json.Linq;
using SpreadsheetEvaluator.Application.Models;
using System;
using System.Linq;

namespace SpreadsheetEvaluator.Application.Utilities
{
    static class JsonParserUtilities
    {
        internal static Cell.Type StringToCellType(string type)
        {
            return (Cell.Type)Enum.Parse(typeof(Cell.Type), CapitalizeFirstLetter(type));
        }

        internal static JProperty GetCellProperty(JToken cell)
        {
            return cell.ToObject<JObject>().Properties().First().Value.ToObject<JObject>().Properties().First();
        }

        internal static string CapitalizeFirstLetter(string text) =>
        text switch
        {
            null => throw new ArgumentNullException(nameof(text)),
            "" => throw new ArgumentException($"{nameof(text)} cannot be empty", nameof(text)),
            _ => text.First().ToString().ToUpper() + text.Substring(1)
        };
    }
}
