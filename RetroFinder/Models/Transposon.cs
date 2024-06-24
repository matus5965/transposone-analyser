using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Xml.Serialization;

namespace RetroFinder.Models
{
    public class Transposon
    {
        public (int start, int end) Location { get; set; }

        public (int start, int end) InnerStructureLocation { get; set; }

        private List<Feature> _features = new List<Feature>();

        public IEnumerable<Feature> Features
        {
            get
            {
                return _features.OrderBy(f => f.Type).ToList();
            }

            private set
            {
                _features = value.ToList();
            }
        }

        public void InsertFeature(Feature f)
        {
            _features.Add(f);
        }
    }
}
