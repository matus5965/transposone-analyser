using RetroFinder.Output;
using System.IO;

namespace RetroFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(Path.Combine("..", "..", ".."));
            IOManager.GreetUser();
            string path = IOManager.GetPath();

            RetroFinder rf = new RetroFinder();
            rf.Analyze(path);
        }
    }
}
