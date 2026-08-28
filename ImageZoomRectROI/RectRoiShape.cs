using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ImageZoomRectROI;

/// <summary>
/// 不旋转的矩形ROI图形，支持自定义位置、尺寸、描边、填充
/// </summary>
public class RectRoiShape : Shape
{
    #region 依赖属性

    // 左上角X坐标
    public static readonly DependencyProperty P1XProperty =
        DependencyProperty.Register(
            nameof(P1X),
            typeof(double),
            typeof(RectRoiShape),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

    // 左上角Y坐标
    public static readonly DependencyProperty P1YProperty =
        DependencyProperty.Register(
            nameof(P1Y),
            typeof(double),
            typeof(RectRoiShape),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

    // 右下角X坐标
    public static readonly DependencyProperty P2XProperty =
        DependencyProperty.Register(
            nameof(P2X),
            typeof(double),
            typeof(RectRoiShape),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

    // 右下角Y坐标
    public static readonly DependencyProperty P2YProperty =
        DependencyProperty.Register(
            nameof(P2Y),
            typeof(double),
            typeof(RectRoiShape),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

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

    #endregion

    /// <summary>
    /// 实现Shape的抽象属性，定义矩形的几何描述
    /// </summary>
    protected override Geometry DefiningGeometry
    {
        get
        {
            if (P1X >= P2X || P1Y >= P2Y)
                return Geometry.Empty;

            return new RectangleGeometry(new Rect(P1X, P1Y, P2X-P1X, P2Y-P1Y));
        }
    }

}