using RetroFinder.Output;
using RetroFinder.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.IO;

namespace RetroFinder
{
    public class RetroFinder
    {
        private readonly string DomainsFile = Path.Join(".", "Data", "protein_domains.fa");
        public void Analyze(string path)
        {
            IOManager.StartValidate();
            IEnumerable<FastaSequence> fastaSequences = FastaUtils.Parse(path);
            IOManager.ShowValidationState(fastaSequences != null);
            if (fastaSequences == null)
            {
                return;
            }

            string folderOfResult = Path.GetDirectoryName(path);

            int parallelDegree = IOManager.GetDegreeOfParallelism();

            ISerializer serializer = IOManager.GetSerializers();
            DomainDatabase database = new DomainDatabase(DomainsFile);

            List<SequenceAnalysis> sequenceAnalysis = new List<SequenceAnalysis>();
            foreach (FastaSequence fs in fastaSequences)
            {
                sequenceAnalysis.Add(new SequenceAnalysis(fs, serializer.Clone(), database, folderOfResult));
            }

            Parallel.ForEach(sequenceAnalysis, new ParallelOptions { MaxDegreeOfParallelism = parallelDegree }, sa => sa.Analyze());
            IOManager.Finish();
        }
    }
}
