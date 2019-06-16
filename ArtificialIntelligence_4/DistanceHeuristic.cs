using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ArtificialIntelligence_4
{
    public class DistanceHeuristic
    {

        private readonly double _smallParam;
        private readonly double _largeParam;
        private const int A_AMOUNT = 3;
        private const int H_AMOUNT = 4;
        private readonly Dictionary<int, int> _pairs;
        private static readonly Random Random = new Random();


        public DistanceHeuristic(double pictureHeight, Dictionary<int, int> pairs)
        {
            _smallParam = Math.Pow(pictureHeight / 100, 2);
            _largeParam = Math.Pow(pictureHeight / 30, 2);
            _pairs = pairs;
        }

        public List<int> GetAffineIndicies(List<Tuple<double, double>> firstImageCoords, List<Tuple<double, double>> secondImageCoords)
        {
            return GetIndicies(A_AMOUNT, firstImageCoords, secondImageCoords);
        }

        public List<int> GetPerspectiveIndicies(List<Tuple<double, double>> firstImageCoords, List<Tuple<double, double>> secondImageCoords)
        {
            return GetIndicies(H_AMOUNT, firstImageCoords, secondImageCoords);

        }

        private List<int> GetIndicies(int pointsNumber, List<Tuple<double, double>> firstImageCoords, List<Tuple<double, double>> secondImageCoords)
        {

            var randomPoint = 0;
            do {
                randomPoint = Random.Next(0, firstImageCoords.Count);
            } while(!_pairs.ContainsKey(randomPoint)); 

            
            var firstImageChosenPoints = new List<int>
            {
                randomPoint
            };

            var secondImageChosenPoints = new List<int>
            {
                _pairs.GetValueOrDefault(randomPoint)
            };

            for (var i = 0; i < firstImageCoords.Count && firstImageChosenPoints.Count < pointsNumber; i++)
            {
                var tmp = _pairs.GetValueOrDefault(i, -1);
                if (randomPoint != i && tmp != -1 && CheckChosenPointsDistance(firstImageChosenPoints, 0, firstImageCoords, firstImageCoords.ElementAt(i)) &&
                   CheckChosenPointsDistance(secondImageChosenPoints, 0, secondImageCoords, secondImageCoords.ElementAt(tmp)))
                {
                    firstImageChosenPoints.Add(i);
                    secondImageChosenPoints.Add(tmp);
                }
            }

            while(firstImageChosenPoints.Count < pointsNumber)
            {
                do
                {
                    randomPoint = Random.Next(0, firstImageCoords.Count);
                } while (!_pairs.ContainsKey(randomPoint) && firstImageChosenPoints.Contains(randomPoint));
                firstImageChosenPoints.Add(randomPoint);
            }

            return firstImageChosenPoints;
        }

        private bool CheckChosenPointsDistance(List<int> chosenPoints, int index, List<Tuple<double, double>> coordinates, Tuple<double, double> pairToCheck)
        {        
            var coordinates2 = new Tuple<double, double>[coordinates.Count + 1];
            coordinates.CopyTo(coordinates2);
            coordinates2[coordinates2.Length- 1] = pairToCheck;

            for (var i = index; i < chosenPoints.Count - 1; i++)
            {
                if(!CheckDistance(coordinates2.ElementAt(chosenPoints.ElementAt(index)), coordinates2.ElementAt(chosenPoints.ElementAt(i + 1))))
                {
                    return false;
                }
            }

            return ++index == chosenPoints.Count || CheckChosenPointsDistance(chosenPoints, index, coordinates, pairToCheck);
        }

        private bool CheckDistance(Tuple<double, double> firstPoint, Tuple<double, double> secondPoint)
        {
            var distance = Math.Pow(firstPoint.Item1 - secondPoint.Item1, 2) + Math.Pow(firstPoint.Item2 - secondPoint.Item2, 2);
            return Math.Pow(_smallParam, 2) < distance && Math.Pow(_largeParam, 2) > distance;
        }
    }
}
