using Material.Icons;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Wlina.Components.WCard;

public class WCard : ContentControl
{
    static WCard()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(WCard), new FrameworkPropertyMetadata(typeof(WCard)));
    }

    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(WCard), new PropertyMetadata(string.Empty));

    public int TextSize
    {
        get { return (int)GetValue(TextSizeProperty); }
        set { SetValue(TextSizeProperty, value); }
    }
    public static readonly DependencyProperty TextSizeProperty =
        DependencyProperty.Register(nameof(TextSize), typeof(int), typeof(WCard), new PropertyMetadata(15));

    public Brush TextColor
    {
        get { return (Brush)GetValue(TextColorProperty); }
        set { SetValue(TextColorProperty, value); }
    }
    public static readonly DependencyProperty TextColorProperty =
        DependencyProperty.Register(nameof(TextColor), typeof(Brush), typeof(WCard), new PropertyMetadata( new SolidColorBrush( Colors.Black)));

    public MaterialIconKind Icon
    {
        get { return (MaterialIconKind)GetValue(IconProperty); }
        set { SetValue(IconProperty, value); }
    }
    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(nameof(Icon), typeof(MaterialIconKind), typeof(WCard), new PropertyMetadata(default(MaterialIconKind)));

    public int IconSize
    {
        get { return (int)GetValue(IconSizeProperty); }
        set { SetValue(IconSizeProperty, value); }
    }
    public static readonly DependencyProperty IconSizeProperty =
        DependencyProperty.Register(nameof(IconSize), typeof(int), typeof(WCard), new PropertyMetadata(18));

    public Brush IconColor
    {
        get { return (Brush)GetValue(IconColorProperty); }
        set { SetValue(IconColorProperty, value); }
    }
    public static readonly DependencyProperty IconColorProperty =
        DependencyProperty.Register(nameof(IconColor), typeof(Brush), typeof(WCard), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

    public UIElement HeadControl
    {
        get { return (UIElement)GetValue(HeadControlProperty); }
        set { SetValue(HeadControlProperty, value); }
    }
    public static readonly DependencyProperty HeadControlProperty =
        DependencyProperty.Register(nameof(HeadControl), typeof(UIElement), typeof(WCard), new PropertyMetadata(null));
}