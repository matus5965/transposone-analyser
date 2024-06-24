using RetroFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Schema;

namespace RetroFinder
{
    public class LTRFinder
    {
        public const int FeatureCount = 7;
        public static readonly Regex RetroRegex = new Regex(@"([ACTG]{100,300})([ACTGN]{1000,3500})\1");

        public FastaSequence Sequence { get; set; }

        public IEnumerable<Transposon> IdentifyElements()
        {
            MatchCollection matches = RetroRegex.Matches(Sequence.Sequence);
            List<Transposon> transposons = new List<Transposon>();

            foreach (Match match in matches)
            {
                (int start, int end) transLocation = (match.Index, match.Index + match.Length - 1); 
                Transposon transposon = new Transposon { Location = transLocation };
                Feature[] Features = new Feature[FeatureCount];

                Group ltrLeft = match.Groups[1];
                transposon.InnerStructureLocation = (transLocation.start + ltrLeft.Length, transLocation.end - ltrLeft.Length);

                Feature feature = new Feature
                {
                    Type = FeatureType.LTRLeft,
                    Location = (transposon.Location.start, ltrLeft.Index + ltrLeft.Length - 1),
                };
                transposon.InsertFeature(feature);

                feature = new Feature
                {
                    Type = FeatureType.LTRRight,
                    Location = (match.Index + match.Length - ltrLeft.Length, transposon.Location.end),
                };
                transposon.InsertFeature(feature);

                transposons.Add(transposon);
            }

            return transposons;
        }
    }
}
