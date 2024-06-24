using System.Collections.Generic;
using System.Collections.Concurrent;
using System;
using System.Linq;
using RetroFinder.Exceptions;
namespace RetroFinder.Models
{
    public class DomainDatabase
    {
        private ConcurrentDictionary<FeatureType, IEnumerable<string>> _domainMap;  
        public DomainDatabase(string path)
        {
            _domainMap = new ConcurrentDictionary<FeatureType, IEnumerable<string>>();
            IEnumerable<FastaSequence> sequences = FastaUtils.Parse(path);
            if (sequences == null)
            {
                throw new InvalidDomainDatabaseException("Invalid format.");
            }

            foreach (FastaSequence seq in sequences)
            {
                string strType = seq.Id.Split(" ")[0].ToUpper();
                FeatureType type;
                if (Enum.TryParse<FeatureType>(strType, ignoreCase: true, out type))
                {
                    _domainMap.AddOrUpdate(type, new List<string>() { seq.Sequence }, (k, v) => v.Append(seq.Sequence));
                }
                else
                {
                    throw new InvalidDomainDatabaseException("Unknown feature. No case sensitive.");
                }
            }
        }

        public IEnumerable<string> GetDomainSequences(FeatureType type)
        {
            return _domainMap.TryGetValue(type, out IEnumerable<string> value) ? value : Enumerable.Empty<string>();
        }
    }
}
