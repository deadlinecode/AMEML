using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using System.Text;

namespace AMEML.Helpers
{
    internal class Converter
    {
        public static List<Dictionary<string, object>> ParseCSV(string CSVString)
        {
            var data = new List<Dictionary<string, object>>();

            using (var reader = new StringReader(CSVString))
            using (var parser = new TextFieldParser(reader))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                string[] headers = null;
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();

                    if (headers == null) headers = fields;
                    else
                    {
                        var jsonObject = new Dictionary<string, object>();
                        for (int i = 0; i < fields.Length; i++) jsonObject[headers[i]] = fields[i];
                        data.Add(jsonObject);
                    }
                }
            }

            return data;
        }

        public static string ToCSV(List<Dictionary<string, object>> data)
        {
            var csvBuilder = new StringBuilder();

            // Append headers
            var headers = data.First().Keys;
            csvBuilder.AppendLine(string.Join(",", headers));

            // Append data
            foreach (var item in data)
            {
                csvBuilder.AppendLine(string.Join(",", item.Values));
            }

            return csvBuilder.ToString();
        }

        public static string ToJSON(List<Dictionary<string, object>> data)
        {
            return JsonConvert.SerializeObject(data, Formatting.Indented);
        }
    }
}
