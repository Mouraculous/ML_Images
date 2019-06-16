using System;
using System.Collections.Generic;
using System.Drawing;

namespace ArtificialIntelligence_4
{
    public class UnderdevelopedPaint
    {
        private static readonly Random Random = new Random();

        public void Paint(
            string firstPicture, 
            string secondPicture,
            List<Tuple<double, double>> firstCoords,
            List<Tuple<double, double>> secondCoords,
            Dictionary<int, int> toDraw,
            string name = "xd.jpg")
        {
            var firstImage = Image.FromFile(firstPicture);
            var secondImage = Image.FromFile(secondPicture);

            var width = firstImage.Width + secondImage.Width;
            var height = Math.Max(firstImage.Height, secondImage.Height);

            var merged = new Bitmap(width, height);
            var g = Graphics.FromImage(merged);

            g.Clear(Color.Black);
            g.DrawImage(firstImage, new Rectangle(0, 0, firstImage.Width, firstImage.Height));
            g.DrawImage(secondImage, new Rectangle(firstImage.Width, 0, secondImage.Width, secondImage.Height));
            foreach (var pair in toDraw)
            {
                var pen = new Pen(GetRandomColor());
                g.DrawLine(pen, (float) firstCoords[pair.Key].Item1, (float) firstCoords[pair.Key].Item2,
                    (float)(firstImage.Width + secondCoords[pair.Value].Item1), (float) secondCoords[pair.Value].Item2);
            }

            merged.Save(name);
            firstImage.Dispose();
            secondImage.Dispose();
            
        }

        private static Color GetRandomColor()
        {
            return Color.FromArgb(Random.Next(0, 255), Random.Next(0, 255), Random.Next(0, 255));
            // The error is here
        }
    }
}
