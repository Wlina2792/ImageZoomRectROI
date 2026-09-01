using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Wlina.Components.RectRoiShape;

public class RectRoiControl : Control
{
    #region 依赖属性

    public static readonly DependencyProperty P1XProperty =
        DependencyProperty.Register(
            nameof(P1X), typeof(double), typeof(RectRoiControl),
            new FrameworkPropertyMetadata(0.0,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnCoordinateChanged));

    public static readonly DependencyProperty P1YProperty =
        DependencyProperty.Register(
            nameof(P1Y), typeof(double), typeof(RectRoiControl),
            new FrameworkPropertyMetadata(0.0,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnCoordinateChanged));

    public static readonly DependencyProperty P2XProperty =
        DependencyProperty.Register(
            nameof(P2X), typeof(double), typeof(RectRoiControl),
            new FrameworkPropertyMetadata(0.0,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnCoordinateChanged));

    public static readonly DependencyProperty P2YProperty =
        DependencyProperty.Register(
            nameof(P2Y), typeof(double), typeof(RectRoiControl),
            new FrameworkPropertyMetadata(0.0,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnCoordinateChanged));
    public static readonly DependencyProperty RoiFillProperty =
       DependencyProperty.Register(nameof(RoiFill), typeof(Brush), typeof(RectRoiControl),
           new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#770000FF"))));

    public static readonly DependencyProperty ThumbColorProperty =
        DependencyProperty.Register(nameof(ThumbColor), typeof(Brush), typeof(RectRoiControl),
               new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0000"))));

    #endregion

    #region 属性包装

    public double P1X
    {
        get => (double)GetValue(P1XProperty);
        set => SetValue(P1XProperty, value);
    }

    public double P1Y
    {
        get => (double)GetValue(P1YProperty);
        set => SetValue(P1YProperty, value);
    }

    public double P2X
    {
        get => (double)GetValue(P2XProperty);
        set => SetValue(P2XProperty, value);
    }

    public double P2Y
    {
        get => (double)GetValue(P2YProperty);
        set => SetValue(P2YProperty, value);
    }
    public Brush RoiFill
    {
        get => (Brush)GetValue(RoiFillProperty);
        set => SetValue(RoiFillProperty, value);
    }
    public Brush ThumbColor
    {
        get=> (Brush)GetValue(ThumbColorProperty);
        set=> SetValue(ThumbColorProperty, value);
    }

    #endregion

    #region 内部引用

    private Thumb? _moveTracker;
    private Thumb? _leftTopTracker;
    private Thumb? _rightTopTracker;
    private Thumb? _leftBottomTracker;
    private Thumb? _rightBottomTracker;

    #endregion

    #region 模板应用

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        // 解除旧事件
        if (_moveTracker != null)
            _moveTracker.DragDelta -= OnMoveDragDelta;
        if (_leftTopTracker != null)
            _leftTopTracker.DragDelta -= OnLeftTopDragDelta;
        if (_rightTopTracker != null)
            _rightTopTracker.DragDelta -= OnRightTopDragDelta;
        if (_leftBottomTracker != null)
            _leftBottomTracker.DragDelta -= OnLeftBottomDragDelta;
        if (_rightBottomTracker != null)
            _rightBottomTracker.DragDelta -= OnRightBottomDragDelta;
        

        // 获取模板部件
        _moveTracker = GetTemplateChild("MoveTracker") as Thumb;
        _leftTopTracker = GetTemplateChild("LeftTopTracker") as Thumb;
        _rightTopTracker = GetTemplateChild("RightTopTracker") as Thumb;
        _leftBottomTracker = GetTemplateChild("LeftBottomTracker") as Thumb;
        _rightBottomTracker = GetTemplateChild("RightBottomTracker") as Thumb;

        // 绑定新事件
        if (_moveTracker != null)
            _moveTracker.DragDelta += OnMoveDragDelta;
        if (_leftTopTracker != null)
            _leftTopTracker.DragDelta += OnLeftTopDragDelta;
        if (_rightTopTracker != null)
            _rightTopTracker.DragDelta += OnRightTopDragDelta;
        if (_leftBottomTracker != null)
            _leftBottomTracker.DragDelta += OnLeftBottomDragDelta;
        if (_rightBottomTracker != null)
            _rightBottomTracker.DragDelta += OnRightBottomDragDelta;
      
       
        // 初始化 MoveTracker 的位置和尺寸
        SyncMoveTracker();
    }

    #endregion

    #region 坐标变更 → 同步 MoveTracker

    private static void OnCoordinateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is RectRoiControl control)
            control.SyncMoveTracker();
    }

    /// <summary>
    /// 将 P1X/P1Y/P2X/P2Y 同步到 MoveTracker 的 Canvas 定位和尺寸
    /// </summary>
    private void SyncMoveTracker()
    {
        if (_moveTracker == null) return;

        double w = P2X - P1X;
        double h = P2Y - P1Y;

        if (w < 0) w = 0;
        if (h < 0) h = 0;

        Canvas.SetLeft(_moveTracker, P1X);
        Canvas.SetTop(_moveTracker, P1Y);
        _moveTracker.Width = w;
        _moveTracker.Height = h;
    }

    #endregion

    #region 拖拽处理：整体移动

    private void OnMoveDragDelta(object sender, DragDeltaEventArgs e)
    {
        double dx = e.HorizontalChange;
        double dy = e.VerticalChange;

        P1X += dx;
        P1Y += dy;
        P2X += dx;
        P2Y += dy;
    }

    #endregion

    #region 拖拽处理：四角缩放

    private void OnLeftTopDragDelta(object sender, DragDeltaEventArgs e)
    {
        P1X += e.HorizontalChange;
        P1Y += e.VerticalChange;
        P1X=Math.Min(P1X, P2X);
        P1Y=Math.Min(P1Y, P2Y);

    }

    private void OnRightTopDragDelta(object sender, DragDeltaEventArgs e)
    {
        P2X += e.HorizontalChange;
        P1Y += e.VerticalChange;

        P2X = Math.Max(P1X, P2X);
        P1Y = Math.Min(P1Y, P2Y);
    }

    private void OnLeftBottomDragDelta(object sender, DragDeltaEventArgs e)
    {
        P1X += e.HorizontalChange;
        P2Y += e.VerticalChange;
        P1X = Math.Min(P1X, P2X);
        P2Y = Math.Max(P1Y, P2Y);
    }

    private void OnRightBottomDragDelta(object sender, DragDeltaEventArgs e)
    {
        P2X += e.HorizontalChange;
        P2Y += e.VerticalChange;
        P2X = Math.Max(P1X, P2X);
        P2Y = Math.Max(P1Y, P2Y);
    }

    #endregion
   

}
