using RetroFinder.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace RetroFinder.Output
{
    public class XMLSerializer: ISerializer
    {
        public void SerializeAnalysisResult(SequenceAnalysis analysis)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SerializationOutput));

            string filePath = Path.Combine(analysis.FolderOfResult, $"{analysis.Sequence.Id}.xml");
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    serializer.Serialize(fileStream, analysis.Output);
                }
            }
            catch (Exception e)
            {
                IOManager.OutputError("XML", filePath, e.Message);
                return;
            }


            IOManager.OutputCreated("XML", filePath);
        }

        public ISerializer Clone()
        {
            return new XMLSerializer();
        }
    }
}
