using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using wpf_Drawing.Models;

namespace wpf_Drawing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CursorModel cursorModel = new();
        ElementCounterModel elementCounterModel = new();
        public double x_position;
        public double y_position; 
        public SolidColorBrush element_color;
        Polyline polyLine;
        Shape element = null;
        Point polyStart;

        public MainWindow()
        {
            InitializeComponent();
            lbl_x.DataContext = cursorModel;
            lbl_y.DataContext = cursorModel;
            lbl_count.DataContext = elementCounterModel;
            // default at start:
            cursorModel.Action = "Rectangle";
            element_color = new SolidColorBrush(Colors.Red);

        }

        private void cnv_MouseMove(object sender, MouseEventArgs e)
        {
            cursorModel.X = Convert.ToString(e.GetPosition(cnv).X);
            cursorModel.Y = Convert.ToString(e.GetPosition(cnv).Y);

            if (e.LeftButton == MouseButtonState.Pressed && cursorModel.Action == "Free Hand")
            {
                Point currentPoint = e.GetPosition(cnv);
                if (polyStart != currentPoint)
                {
                    polyLine.Points.Add(currentPoint);
                }
            }
        }

        private void cnv_Loaded(object sender, RoutedEventArgs e)
        {
            lbl_width.Content = cnv.ActualWidth;
            lbl_height.Content = cnv.ActualHeight;
        }


        private void cnv_MouseDown(object sender, MouseButtonEventArgs e)
        {
            x_position = Convert.ToDouble(e.GetPosition(cnv).X);
            y_position = Convert.ToDouble(e.GetPosition(cnv).Y);

            switch (cursorModel.Action)
            {
                case "Rectangle":
                    element = new Rectangle();
                    break;
                case "Circle":
                    element = new Ellipse();
                    break;
                case "Free Hand":
                    polyStart = e.GetPosition(cnv);
                    polyLine = new Polyline
                    {
                        Stroke = new SolidColorBrush(Colors.Black),
                        StrokeThickness = 4.0
                    };
                    cnv.Children.Add(polyLine);
                    break;
                case "Delete":
                    for (int i = cnv.Children.Count - 1; i >= 0; i--)
                    {
                        if (cnv.Children[i] == e.OriginalSource) { cnv.Children.RemoveAt(i);}
                    }  
                    break;
                default:
                    break;
            }

            if (element != null) // applicable only for Rectangle and Circle. Free hand is different
            {
                element.Stroke = new SolidColorBrush(Colors.Black);
                element.StrokeThickness = 3.0;
                element.Width = 30;
                element.Height = 30;
                element.Fill = element_color;
                Canvas.SetTop(element, y_position);
                Canvas.SetLeft(element, x_position);
                cnv.Children.Add(element);
                element = null;
            }
        }

        private void Element_Click (object sender, RoutedEventArgs e)
        {
            switch (sender.GetType().GetProperty("Content").GetValue(sender))
            {
                case "Rectangle":
                    cursorModel.Action = "Rectangle";
                    break;
                case "Circle":
                    cursorModel.Action = "Circle";
                    break;
                case "Free Hand":
                    cursorModel.Action = "Free Hand";
                    break;
                default:
                    break;
            }
        }

        private void Color_Click(object sender, RoutedEventArgs e)
        {
            switch (sender.GetType().GetProperty("Content").GetValue(sender))
            {
                case "Red":
                    element_color = new SolidColorBrush(Colors.Red);
                    break;
                case "Green":
                    element_color = new SolidColorBrush(Colors.Green);
                    break;
                case "Blue":
                    element_color = new SolidColorBrush(Colors.Blue);
                    break;
                default:
                    break;
            }
        }

        private void Action_Click(object sender, RoutedEventArgs e)
        {
            switch (sender.GetType().GetProperty("Content").GetValue(sender))
            {
                case "Delete":
                    cursorModel.Action = "Delete";
                    break;
                case "Delete All":
                    cnv.Children.Clear();
                    break;
                case "Save As":
                    SaveFileDialog saveFileDialog = new()
                    {
                        Filter = "PNG Image|*.png",
                        Title = "Save an PNG File"
                    };
                    saveFileDialog.ShowDialog();
                    if (saveFileDialog.FileName != "")
                    {
                        RenderTargetBitmap rtb = new(400, 400, 96d, 96d, System.Windows.Media.PixelFormats.Default);
                        rtb.Render(cnv);
                        BitmapEncoder pngEncoder = new PngBitmapEncoder();
                        pngEncoder.Frames.Add(BitmapFrame.Create(rtb));
                        using var fs = System.IO.File.OpenWrite(saveFileDialog.FileName);
                        pngEncoder.Save(fs);
                    }
                    break;
                default:
                    break;
            }
        }

        private void StackPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void cnv_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Pen;
        }
    }
}
