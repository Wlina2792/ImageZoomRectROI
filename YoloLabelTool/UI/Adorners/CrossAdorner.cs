using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Documents;



namespace YoloLabelTool.UI.Adorners;

public class CrossAdorner : Adorner
{
    private Point _mousePosition;

    // 构造函数必须接收一个 UIElement（通常是 Window 或顶层容器），并传给基类
    public CrossAdorner(UIElement adornedElement) : base(adornedElement)
    {
        // 让鼠标事件穿透，不影响下层控件
        this.IsHitTestVisible = false;
    }

    // 更新鼠标位置并触发重绘
    public void UpdatePosition(Point position)
    {
        _mousePosition = position;
        InvalidateVisual(); // 强制调用 OnRender
    }


    // 核心：绘制十字线
    protected override void OnRender(DrawingContext drawingContext)
    {
        // 获取被装饰元素（通常是窗口）的宽高
        double width = AdornedElement.RenderSize.Width;
        double height = AdornedElement.RenderSize.Height;

        // 如果窗口还没加载完成，直接返回
        if (width == 0 || height == 0) return;
        // 格式：#RRGGBB 或 #AARRGGBB
        var color = (Color)ColorConverter.ConvertFromString("#FF00FF");
        Pen pen = new Pen(new SolidColorBrush(color), 1);
        pen.Freeze(); // 性能优化

        // 竖线 (从顶部画到底部)
        drawingContext.DrawLine(pen,
            new Point(_mousePosition.X, 0),
            new Point(_mousePosition.X, height));

        // 横线 (从左画到右)
        drawingContext.DrawLine(pen,
            new Point(0, _mousePosition.Y),
            new Point(width, _mousePosition.Y));
    }



}
