using RetroFinder.Models;
using System.Collections;

namespace RetroFinder.Output
{
    public interface ISerializer
    {
        void SerializeAnalysisResult(SequenceAnalysis analysis);
        ISerializer Clone();
    }
}
