using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Collections.Generic;
using System.Linq;

namespace ArtificialIntelligence_4
{
    public class KeyPointPairsAlgorithm
    {
        private static DenseMatrix CalculateDistances(IReadOnlyList<DenseVector> firstPicture, IReadOnlyList<DenseVector> secondPicture)
        {
            var result = new DenseMatrix(firstPicture.Count, secondPicture.Count);
            for (var i = 0; i < firstPicture.Count; i++)
            {
                for (var j = 0; j < secondPicture.Count; j++)
                {
                    result[i, j] = Distance.Euclidean(firstPicture[i], secondPicture[j]);
                }
            }

            return result;
        }

        private static int GetPairedPoint(int rowNumber, DenseMatrix distances)
        {
            var row = distances.Row(rowNumber);
            var col = row.MinimumIndex();
            return distances.Column(col).MinimumIndex() == rowNumber ? col : -1;
        }

        public Dictionary<int, int> GetPairIndices(IReadOnlyList<DenseVector> firstPicture, IReadOnlyList<DenseVector> secondPicture)
        {
            var distances = CalculateDistances(firstPicture, secondPicture);
            var result = new Dictionary<int, int>();

            for (var i = 0; i < distances.RowCount; i++)
            {
                result.Add(i, GetPairedPoint(i, distances));
            }

            return result
                .Where(kvp => kvp.Value != -1)
                .ToDictionary(k => k.Key, v => v.Value);
        }
    }
}
