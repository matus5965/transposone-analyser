using RetroFinder.Models;
using System.Collections.Generic;
using RetroFinder.Output;
using System.Linq;
using RetroFinder.Domains;
using System.Threading.Tasks;

namespace RetroFinder
{
    public class SequenceAnalysis
    {
        public FastaSequence Sequence { get; set; }
        public IEnumerable<Transposon> Transposons { get; set; }
        public SerializationOutput Output { get; set; }
        public readonly string FolderOfResult;

        private readonly ISerializer _output;
        private readonly DomainDatabase _database;

        public SequenceAnalysis(FastaSequence sequence, ISerializer output, DomainDatabase database, string folderOfResult)
        {
            Sequence = sequence;
            _output = output;
            _database = database;
            FolderOfResult = folderOfResult;

        }

        public void Analyze()
        {
            LTRFinder ltrFinder = new LTRFinder();
            ltrFinder.Sequence = Sequence;
            Transposons = ltrFinder.IdentifyElements();

            IOManager.ShowElementCount(Sequence.Id, Transposons.Count());
            foreach (var t in Transposons)
            {
                AnalyzeTransposon(t);
            }

            //Parallel.ForEach(Transposons, t => AnalyzeTransposon(t));

            List<OutputTransposon> outputTransposons = new List<OutputTransposon>();
            foreach (var t in Transposons)
            {
                outputTransposons.Add(new OutputTransposon { Start = t.Location.start, End = t.Location.end, Features = t.Features.ToArray() });
            }

            Output = new SerializationOutput { Transposons = outputTransposons.ToArray() };
            _output.SerializeAnalysisResult(this);
        }

        private void AnalyzeTransposon(Transposon trans)
        {
            (int innerStart, int innerLength) = (trans.InnerStructureLocation.start, trans.InnerStructureLocation.end - trans.InnerStructureLocation.start + 1);
            string seq = Sequence.Sequence.Substring(innerStart, innerLength);

            DomainFinder domainFinder = new DomainFinder(seq, _database, trans.Location.start);
            domainFinder.Shift = trans.InnerStructureLocation.start;
            IEnumerable<Feature> features = domainFinder.IdentifyDomains();
            
            DomainPicker domainPicker = new DomainPicker(features);
            IEnumerable<Feature> pickedFeatures = domainPicker.PickDomains();

            foreach (Feature feature in pickedFeatures)
            {
                trans.InsertFeature(feature);
            }
        }
    }
}
