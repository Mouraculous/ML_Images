using System;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;

namespace ArtificialIntelligence_4
{
    public class NeighborhoodCoherenceAnalysis
    {
        private readonly int _pointsInNeighborhood;
        private readonly double _cohesionThreshold;

        public NeighborhoodCoherenceAnalysis(int pointsInNeighborhood, double cohesionThreshold)
        {
            _pointsInNeighborhood = pointsInNeighborhood;
            _cohesionThreshold = cohesionThreshold;
        }

        private static DenseMatrix CalculateDistances(List<Tuple<double, double>> coords, ICollection<int> indices)
        {
            var result = new DenseMatrix(coords.Count, coords.Count);
            var coords2 = new Tuple<double, double>[coords.Count];
            coords.CopyTo(coords2);
            for (var i = 0; i < coords.Count; i++)
            {
                for (var j = 0; j < coords.Count; j++)
                {
                    result[i, j] = indices.Contains(i) && indices.Contains(j) 
                        ? Math.Sqrt((coords[i].Item1 - coords2[j].Item1) * (coords[i].Item1 - coords2[j].Item1) +
                                  (coords[i].Item2 - coords2[j].Item2) * (coords[i].Item2 - coords2[j].Item2)) 
                        : double.MaxValue;
                }
            }

            return result;
        }

        public Dictionary<int, int> FindCoherent(List<Tuple<double, double>> firstCoords, List<Tuple<double, double>> secondCoords, Dictionary<int,int> pairs)
        {
            var coherent = new Dictionary<int, int>();
            var firstDistances = CalculateDistances(firstCoords, pairs.Keys.ToList());
            var secondDistances = CalculateDistances(secondCoords, pairs.Values.ToList());

            foreach (var pair in pairs)
            {
                if (IsCoherent(pair, firstDistances, secondDistances, pairs))
                {
                    coherent[pair.Key] = pair.Value;
                }
            }
            
            return coherent;
        }

        private bool IsCoherent(KeyValuePair<int, int> pair, DenseMatrix firstDistances, DenseMatrix secondDistances, IReadOnlyDictionary<int, int> pairs)
        {
            var firstRow = firstDistances.Row(pair.Key);
            var secondRow = secondDistances.Row(pair.Value);

            CreateNeighborhood(firstRow, secondRow, out var firstNeighboringPoints, out var secondNeighboringPoints);

            var count = firstNeighboringPoints.Count(c => secondNeighboringPoints.Contains(pairs[c]));

            return (double)count / _pointsInNeighborhood >= _cohesionThreshold;
        }

        private void CreateNeighborhood(
            Vector<double> firstRow, 
            Vector<double> secondRow,
            out List<int> firstNeighboringPoints,
            out List<int> secondNeighboringPoints)
        {
            firstNeighboringPoints = new List<int>();
            secondNeighboringPoints = new List<int>();

            for (var i = 0; i < _pointsInNeighborhood; i++)
            {
                var firstMinIndex = firstRow.MinimumIndex();
                var secondMinIndex = secondRow.MinimumIndex();

                firstNeighboringPoints.Add(firstMinIndex);
                secondNeighboringPoints.Add(secondMinIndex);

                firstRow[firstMinIndex] = double.MaxValue;
                secondRow[secondMinIndex] = double.MaxValue;
            }
        }
    }
}
