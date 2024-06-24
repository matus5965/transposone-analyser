using System.Xml.Serialization;

namespace RetroFinder.Output
{
    public class SerializationOutput
    {
        [XmlArrayItem]
        public OutputTransposon[] Transposons { get; set; }
    }
}
