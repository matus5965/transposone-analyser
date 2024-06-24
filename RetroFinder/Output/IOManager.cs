using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace RetroFinder.Output
{
    public static class IOManager
    {
        private static int MaxDegreeOfParallelism = 8;
        public static void GreetUser()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("RETRO FINDER");
            Console.WriteLine($"Current folder: \"{Directory.GetCurrentDirectory()}\"");
            Console.WriteLine();
            Console.WriteLine("WARNING:");
            Console.WriteLine("Result of analysis is stored in a same folder as provided Fasta file.");
            Console.WriteLine("Make sure the folder has proper access permissions.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }

        public static string GetPath()
        {
            string path = null;
            while (path == null)
            {
                Console.Write("Fasta file path: ");
                string possiblePath = Console.ReadLine();
                if (possiblePath == null || possiblePath == "")
                {
                    continue;
                }

                path = possiblePath;
            }

            return path;
        }

        public static void InvalidFileType(string path)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Invalid file was provided: \"{path}\"");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void StartValidate()
        {
            Console.WriteLine("Starts validating provided Fasta file.");
        }

        public static void InvalidFastaFile(string reason, int lineNumber)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Provided Fasta file has invalid format. Namely: \"{reason}\" on line {lineNumber}.");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void FasteFileError(string reason)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error happend during reading the file.");
            Console.WriteLine($"Reason: {reason}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void ShowValidationState(bool state)
        {
            if (state)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Validation state: OK");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Validation state: NOK");
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        public static int GetDegreeOfParallelism()
        {
            int? degree = null;
            while (degree == null)
            {
                Console.Write($"Set degree of parallelism(positive integer less then {MaxDegreeOfParallelism}): ");
                string line = Console.ReadLine();
                string[] tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length == 0)
                {
                    continue;
                }
                if (tokens.Length != 1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid number of tokens. One positive integer.");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }

                int n;
                if (!int.TryParse(tokens[0], NumberStyles.Integer, null, out n) || n <= 0 || n > MaxDegreeOfParallelism)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Invalid argument. Positive integer expected less then {MaxDegreeOfParallelism} expected.");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }

                degree = n;
            }

            return degree.Value;
        }

        public static ISerializer GetSerializers()
        {
            List<ISerializer> serializers = new List<ISerializer>();
            Console.WriteLine("Output file types:");
            Console.WriteLine("A: XML");
            Console.WriteLine("B: JSON");
            Console.WriteLine("C: XML and JSON");

            string selectedType = null;
            while (selectedType == null)
            {
                Console.Write("Select wanted type: ");
                string line = Console.ReadLine();
                if (line == null || (line = line.Trim()) == "")
                {
                    continue;
                }

                if (!(line.Equals("A") || line.Equals("B") || line.Equals("C")))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid selected output file type. Select A, B or C.");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }

                selectedType = line;
            }

            Console.WriteLine();
            if (selectedType == "A")
            {
                return  new XMLSerializer();
            }
            else if (selectedType == "B")
            {
                return new JSONSerializer();
            }
            return new BothSerializer();
        }

        public static void ShowElementCount(string seqID, int elementCount)
        {
            (string s1, string s2) = elementCount == 1 ? ("was", "element") : ("were", "elements");
            Console.WriteLine($"In sequence \"{seqID}\" {s1} found {elementCount} {s2}");
        }

        public static void OutputCreated(string type, string name)
        {
            Console.WriteLine($"{type.ToUpper()} file \"{name}\" was created");
        }

        public static void OutputError(string type, string name, string reason)
        {
            Console.WriteLine($"{type.ToUpper()} file \"{name}\" was not created.\nReason: {reason}");
        }
        public static void Finish()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("All sequences have been analyzed.");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
