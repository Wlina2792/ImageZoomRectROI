using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using YoloLabelTool.UI.Adorners;
using YoloLabelTool.UI.ViewModels.Windows;

namespace YoloLabelTool.UI.Views.Windows;

/// <summary>
/// MainWindow.xaml 的交互逻辑
/// </summary>
public partial class MainWindow : Window
{
    private MainWindowViewModel _vm;
    private CrossAdorner _crossAdorner;
    public MainWindow(MainWindowViewModel mainWindowViewModel)
    {
        InitializeComponent();
        _vm = mainWindowViewModel;
        this.DataContext = _vm;
    }
    #region 十字坐标装饰器
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
        }
    }
    #endregion

    #region 滚动事件传递
    private void AdornerGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {

        myZoomImage.ForwardMouseWheel(e);
        e.Handled = true;

    }
    #endregion

    #region 绘制新区
    private void Border_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (myZoomImage.Source == null)
        {
            return;
        }
        var pn = myZoomImage.GetImagePoint(e.GetPosition(myZoomImage));
        Debug.WriteLine($"开始点{pn}");
        this._vm.DrawingPointX1_n = pn.X;
        this._vm.DrawingPointY1_n = pn.Y;
    }

    private void Border_MouseMove(object sender, MouseEventArgs e)
    {
        if (myZoomImage.Source == null||this._vm.DrawingPointX1_n == null||this._vm.DrawingPointY1_n==null)
        {
            return;

        }
        var pn = myZoomImage.GetImagePoint(e.GetPosition(myZoomImage));
        this._vm.DrawingPointX2_n = pn.X;
        this._vm.DrawingPointY2_n = pn.Y;
    }

    private void Border_MouseUp(object sender, MouseButtonEventArgs e)
    {
        this._vm.AddImageLabel();
    }
    #endregion
}
