using RetroFinder.Models;
using RetroFinder.Models.GraphModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RetroFinder.Domains
{
    public class DomainPicker
    {
        private IEnumerable<Feature> _possibleFeatures { get; set; }

        public DomainPicker(IEnumerable<Feature> possibleFeatures)
        {
            _possibleFeatures = possibleFeatures;
        }
        public IEnumerable<Feature> PickDomains()
        {
            Node node = GetBestNode();
            if (node == null)
            {
                return new List<Feature>();
            }

            List<Feature> result = new List<Feature>();
            while (node != null)
            {
                result.Add(node.Feature);
                node = node.Ancestor;
            }

            result.Reverse();
            return result;
        }

        public Node GetBestNode()
        {
            Node bestNode = null;
            List<Node> nodes = new List<Node>();

            foreach (var feature in _possibleFeatures.OrderBy(f => f.Type))
            {
                Node newNode = new Node() { Feature = feature, Ancestor = null, FeatureCount = 1, TotalScore = feature.Score };
                Node bestParent = null;

                bool isLonger;
                bool isMoreScore;
                foreach (Node parent in nodes)
                {
                    if (parent.Feature.Type < feature.Type && parent.Feature.Location.end < feature.Location.start)
                    {
                        isLonger = bestParent == null || (parent.FeatureCount > bestParent.FeatureCount);
                        isMoreScore = bestParent == null || (parent.FeatureCount == bestParent.FeatureCount && parent.TotalScore > bestParent.TotalScore);

                        if (isLonger || isMoreScore)
                        {
                            bestParent = parent;
                        }
                    }
                }

                newNode.Ancestor = bestParent;
                if (bestParent != null)
                {
                    newNode.FeatureCount += bestParent.FeatureCount;
                    newNode.TotalScore += bestParent.TotalScore;
                }

                nodes.Add(newNode);
                isLonger = bestNode == null || (newNode.FeatureCount > bestNode.FeatureCount);
                isMoreScore = bestNode == null || (newNode.FeatureCount == bestNode.FeatureCount && newNode.TotalScore > bestNode.TotalScore);
                if (isLonger || isMoreScore)
                {
                    bestNode = newNode;
                }
            }

            return bestNode;
        }
    }
}
