using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;

namespace RectangleApp
{
    public class MyPoint
    {
        public int X { get; set; }
        public int Y { get; set; }

        public MyPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class MyRectangle
    {
        public Color Color { get; set; }
        public MyPoint BotLeft { get; set; }
        public MyPoint TopLeft { get; set; }
        public MyPoint BotRight { get; set; }
        public MyPoint TopRight { get; set; }

        public MyRectangle(Color color, MyPoint botLeft, MyPoint topLeft, MyPoint botRight, MyPoint topRight)
        {
            Color = color;
            BotLeft = botLeft;
            TopLeft = topLeft;
            BotRight = botRight;
            TopRight = topRight;
        }

        public MyRectangle DrawMainRectangle(
            List<MyRectangle> rectangles,
            MyRectangle mainRectangle,
            bool outsideRectanglesDisabled,
            bool ignoreColors,
            IList colors)
        {
            int minX = int.MaxValue, minY = int.MaxValue;
            int maxX = int.MinValue, maxY = int.MinValue;

            foreach (var rectangle in rectangles)
            {
                if(ignoreColors)
                {
                    if (colors.Contains(Helpers.ColorHelper.GetColorName(rectangle.Color.ToString()))) {
                        continue;
                    }
                }
                if (outsideRectanglesDisabled)
                {
                    if (mainRectangle.ContainsPoint(rectangle.BotLeft))
                    {
                        minX = Math.Min(minX, rectangle.BotLeft.X);
                        minY = Math.Min(minY, rectangle.BotLeft.Y);
                    }
                    if (mainRectangle.ContainsPoint(rectangle.TopLeft))
                    {
                        minX = Math.Min(minX, rectangle.TopLeft.X);
                        minY = Math.Min(minY, rectangle.TopLeft.Y);
                    }
                    if (mainRectangle.ContainsPoint(rectangle.BotRight))
                    {
                        maxX = Math.Max(maxX, rectangle.BotRight.X);
                        maxY = Math.Max(maxY, rectangle.BotRight.Y);
                    }
                    if (mainRectangle.ContainsPoint(rectangle.TopRight))
                    {
                        maxX = Math.Max(maxX, rectangle.TopRight.X);
                        maxY = Math.Max(maxY, rectangle.TopRight.Y);
                    }
                }
                else
                {
                    minX = Math.Min(minX, rectangle.BotLeft.X);
                    minY = Math.Min(minY, rectangle.BotLeft.Y);
                    maxX = Math.Max(maxX, rectangle.TopRight.X);
                    maxY = Math.Max(maxY, rectangle.TopRight.Y);
                }
            }

            MyPoint botLeft = new MyPoint(minX, minY);
            MyPoint topLeft = new MyPoint(minX, maxY);
            MyPoint botRight = new MyPoint(maxX, minY);
            MyPoint topRight = new MyPoint(maxX, maxY);

            mainRectangle = new MyRectangle(Colors.Blue, botLeft, topLeft, botRight, topRight);

            using (StreamWriter writer = new StreamWriter("log.txt", true))
            {
                writer.WriteLine($"Main rectangle corners: {mainRectangle.BotLeft.X}, {mainRectangle.BotLeft.Y} - {mainRectangle.TopRight.X}, {mainRectangle.TopRight.Y}");
            }
            Console.WriteLine($"Main rectangle corners: {mainRectangle.BotLeft.X}, {mainRectangle.BotLeft.Y} - {mainRectangle.TopRight.X}, {mainRectangle.TopRight.Y}");


            return mainRectangle;
        }
        public bool ContainsPoint(MyPoint point)
        {
            return BotRight.X <= point.X && point.X <= TopLeft.X && BotLeft.Y <= point.Y && point.Y <= TopRight.Y;
        }
    }
}
