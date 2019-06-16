using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ArtificialIntelligence_4
{
    class PerspectivicTransform : ITransform
    {
        public int PointsNeeded => 4;

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
                new[] {firstCoords[pairs[0].Key].Item1, firstCoords[pairs[0].Key].Item2, 1, 0, 0, 0, -firstCoords[pairs[0].Key].Item1*secondCoords[pairs[0].Value].Item1, -firstCoords[pairs[0].Key].Item2*secondCoords[pairs[0].Value].Item1},
                new[] {firstCoords[pairs[1].Key].Item1, firstCoords[pairs[1].Key].Item2, 1, 0, 0, 0, -firstCoords[pairs[1].Key].Item1*secondCoords[pairs[1].Value].Item1, -firstCoords[pairs[1].Key].Item2*secondCoords[pairs[1].Value].Item1},
                new[] {firstCoords[pairs[2].Key].Item1, firstCoords[pairs[2].Key].Item2, 1, 0, 0, 0, -firstCoords[pairs[2].Key].Item1*secondCoords[pairs[2].Value].Item1, -firstCoords[pairs[2].Key].Item2*secondCoords[pairs[2].Value].Item1},
                new[] {firstCoords[pairs[3].Key].Item1, firstCoords[pairs[3].Key].Item2, 1, 0, 0, 0, -firstCoords[pairs[3].Key].Item1*secondCoords[pairs[3].Value].Item1, -firstCoords[pairs[3].Key].Item2*secondCoords[pairs[3].Value].Item1},
                new[] {0, 0, 0, firstCoords[pairs[0].Key].Item1, firstCoords[pairs[0].Key].Item2, 1, -firstCoords[pairs[0].Key].Item1*secondCoords[pairs[0].Value].Item2, -firstCoords[pairs[0].Key].Item2*secondCoords[pairs[0].Value].Item2},
                new[] {0, 0, 0, firstCoords[pairs[1].Key].Item1, firstCoords[pairs[1].Key].Item2, 1, -firstCoords[pairs[1].Key].Item1*secondCoords[pairs[1].Value].Item2, -firstCoords[pairs[1].Key].Item2*secondCoords[pairs[1].Value].Item2},
                new[] {0, 0, 0, firstCoords[pairs[2].Key].Item1, firstCoords[pairs[2].Key].Item2, 1, -firstCoords[pairs[2].Key].Item1*secondCoords[pairs[2].Value].Item2, -firstCoords[pairs[2].Key].Item2*secondCoords[pairs[2].Value].Item2},
                new[] {0, 0, 0, firstCoords[pairs[3].Key].Item1, firstCoords[pairs[3].Key].Item2, 1, -firstCoords[pairs[3].Key].Item1 * secondCoords[pairs[3].Value].Item2, -firstCoords[pairs[3].Key].Item2 * secondCoords[pairs[3].Value].Item2 }
            };

            var matrix = Matrix.Build.DenseOfRowArrays(tempMatrix);

            matrix = matrix.Inverse();

            var paramsVector = matrix.Multiply(vector);

            var transformMatrix = new DenseMatrix(3, 3);

            for (var i = 0; i < 3; i++)
            {
                transformMatrix[0, i] = paramsVector[i];
                transformMatrix[1, i] = paramsVector[3 + i];
                transformMatrix[2, i] = i == 2 ? 1 : paramsVector[6 + i];
            }

            return transformMatrix;
        }
    }
}
