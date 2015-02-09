using CirclePacker_CSharp.MathHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CirclePacker_CSharp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        bool isRunning = false;
        CirclePacker cp;
        public MainPage()
        {
            this.InitializeComponent();
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        void CompositionTarget_Rendering(object sender, object e)
        {
            if (isRunning && cp != null)
            {
                cp.Iterate(30);
                cp.Render();
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            isRunning = !isRunning;
            if (isRunning)
            {
                if (cp == null)
                {
                    // Assign the canvas we will be drawing on
                    cp = new CirclePacker(CircleCanvas);
                    // We're using random circles for now
                    //cp.GenerateSampleCircles(100, 10, 50);

                    // But we could take a list of values and use the 
                    //   CircleVisHelper class to generate a list of circles.
                    //   I wrote this to abstract the mathematics of visualizing
                    //   a value into the 2D area of a circle.
                    // To so this, we need to establish a baseline value and
                    //   a baseline width for our circles. Let's say we want a 
                    //   value of 100 to be represented by a 150 px circle.
                    //   We would use the following code.
                    var listOfValues = GenerateSampleData();
                    CircleVisHelper cvh = new CircleVisHelper(100, 150);
                    cvh.RenderHeight = Convert.ToInt32(CircleCanvas.ActualHeight);
                    cvh.RenderWidth = Convert.ToInt32(CircleCanvas.ActualWidth);
                    var sizedCircles = cvh.GetCircles(listOfValues);
                    cp.AllCircles = sizedCircles;

                    // Iterate the circle packing algorithm a couple times
                    cp.Iterate(100);
                    // And render into the canvas
                    // Technically, this is not the best method for rendering
                    //   because it isn't abstract... it's tied to a XAML rendering
                    //   (specifically a Canvas object). But you can override that
                    //   method if you're using a different UI framework
                    cp.Render();
                }
            }
        }

        private List<double> GenerateSampleData()
        {
            List<double> returnList = new List<double>();
            for (double i = 1; i <= 100.0; i++)
            {
                returnList.Add(i);
            }
            return returnList;
        }
    }
}
