using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroFinder.Models.GraphModel
{
    public class Node
    {
        public Feature Feature;
        public Node Ancestor;
        public int TotalScore;
        public int FeatureCount;
    }
}
