using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;

namespace ArtificialIntelligence_4
{
    public interface ITransform
    {
        DenseMatrix GetTransformMatrix(
            List<KeyValuePair<int, int>> pairs,
            List<Tuple<double, double>> firstCoords,
            List<Tuple<double, double>> secondCoords);

        int PointsNeeded { get; }
    }
}
