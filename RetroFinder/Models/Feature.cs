using System.Text.Json.Serialization;
using System.Threading;
using System.Xml.Serialization;

namespace RetroFinder.Models
{
    public class Feature
    {
        public FeatureType Type { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public (int start, int end) Location { get; set; }

        public int Start
        {
            get
            {
                return Location.start;
            }
            set
            {
                Location = (value, Location.end);
            }
        }

        public int End
        {
            get
            {
                return Location.end;
            }
            set
            {
                Location = (Location.start, value);
            }
        }

        [XmlIgnore]
        [JsonIgnore]
        public int Score { get; set; }
    }
}
