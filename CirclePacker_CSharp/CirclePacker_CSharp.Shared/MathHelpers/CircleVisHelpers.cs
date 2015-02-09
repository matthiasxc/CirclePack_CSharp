using CirclePacker_CSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;

namespace CirclePacker_CSharp.MathHelpers
{
    public class CircleVisHelper
    {
        public double BaseValue { get; set; }
        public double BaseWidth { get; set; }
        public int RenderWidth { get; set; }
        public int RenderHeight { get; set;}
        private double baseArea {get; set;}
        public CircleVisHelper()
        {
            BaseValue = 100;
            BaseWidth = 100;
            setBaseArea();
        }

        public CircleVisHelper(double baseVal, double baseW)
        {
            BaseValue = baseVal;
            BaseWidth = baseW;
            setBaseArea();
        }

        void setBaseArea()
        {
            baseArea = Math.PI * ((BaseWidth / 2) * (BaseWidth / 2));
        }

        /// <summary>
        /// Using the base width and base value, calculate a single circle width
        ///   based on a value
        /// </summary>
        public double GetCircleRadius(double value)
        {
            double targetArea = value / BaseValue * baseArea;
            return Math.Sqrt(targetArea/Math.PI);
        }

        /// <summary>
        /// Predefine the circles based on a simply list of values
        /// </summary>
        public List<Circle> GetCircles(List<double> values)
        {
            List<Circle> returnCircles = new List<Circle>();
            Random r = new Random();
            foreach (double val in values)
            {
                var targetRadius = GetCircleRadius(val);
                Circle c = new Circle(r.Next(RenderWidth), r.Next(RenderHeight), (float)targetRadius);
                c.circleColor = Color.FromArgb(128, (byte)r.Next(255), 128, 200);
                c.SetCanvasDimensions(RenderWidth, RenderHeight);
                returnCircles.Add(c);
            }
            return returnCircles;
        }
    }
}
