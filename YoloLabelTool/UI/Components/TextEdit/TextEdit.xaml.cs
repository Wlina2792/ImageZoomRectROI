using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Wlina.Components.TextEdit;

public class TextEdit : Control
{

    public TextEdit()
    {
        MouseLeftButtonDown += TextEditable_MouseLeftButtonDown;
        LostFocus += TextEditable_LostFocus;
    }

    private void TextEditable_LostFocus(object sender, RoutedEventArgs e)
    {
        isSelected = false;
       // Background = Brushes.Transparent;
        box.Visibility = Visibility.Collapsed;
        block.Visibility = Visibility.Visible;
    }

    bool isSelected = false;
    private void TextEditable_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (!isSelected)
        {
            isSelected = true;
            block.Visibility = Visibility.Collapsed;  // 先显示，再聚焦
            box.Visibility = Visibility.Visible;
            box.Focus();
            box.SelectAll();
        }
        e.Handled = true;  // 关键：阻止事件冒泡到 ListBoxItem，防止选中和抢焦点
    }

    static TextEdit()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(TextEdit), new FrameworkPropertyMetadata(typeof(TextEdit)));
    }

    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(TextEdit), new PropertyMetadata(string.Empty, OnTextChanged));

    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {

    }

    TextBlock block = null;
    TextBox box = null;
    public override void OnApplyTemplate()
    {
        block = GetTemplateChild("PART_TextBlock") as TextBlock;
        box = GetTemplateChild("PART_TextBox") as TextBox;
        block.LostFocus += TextEditable_LostFocus;
        box.LostFocus += TextEditable_LostFocus;
    }

}
