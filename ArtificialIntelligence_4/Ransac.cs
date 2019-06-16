using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;

namespace ArtificialIntelligence_4
{
    public class Ransac
    {
        public static readonly Random Random = new Random();

        public DenseMatrix FindBestTransform(
            ITransform transform,
            Dictionary<int, int> pairs,
            List<Tuple<double, double>> firstCoords,
            List<Tuple<double, double>> secondCoords,
            int cycles,
            double maxError,
            out Dictionary<int, int> bestPairs)
        {
            var kvpList = pairs.ToList();
            var transforms = new List<DenseMatrix>();
            for (var i = 0; i < cycles; i++)
            {
                var samplePoints = GetRandomPoints(kvpList, transform.PointsNeeded);
                transforms.Add(transform.GetTransformMatrix(samplePoints, firstCoords, secondCoords));
            }

            var bestTransform = transforms
                .Select(s => new
                {
                    Matrix = s,
                    Value = TestTransform(s, kvpList, firstCoords, secondCoords, maxError)
                })
                .OrderByDescending(o => o.Value)
                .First()
                .Matrix;
            bestPairs = GetBestPairsForTransform(bestTransform, pairs, firstCoords, secondCoords, maxError);

            return bestTransform;
        }

        private List<KeyValuePair<int, int>> GetRandomPoints(IReadOnlyList<KeyValuePair<int, int>> kvpList, int pointsNeeded)
        {
            var samplePoints = new List<KeyValuePair<int, int>>();
            while(samplePoints.Count < pointsNeeded)
            {
                var next = kvpList[Random.Next(kvpList.Count)];
                if (!samplePoints.Contains(next))
                {
                    samplePoints.Add(next);
                }
            }

            return samplePoints;
        }

        private double TestTransform(
            DenseMatrix transform,
            IReadOnlyCollection<KeyValuePair<int, int>> pairs,
            IReadOnlyList<Tuple<double, double>> firstCoords,
            IReadOnlyList<Tuple<double, double>> secondCoords,
            double maxError)
        {
            var score = 0;
            foreach (var pair in pairs)
            {
                var first = Vector.Build.DenseOfEnumerable(new []{firstCoords[pair.Key].Item1, firstCoords[pair.Key].Item2, 1});
                first = transform.Multiply(first);
                first = Vector.Build.DenseOfEnumerable(new[] {first[0], first[1]});
                var second = Vector.Build.DenseOfEnumerable(new[] { secondCoords[pair.Value].Item1, secondCoords[pair.Value].Item2});
                if (Distance.Euclidean(first, second) <= maxError)
                {
                    score++;
                }
            }

            return score;
        }

        private Dictionary<int, int> GetBestPairsForTransform(
            DenseMatrix transform,
            Dictionary<int, int> pairs,
            IReadOnlyList<Tuple<double, double>> firstCoords,
            IReadOnlyList<Tuple<double, double>> secondCoords,
            double maxError)
        {
            var dict = new Dictionary<int, int>();
            foreach (var pair in pairs)
            {
                var first = Vector.Build.DenseOfEnumerable(new[] { firstCoords[pair.Key].Item1, firstCoords[pair.Key].Item2, 1 });
                first = transform.Multiply(first);
                first = Vector.Build.DenseOfEnumerable(new[] { first[0], first[1] });
                var second = Vector.Build.DenseOfEnumerable(new[] { secondCoords[pair.Value].Item1, secondCoords[pair.Value].Item2 });
                if (Distance.Euclidean(first, second) <= maxError)
                {
                    dict.Add(pair.Key, pair.Value);
                }
            }

            return dict;
        }
    }
}
