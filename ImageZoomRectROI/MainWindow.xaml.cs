using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageZoomRectROI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private CrossAdorner _crossAdorner;
    public MainWindow()
    {
        InitializeComponent();
        this.DataContext = new MainWindowVM();
      
    }
    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);
        // Window 模板已应用，AdornerDecorator 已存在
        var layer = AdornerLayer.GetAdornerLayer(AdornerGrid);
        if (layer != null)
        {
            _crossAdorner = new CrossAdorner(AdornerGrid);
            layer.Add(_crossAdorner);
        }
    }
    private void AdornerGrid_MouseMove(object sender, MouseEventArgs e)
    {
        if (_crossAdorner != null)
        {
            Point adornerGridpos = e.GetPosition(AdornerGrid);
            _crossAdorner.UpdatePosition(adornerGridpos);



            AdornerGrid_TextBlock.Text = $"AdornerGrid位置{adornerGridpos.X:F1} :  {adornerGridpos.Y:F1}";

            Point myZoomImagepos = myZoomImage.GetImagePoint(adornerGridpos);

            myZoomImage_TextBlock.Text= $"myZoomImagepos位置{myZoomImagepos.X:F5} :  {myZoomImagepos.Y:F5}";

            //Debug.WriteLine($"{myZoomImage.GetContainerPoint(new Point(0, 0))}");
            //Point? nullablePoint = roiedit.GetImagePoint(pos);
            //if (nullablePoint.HasValue)
            //{
            //    text_pointX.Text = "图片坐标X" + pos.X.ToString();
            //    text_pointY.Text = "图片坐标Y" + pos.Y.ToString();
            //}
            //else
            //{
            //    text_pointX.Text = null; text_pointY.Text = null;
            //}

            //Canvas.SetLeft(mythumb, roiedit.GetContainerPoint(new Point(0.37, 0.345))?.X ?? 0);
            //Canvas.SetTop(mythumb, roiedit.GetContainerPoint(new Point(0.37, 0.345))?.Y ?? 0);

        }
    }


//将roi框的滚动传递到imagezoom
    private void AdornerGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {

        myZoomImage.ForwardMouseWheel(e);
        e.Handled = true;

    }
}