using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RetroFinder.Output
{
    public class JSONSerializer: ISerializer
    {
        public void SerializeAnalysisResult(SequenceAnalysis analysis)
        {
            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() },
                WriteIndented = true
            };

            string jsonString = JsonSerializer.Serialize(analysis.Output, options);
            string filePath = Path.Combine(analysis.FolderOfResult, $"{analysis.Sequence.Id}.json");

            try
            {
                File.WriteAllText(filePath, jsonString);
            }
            catch (Exception e)
            {
                IOManager.OutputError("JSON", filePath, e.Message);
                return;
            }

            IOManager.OutputCreated("JSON", filePath);
        }

        public ISerializer Clone()
        {
            return new JSONSerializer();
        }
    }
}
