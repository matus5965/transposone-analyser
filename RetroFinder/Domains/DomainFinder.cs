using RetroFinder.Models;
using System;
using System.Collections.Generic;

namespace RetroFinder.Domains
{
    public class DomainFinder
    {
        private const int MatchScore = 3;
        private const int GapPenalty = -2;
        private const int NoMatchScore = -3;

        private string _sequence; // Transposone sequence
        private DomainDatabase _database;
        public int Shift;
        public DomainFinder(string sequence, DomainDatabase database, int shift)
        {
            _sequence = sequence;
            _database = database;
        }

        public IEnumerable<Feature> IdentifyDomains()
        {
            List<Feature> features = new List<Feature>();

            foreach (FeatureType ft in Enum.GetValues(typeof(FeatureType)))
            {
                foreach(string seq in _database.GetDomainSequences(ft))
                {
                    Feature alligned = Allign(_sequence, seq);
                    if (alligned == null)
                    {
                        continue;
                    }

                    alligned.Type = ft;
                    features.Add(alligned);
                }
            }

            return features;
        }

        private Feature Allign(string reference, string sample)
        {
            Position[,] matrix = new Position[reference.Length + 1, sample.Length + 1];
            InitializeMatrix(matrix, reference.Length, sample.Length);
            Position maximum = null;

            for (int row = 1; row < reference.Length + 1; row++)
            {
                for (int col = 1; col < sample.Length + 1; col++)
                {
                    Position pos = ResolvePosition(row, col, reference[row - 1] == sample[col - 1], matrix);
                    matrix[row, col] = pos;

                    if ((maximum == null && pos.Score > 0) || (maximum != null && pos.Score > maximum.Score))
                    {
                        maximum = pos;
                    }
                }
            }

            return GetBestAllign(matrix, maximum, sample.Length / 2);
        }

        private void InitializeMatrix(Position[,] matrix, int lenReference, int lenSample)
        {
            for (int row = 0; row < lenReference + 1; row++)
            {
                matrix[row, 0] = new Position { Score = 0, Indeces = (row, 0), Ancestor = null };
            }
            for (int col = 0; col < lenSample + 1; col++)
            {
                matrix[0, col] = new Position { Score = 0, Indeces = (0, col), Ancestor = null };
            }
        }

        private Position ResolvePosition(int row, int col, bool isMatch, Position[,] matrix)
        {
            int matchScore = matrix[row - 1, col - 1].Score + (isMatch ? MatchScore : NoMatchScore);
            int referenceGapPenalty = matrix[row, col - 1].Score + GapPenalty;
            int sampleGapPenalty = matrix[row - 1, col].Score + GapPenalty;

            Position position = new Position { Score = 0, Indeces = (row, col), Ancestor = null };
            if (matchScore > 0 && matchScore >= referenceGapPenalty && matchScore >= sampleGapPenalty)
            {
                position.Score = matchScore;
                position.Ancestor = matrix[row - 1, col - 1];
            }

            if (referenceGapPenalty > 0 && referenceGapPenalty >= matchScore && referenceGapPenalty >= sampleGapPenalty)
            {
                position.Score = referenceGapPenalty;
                position.Ancestor = matrix[row, col - 1];
            }

            if (sampleGapPenalty > 0 && sampleGapPenalty >= matchScore && sampleGapPenalty >= referenceGapPenalty)
            {
                position.Score = sampleGapPenalty;
                position.Ancestor = matrix[row - 1, col];
            }

            return position;
        }

        private Feature GetBestAllign(Position[,] matrix, Position end, int minimumAllignLength)
        {
            if (end == null)
            {
                return null;
            }

            Position pos = end;
            while (pos.Ancestor.Score != 0)
            {
                pos = pos.Ancestor;
            }

            if (end.Indeces.col - pos.Indeces.col + 1 < minimumAllignLength)
            {
                return null;
            }

            return new Feature { Score = end.Score, Location = (pos.Indeces.row - 1 + Shift, end.Indeces.row - 1 + Shift) };
        }
    }
}
