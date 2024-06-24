using RetroFinder.Models;
using RetroFinder.Output;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RetroFinder
{
    public class FastaUtils
    {
        private static HashSet<char> KnownBases = new HashSet<char>() { 'A', 'C', 'G', 'T', 'N' };
        public static bool Validate(string path)
        {
            int fastaSequencesCount = 0;
            HashSet<string> ids = new HashSet<string>();

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string id = sr.ReadLine();
                    int lineNumber = 1;
                    while (id != null)
                    {
                        id = id.TrimEnd();
                        if (!ValidateSequenceID(id, ids, lineNumber))
                        {
                            return false;
                        }
                        lineNumber++;

                        id = id.Substring(1);
                        ids.Add(id);

                        bool isNonEmptySequence = false;

                        string seqLine;
                        while ((seqLine = sr.ReadLine()) != null) // Sequence can be on multiple lines
                        {
                            if (seqLine.Length != 0 && seqLine[0] == '>') // New sequence ID break
                            {
                                break;
                            }

                            if (!ValidateSequence(seqLine, lineNumber))
                            {
                                return false;
                            }
                            lineNumber++;

                            isNonEmptySequence = isNonEmptySequence || seqLine.Length > 0;
                        }
                        
                        if (!isNonEmptySequence)
                        {
                            IOManager.InvalidFastaFile($"Missing sequence for ID: {id}", lineNumber);
                            return false;
                        }
                        fastaSequencesCount += 1;

                        id = seqLine;
                    }
                }
            }
            catch (Exception e)
            {
                IOManager.FasteFileError(e.Message);
                return false;
            }

            if (fastaSequencesCount == 0)
            {
                IOManager.InvalidFastaFile("No sequence present", 1);
                return false;
            }

            return true;
        }

        private static bool ValidateSequenceID(string idLine, HashSet<string> ids, int lineNumber)
        {
            if (idLine.Length <= 1 || idLine[0] != '>')
            {
                IOManager.InvalidFastaFile("Invalid sequence ID", lineNumber);
                return false;
            }

            if (ids.Contains(idLine.Substring(1)))
            {
                IOManager.InvalidFastaFile("Duplicit sequence ID", lineNumber);
                return false;
            }

            return true;
        }

        private static bool ValidateSequence(string seqLine, int lineNumber)
        {
            if (!seqLine.All(c => KnownBases.Contains(c)))
            {
                IOManager.InvalidFastaFile("Sequence constains unknown base", lineNumber);
                return false;
            }

            return true;
        }

        public static IEnumerable<FastaSequence> Parse(string path)
        {
            if (path == null || !Validate(path))
            {
                return null;
            }

            List<FastaSequence> fastaSequences = new List<FastaSequence>();

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string id = sr.ReadLine();

                    while (id != null)
                    {
                        id = id.TrimEnd().Substring(1);
                        string seqLine;
                        StringBuilder sequence = new StringBuilder();

                        while ((seqLine = sr.ReadLine()) != null)
                        {
                            if (seqLine.Length != 0 && seqLine[0] == '>')
                            {
                                break;
                            }

                            sequence.Append(seqLine);
                        }

                        fastaSequences.Add(new FastaSequence(id, sequence.ToString()));
                        id = seqLine;
                    }
                }
            }
            catch (Exception e)
            {
                IOManager.FasteFileError(e.Message);
                return null;
            }

            return fastaSequences;
        }
    }
}
