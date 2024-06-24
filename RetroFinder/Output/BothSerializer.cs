using System;

namespace RetroFinder.Output
{
    public class BothSerializer: ISerializer
    {
        public void SerializeAnalysisResult(SequenceAnalysis analysis)
        {
            JSONSerializer jsonSerializer = new JSONSerializer();
            jsonSerializer.SerializeAnalysisResult(analysis);

            XMLSerializer xmlSerializer = new XMLSerializer();
            xmlSerializer.SerializeAnalysisResult(analysis);
        }

        public ISerializer Clone()
        {
            return new BothSerializer();
        }
    }
}
