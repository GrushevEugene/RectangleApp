using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Point = System.Windows.Point;

namespace RectangleApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<MyRectangle> rectangles = new List<MyRectangle>();
        MyRectangle mainRectangle = new MyRectangle(Colors.Black, new MyPoint(0, 0), new MyPoint(0, 0), new MyPoint(0, 0), new MyPoint(0, 0));
        public MainWindow()
        {
            InitializeComponent();
            colors.ItemsSource = Helpers.ColorHelper.GetColors();

            //TODO:
            rectangles =
                [
                    new MyRectangle(Colors.Red, new MyPoint(20, 10), new MyPoint(20, 60), new MyPoint(50, 10), new MyPoint(50, 60)),
                    new MyRectangle(Colors.Green, new MyPoint(30, 30), new MyPoint(30, 80), new MyPoint(80, 30), new MyPoint(80, 80)),
                    new MyRectangle(Colors.Yellow, new MyPoint(40, 40), new MyPoint(40, 100), new MyPoint(60, 40), new MyPoint(60, 100)),
                ];

            foreach (var rectangle in rectangles)
            {
                DrawRectangles(rectangle);
            }
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void DrawRectangles(MyRectangle myRectangle)
        {
            CanvasBorder.BorderThickness = new Thickness(1);

            var partiallyTransparentSolidColorBrush = new SolidColorBrush(myRectangle.Color);
            partiallyTransparentSolidColorBrush.Opacity = 0.25;

            var polygon = new Polygon
            {
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                Fill = partiallyTransparentSolidColorBrush,
                Points =
                {
                    new Point(myRectangle.BotLeft.X, myRectangle.BotLeft.Y),
                    new Point(myRectangle.TopLeft.X, myRectangle.TopLeft.Y),
                    new Point(myRectangle.TopRight.X, myRectangle.TopRight.Y),
                    new Point(myRectangle.BotRight.X, myRectangle.BotRight.Y),
                }
            };
            canvas.Children.Add(polygon);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            try
            {
                foreach (var rectangle in rectangles)
                {
                    DrawRectangles(rectangle);
                }
                mainRectangle = mainRectangle.DrawMainRectangle(rectangles, mainRectangle, outsideRectanglesDisabled.IsChecked.Value, ignoreColors.IsChecked.Value, colors.SelectedItems);
                DrawRectangles(mainRectangle);
            }
            catch (FormatException)
            {
                MessageBox.Show("Unable to write, wrong coordinates wormat.");
            }
        }

        private bool ValidateRectangle(MyRectangle myRectangle)
        {
            if (canvas.ActualHeight < myRectangle.TopLeft.Y || canvas.ActualWidth < myRectangle.TopLeft.X ||
                canvas.ActualHeight < myRectangle.TopRight.Y || canvas.ActualWidth < myRectangle.TopRight.X)
            {
                MessageBox.Show("Unable to write, rectangle more than canvas.");
                return false;
            }

            if (myRectangle.BotRight.X != myRectangle.TopRight.X ||
                myRectangle.BotRight.Y != myRectangle.BotLeft.Y ||
                myRectangle.BotLeft.X != myRectangle.TopLeft.X ||
                myRectangle.TopRight.Y != myRectangle.TopLeft.Y
                )
            {
                MessageBox.Show("Unable to write, wrong rectangle.");
                return false;
            }

            return true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                mainRectangle = new MyRectangle
                    (
                        Colors.Blue,
                        new MyPoint(Convert.ToInt32(BotLeftX.Text), Convert.ToInt32(BotLeftY.Text)),
                        new MyPoint(Convert.ToInt32(TopLeftX.Text), Convert.ToInt32(TopLeftY.Text)),
                        new MyPoint(Convert.ToInt32(BotRightX.Text), Convert.ToInt32(BotRightY.Text)),
                        new MyPoint(Convert.ToInt32(TopRightX.Text), Convert.ToInt32(TopRightY.Text))
                    );
                if (ValidateRectangle(mainRectangle))
                {
                    DrawRectangles(mainRectangle);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Unable to write, wrong coordinates wormat.");
            }
        }
    }
}