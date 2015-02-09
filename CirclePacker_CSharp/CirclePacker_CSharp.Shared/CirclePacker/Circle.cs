using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;

namespace CirclePacker_CSharp
{
    public class Circle
    {
        public float x, y, radius;
        public Color circleColor;
        public float Width = 100.0f;
        public float Height = 100.0f;
        public string CircleName;

        public Circle(float x, float y, float radius)
        {
            this.x = x;
            this.y = y;
            this.radius = radius;
            this.circleColor = Color.FromArgb(255, 156, 156, 156);
        }

        public bool Contains(float x, float y)
        {
            float dx = this.x - x;
            float dy = this.y - y;
            return Math.Sqrt(dx * dx + dy * dy) <= radius;
        }

        public float DistanceToCenter
        {
            get
            {
                float dx = x - Width / 2;
                float dy = y - Height / 2;
                return (float)Math.Sqrt(dx * dx + dy * dy);
            }
        }

        public bool Intersects(Circle c)
        {
            float dx = c.x - x;
            float dy = c.y - y;
            float d = (float)Math.Sqrt(dx * dx + dy * dy);
            return d < radius || d < c.radius;
        }

        public void SetCanvasDimensions(double width, double height)
        {
            Width = (float)width;
            Height = (float)height;
        }
    }
}
