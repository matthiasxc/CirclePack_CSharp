using CirclePacker_CSharp.MathHelpers;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace CirclePacker_CSharp
{
    public class CirclePacker
    {
        public List<Circle> AllCircles { get; set; }
        long Iterations { get; set; }

        Canvas HostCanvas { get; set; }

        public CirclePacker()
        {

        }

        public CirclePacker(Canvas hostCanvas)
        {
            HostCanvas = hostCanvas;

        }
        public void GenerateSampleCircles(int circleCount, int circleMin, int circleMax)
        {
            if (HostCanvas != null)
            {
                AllCircles = CreateRandomCircles(circleCount, circleMin, circleMax, (int)HostCanvas.ActualWidth, (int)HostCanvas.ActualHeight);
            }
        }

        List<Circle> CreateRandomCircles(int circleCount, int circleMin, int circleMax, int canvasWidth, int canvasHeight)
        {
            List<Circle> result = new List<Circle>();
            Random r = new Random();
            for (int i = 0; i < circleCount; i++)
            {
                Circle c = new Circle(r.Next(canvasWidth), r.Next(canvasHeight), r.Next(circleMin, circleMax));
                c.circleColor = Color.FromArgb(128, (byte)r.Next(255), 128, 200);
                c.SetCanvasDimensions(HostCanvas.ActualWidth, HostCanvas.ActualHeight);
                result.Add(c);
            }

            return result;
        }

        public void Iterate(int iterationCount)
        {
            var sortedCircles = from c in AllCircles
                                orderby c.DistanceToCenter descending
                                select c;

            var sCircles = sortedCircles.ToList();
            Vector2 v = new Vector2();

            // Cycle through circles for collision detection
            foreach (Circle c1 in sCircles)
            {
                foreach (Circle c2 in sCircles)
                {
                    if (c1 != c2)
                    {
                        float dx = c2.x - c1.x;
                        float dy = c2.y - c1.y;
                        float r = c1.radius + c2.radius;
                        float d = (dx * dx) + (dy * dy);
                        if (d < (r * r) - 0.01)
                        {
                            v = new Vector2(dx, dy);
                            v.Normalize();
                            v = v * (float)((r - Math.Sqrt(d)) * .5);

                            c2.x += v.X;
                            c2.y += v.Y;
                            c1.x -= v.X;
                            c1.y -= v.Y;
                        }
                    }
                }

            }

            // Contract all circles into the center 
            float dampening = 0.1f / (float)iterationCount;
            foreach (Circle c in sCircles)
            {
                v = new Vector2(c.x - (float)(HostCanvas.ActualWidth / 2), c.y - (float)(HostCanvas.ActualHeight / 2));
                v *= dampening;
                c.x -= v.X;
                c.y -= v.Y;
            }
        }

        public void Render()
        {
            for (int i = 0; i < AllCircles.Count; i++)
            {
                Circle c = AllCircles[i];
                // Just in case there are some NaN values out there
                if (c.x.ToString() != "NaN" && c.y.ToString() != "NaN")
                {
                    if (i < HostCanvas.Children.Count)
                    {
                        var e = ((Ellipse)HostCanvas.Children[i]);
                        e.Width = e.Height = (c.radius * 2) - 2;
                        e.Fill = new SolidColorBrush(c.circleColor);
                        Canvas.SetLeft(e, c.x - c.radius);
                        Canvas.SetTop(e, c.y - c.radius);
                    }
                    else
                    {
                        Ellipse e = new Ellipse();
                        e.Width = e.Height = (c.radius * 2) - 2;
                        e.Fill = new SolidColorBrush(c.circleColor);
                        Canvas.SetLeft(e, c.x - c.radius);
                        Canvas.SetTop(e, c.y - c.radius);
                        HostCanvas.Children.Add(e);
                    }
                }
            }

            // in case the number of circles have dropped
            for (int i = AllCircles.Count; i < HostCanvas.Children.Count; i++)
            {
                HostCanvas.Children.RemoveAt(AllCircles.Count);
            }
        }
    }
}
