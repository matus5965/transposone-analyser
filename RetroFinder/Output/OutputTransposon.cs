using RetroFinder.Models;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace RetroFinder.Output
{
    public class OutputTransposon // I had problem with IEnumerable<Feature>. Therefore, this model was created
    {
        public int Start { get; set; }
        public int End { get; set; }

        [XmlArrayItem]
        public Feature[] Features { get; set; }
    }
}
