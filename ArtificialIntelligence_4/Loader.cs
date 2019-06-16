using System;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ArtificialIntelligence_4
{
    public class Loader
    {
        private const char SplitSymbol = ' ';

        public List<DenseVector> Load(string path, out List<Tuple<double, double>> coordinates)
        {
            var lines = File.ReadAllLines(path);
            coordinates = lines.Skip(2).Select(ExtractCoords).ToList();
            return lines.Skip(2).Select(ExtractPoint).ToList();
        }

        private Tuple<double, double> ExtractCoords(string line)
        {
            var lineSplitted = line.Split(SplitSymbol);
            return new Tuple<double, double>(
                double.Parse(lineSplitted[0], new NumberFormatInfo {NumberDecimalSeparator = "."}),
                double.Parse(lineSplitted[1], new NumberFormatInfo {NumberDecimalSeparator = "."}));
        }

        private static DenseVector ExtractPoint(string line)
        {
            var variables = line.Split(SplitSymbol).Skip(5).ToArray();
            var newPoint = new DenseVector(variables.Length);

            for (var i = 0; i < variables.Length; i++)
            {
                var variable = variables[i];
                newPoint[i] = double.Parse(variable, new NumberFormatInfo {NumberDecimalSeparator = "."});
            }

            return newPoint;
        }
    }
    
}
