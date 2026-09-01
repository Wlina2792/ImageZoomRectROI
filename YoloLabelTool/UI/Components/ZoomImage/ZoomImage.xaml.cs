using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace Wlina.Components.ZoomImage;


public partial class ZoomImage : UserControl
{
    #region 依赖属性

    public static readonly DependencyProperty SourceProperty =
        DependencyProperty.Register(nameof(Source), typeof(ImageSource), typeof(ZoomImage),
            new PropertyMetadata(null, OnSourceChanged));

    public static readonly DependencyProperty ZoomRatioProperty =
        DependencyProperty.Register(nameof(ZoomRatio), typeof(double), typeof(ZoomImage),
            new PropertyMetadata(1.0, OnZoomRatioChanged));

    public static readonly DependencyProperty TranslateXProperty =
        DependencyProperty.Register(nameof(TranslateX), typeof(double), typeof(ZoomImage),
            new PropertyMetadata(0.0));

    public static readonly DependencyProperty TranslateYProperty =
        DependencyProperty.Register(nameof(TranslateY), typeof(double), typeof(ZoomImage),
            new PropertyMetadata(0.0));

    public static readonly DependencyProperty OffsetXProperty =
        DependencyProperty.Register(nameof(OffsetX), typeof(double), typeof(ZoomImage),
            new PropertyMetadata(0.0));

    public static readonly DependencyProperty OffsetYProperty =
        DependencyProperty.Register(nameof(OffsetY), typeof(double), typeof(ZoomImage),
            new PropertyMetadata(0.0));



    public ImageSource Source
    {
        get => (ImageSource)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public double ZoomRatio
    {
        get => (double)GetValue(ZoomRatioProperty);
        set => SetValue(ZoomRatioProperty, value);
    }

    public double TranslateX
    {
        get => (double)GetValue(TranslateXProperty);
        set => SetValue(TranslateXProperty, value);
    }

    public double TranslateY
    {
        get => (double)GetValue(TranslateYProperty);
        set => SetValue(TranslateYProperty, value);
    }

    public double OffsetX
    {
        get => (double)GetValue(OffsetXProperty);
        set => SetValue(OffsetXProperty, value);
    }

    public double OffsetY
    {
        get => (double)GetValue(OffsetYProperty);
        set => SetValue(OffsetYProperty, value);
    }



    #endregion

    #region 私有字段

    private double _translateXCurrent;
    private double _translateYCurrent;
    private Point _lastMousePos;
    private bool _isDragging;

    private const double MinZoom = 0.01;
    private const double MaxZoom = 50.0;

    #endregion

    public ZoomImage()
    {
        InitializeComponent();
        myimage.SizeChanged += Myimage_SizeChanged;
        SizeChanged += ZoomImage_SizeChanged;
    }

    #region 依赖属性回调

    private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (ZoomImage)d;
        control.myimage.Source = e.NewValue as ImageSource;
        control.Reset();
    }

    private static void OnZoomRatioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (ZoomImage)d;
        control.UpdateVisuals();
    }

    #endregion

    #region 视觉更新

    private void UpdateVisuals()
    {
        PART_ScaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, null);
        PART_ScaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, null);
        PART_TranslateTransform.BeginAnimation(TranslateTransform.XProperty, null);
        PART_TranslateTransform.BeginAnimation(TranslateTransform.YProperty, null);

        PART_ScaleTransform.ScaleX = ZoomRatio;
        PART_ScaleTransform.ScaleY = ZoomRatio;
        PART_TranslateTransform.X = _translateXCurrent;
        PART_TranslateTransform.Y = _translateYCurrent;

        SetCurrentValue(TranslateXProperty, _translateXCurrent);
        SetCurrentValue(TranslateYProperty, _translateYCurrent);

        UpdateOffsets();
    }

    private void UpdateOffsets()
    {
        if (myimage.ActualWidth <= 0 || myimage.ActualHeight <= 0) return;

        var rect = myimage.TransformToAncestor(ImageContainer)
            .TransformBounds(new Rect(0, 0, myimage.ActualWidth, myimage.ActualHeight));

        double containerWidth = ImageContainer.ActualWidth;
        double containerHeight = ImageContainer.ActualHeight;

        if (containerWidth <= 0 || containerHeight <= 0) return;

        SetCurrentValue(OffsetXProperty, rect.X / containerWidth);
        SetCurrentValue(OffsetYProperty, rect.Y / containerHeight);
    }

    #endregion

    #region 重置

    public void Reset()
    {
        _translateXCurrent = 0;
        _translateYCurrent = 0;
        SetCurrentValue(ZoomRatioProperty, 1.0);
        SetCurrentValue(TranslateXProperty, 0.0);
        SetCurrentValue(TranslateYProperty, 0.0);
        UpdateVisuals();
    }

    #endregion

    #region 缩放

    public void ZoomIn()
    {
        double newRatio = Math.Min(MaxZoom, ZoomRatio * 1.2);
        SetCurrentValue(ZoomRatioProperty, newRatio);
    }

    public void ZoomOut()
    {
        double newRatio = Math.Max(MinZoom, ZoomRatio / 1.2);
        SetCurrentValue(ZoomRatioProperty, newRatio);
    }

    #endregion

    #region 鼠标事件

    private void ImageContainer_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount == 2)
        {
            this.Reset();
        }
        _lastMousePos = e.GetPosition(ImageContainer);
        _isDragging = true;
        ImageContainer.CaptureMouse();
    }

    private void ImageContainer_MouseMove(object sender, MouseEventArgs e)
    {
        if (!_isDragging) return;

        Point currentPos = e.GetPosition(ImageContainer);
        double dx = currentPos.X - _lastMousePos.X;
        double dy = currentPos.Y - _lastMousePos.Y;

        _translateXCurrent += dx;
        _translateYCurrent += dy;
        _lastMousePos = currentPos;

        UpdateVisuals();
    }

    private void ImageContainer_MouseUp(object sender, MouseButtonEventArgs e)
    {
        _isDragging = false;
        ImageContainer.ReleaseMouseCapture();
    }

    private void ImageContainer_LostMouseCapture(object sender, MouseEventArgs e)
    {
        _isDragging = false;
    }

    private void ImageContainer_MouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (myimage.Source == null) return;

        Point mousePos = e.GetPosition(ImageContainer);

        Rect rect = myimage.TransformToAncestor(ImageContainer)
            .TransformBounds(new Rect(0, 0, myimage.ActualWidth, myimage.ActualHeight));

        if (mousePos.X < rect.X || mousePos.X > rect.X + rect.Width ||
            mousePos.Y < rect.Y || mousePos.Y > rect.Y + rect.Height)
        {
            if (e.Delta > 0) ZoomIn();
            else ZoomOut();
            return;
        }

        double percentX = (mousePos.X - rect.X) / rect.Width;
        double percentY = (mousePos.Y - rect.Y) / rect.Height;

        double rawRatio = e.Delta > 0 ? ZoomRatio * 1.2 : ZoomRatio / 1.2;
        double newRatio = Math.Max(MinZoom, Math.Min(MaxZoom, rawRatio));

        double wChange = Math.Abs(myimage.ActualWidth * newRatio - rect.Width);
        double hChange = Math.Abs(myimage.ActualHeight * newRatio - rect.Height);

        Point origin = myimage.RenderTransformOrigin;
        double dx = wChange * (percentX - origin.X);
        double dy = hChange * (percentY - origin.Y);

        _translateXCurrent += (e.Delta > 0 ? -dx : dx);
        _translateYCurrent += (e.Delta > 0 ? -dy : dy);

        SetCurrentValue(ZoomRatioProperty, newRatio);
        UpdateVisuals();
        e.Handled = true;
    }

    #endregion

    #region 尺寸变化监听

    private void Myimage_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        UpdateOffsets();
    }

    private void ZoomImage_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        UpdateOffsets();
    }

    #endregion

    #region 坐标转换
    /// <summary>
    /// 父容器坐标转图片归一化坐标
    /// </summary>
    /// <param name="containerPoint"></param>
    /// <returns></returns>
    public Point GetImagePoint(Point containerPoint)
    {
        if (myimage.Source == null) return new Point();

        Rect rect = myimage.TransformToAncestor(this)
            .TransformBounds(new Rect(0, 0, myimage.ActualWidth, myimage.ActualHeight));

        double normalizedX = (containerPoint.X - rect.X) / rect.Width;
        double normalizedY = (containerPoint.Y - rect.Y) / rect.Height;

        return new Point(normalizedX, normalizedY);
    }
    /// <summary>
    /// 图片归一化坐标转父容器坐标
    /// </summary>
    /// <param name="normalizedPoint"></param>
    /// <returns></returns>
    public Point? GetContainerPoint(Point normalizedPoint)
    {
        if (myimage.Source == null) return null;

        Rect rect = myimage.TransformToAncestor(this)
            .TransformBounds(new Rect(0, 0, myimage.ActualWidth, myimage.ActualHeight));

        double containerX = rect.X + normalizedPoint.X * rect.Width;
        double containerY = rect.Y + normalizedPoint.Y * rect.Height;

        return new Point(containerX, containerY);
    }

    #endregion

    #region 手动传递滚动事件
    public void ForwardMouseWheel(MouseWheelEventArgs e)
    {
        ImageContainer_MouseWheel(ImageContainer, e);
    }
    #endregion
}