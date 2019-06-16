using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;

namespace ArtificialIntelligence_4
{
    public class AfiniteTransform : ITransform
    {
        public int PointsNeeded => 3;

        public DenseMatrix GetTransformMatrix(
            List<KeyValuePair<int, int>> pairs, 
            List<Tuple<double, double>> firstCoords,
            List<Tuple<double, double>> secondCoords)
        {
            var vector = new DenseVector(pairs.Count * 2);
            for (var i = 0; i < pairs.Count; i++)
            {
                vector[i] = secondCoords[pairs[i].Value].Item1;
                vector[i + pairs.Count] = secondCoords[pairs[i].Value].Item2;
            }

            var tempMatrix = new[]
            {
                new[] {firstCoords[pairs[0].Key].Item1, firstCoords[pairs[0].Key].Item2, 1, 0, 0, 0},
                new[] {firstCoords[pairs[1].Key].Item1, firstCoords[pairs[1].Key].Item2, 1, 0, 0, 0},
                new[] {firstCoords[pairs[2].Key].Item1, firstCoords[pairs[2].Key].Item2, 1, 0, 0, 0},
                new[] {0, 0, 0, firstCoords[pairs[0].Key].Item1, firstCoords[pairs[0].Key].Item2, 1},
                new[] {0, 0, 0, firstCoords[pairs[1].Key].Item1, firstCoords[pairs[1].Key].Item2, 1},
                new[] {0, 0, 0, firstCoords[pairs[2].Key].Item1, firstCoords[pairs[2].Key].Item2, 1}
            };

            var matrix = Matrix.Build.DenseOfRowArrays(tempMatrix);

            matrix = matrix.Inverse();

            var paramsVector = matrix.Multiply(vector);

            var transformMatrix = new DenseMatrix(pairs.Count, pairs.Count);

            for (var i = 0; i < pairs.Count; i++)
            {
                transformMatrix[0, i] = paramsVector[i];
                transformMatrix[1, i] = paramsVector[pairs.Count + i];
                transformMatrix[2, i] = i == 2 ? 1 : 0;
            }

            return transformMatrix;
        }
    }
}
