using Material.Icons;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Wlina.Components.IconButton;

public class IconButton : Button
{
    #region 构造函数

    static IconButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(IconButton), new FrameworkPropertyMetadata(typeof(IconButton)));
    }

    #endregion

    #region 依赖属性

    // 图标
    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(nameof(Icon), typeof(MaterialIconKind?),
            typeof(IconButton), new PropertyMetadata(null, OnIconChanged));

    // 图标颜色
    public static readonly DependencyProperty IconColorProperty =
        DependencyProperty.Register(nameof(IconColor), typeof(Brush),
            typeof(IconButton), new PropertyMetadata(new SolidColorBrush(Colors.Blue)));
        
    private static readonly DependencyPropertyKey HasIconKey =
        DependencyProperty.RegisterReadOnly(nameof(HasIcon), typeof(bool),
            typeof(IconButton), new PropertyMetadata(false));

    public static readonly DependencyProperty HasIconProperty = HasIconKey.DependencyProperty;

    // 图标大小
    public static readonly DependencyProperty IconSizeProperty =
        DependencyProperty.Register(nameof(IconSize), typeof(int), typeof(IconButton),
            new PropertyMetadata(16));

    // 按钮文字
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(nameof(Text), typeof(string),
            typeof(IconButton), new PropertyMetadata(string.Empty, OnTextChanged));

    // 文字颜色（默认画刷已冻结）
    public static readonly DependencyProperty TextColorProperty =
        DependencyProperty.Register(nameof(TextColor), typeof(Brush),
              typeof(IconButton), new PropertyMetadata(new SolidColorBrush(Colors.White)));

   
    private static readonly DependencyPropertyKey HasTextKey =
        DependencyProperty.RegisterReadOnly(nameof(HasText), typeof(bool),
            typeof(IconButton), new PropertyMetadata(false));

    public static readonly DependencyProperty HasTextProperty = HasTextKey.DependencyProperty;

    //边角圆度
    public static readonly DependencyProperty RadiusProperty =
    DependencyProperty.Register(nameof(Radius), typeof(CornerRadius),
        typeof(IconButton), new PropertyMetadata(new CornerRadius(0)));


    #endregion

    #region 包装器

    public MaterialIconKind? Icon
    {
        get => (MaterialIconKind?)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public Brush IconColor
    {
        get => (Brush)GetValue(IconColorProperty);
        set => SetValue(IconColorProperty, value);
    }

    public bool HasIcon
    {
        get => (bool)GetValue(HasIconProperty);
    }

    public int IconSize
    {
        get => (int)GetValue(IconSizeProperty);
        set => SetValue(IconSizeProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public Brush TextColor
    {
        get => (Brush)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    public bool HasText
    {
        get => (bool)GetValue(HasTextProperty);
    }


    public CornerRadius Radius
    {
        get => (CornerRadius)GetValue(RadiusProperty);
        set => SetValue(RadiusProperty, value);
    }
    #endregion

    #region 属性变更

    private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var button = (IconButton)d;
        // 【修复问题5】HasIcon 为只读 DP，只能通过 Key 在内部写入
        button.SetValue(HasIconKey, e.NewValue != null);
    }

    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var button = (IconButton)d;
        button.SetValue(HasTextKey, !string.IsNullOrEmpty(e.NewValue as string));
    }

    #endregion
}