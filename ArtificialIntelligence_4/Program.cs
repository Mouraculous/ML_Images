using System;

namespace ArtificialIntelligence_4
{
    class Program
    {
        public const string FirstSiftPath = @"..\..\..\rsc\2.png.haraff.sift";
        public const string SecondSiftPath = @"..\..\..\rsc\3.png.haraff.sift";
        public const string FirstFilePath = @"..\..\..\rsc\2.png";
        public const string SecondFilePath = @"..\..\..\rsc\3.png";

        private static void Main(string[] args)
        {
            var paint = new UnderdevelopedPaint();
            var loader = new Loader();
            var firstImage = loader.Load(FirstSiftPath, out var firstImageCoords);
            var secondImage = loader.Load(SecondSiftPath, out var secondImageCoords);
            var afiniteTransform = new AfiniteTransform();
            var perspectivicTransform = new PerspectivicTransform();
            var afinicRansac = new Ransac();

            Console.WriteLine("Loaded");

            var pairConnector = new KeyPointPairsAlgorithm();
            var pairs = pairConnector.GetPairIndices(firstImage, secondImage);

            Console.WriteLine("Paired");
            paint.Paint(FirstFilePath, SecondFilePath, firstImageCoords, secondImageCoords, pairs, "paired.jpg");

            var cohesion = new NeighborhoodCoherenceAnalysis(8, 0.5);
            pairs = cohesion.FindCoherent(firstImageCoords, secondImageCoords, pairs);

            Console.WriteLine("Coherent");
            paint.Paint(FirstFilePath, SecondFilePath, firstImageCoords, secondImageCoords, pairs, "coherent.jpg");
           
            //Console.WriteLine("Get three points indicies: ");
            //var distanceHeuristic = new DistanceHeuristic(720, pairs);
            //var hahahaahaha = distanceHeuristic.GetAffineIndicies(firstImageCoords, secondImageCoords);
            //foreach(var i in hahahaahaha) {
            //    Console.Write(i + " ");
            //}

            var matrix = afinicRansac.FindBestTransform(perspectivicTransform, pairs, firstImageCoords, secondImageCoords, 2000, 70, out pairs);
            Console.WriteLine("Selected after applying Transform");
            paint.Paint(FirstFilePath, SecondFilePath, firstImageCoords, secondImageCoords, pairs, "finalP.jpg");

            Console.Read();
        }
    }
}
