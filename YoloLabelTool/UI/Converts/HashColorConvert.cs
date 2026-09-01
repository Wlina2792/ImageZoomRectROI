using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace YoloLabelTool.UI.Converts;


public class HashColorConvert : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int hashCode)
        {

            // 色相均匀分布在 0~360 度
            double hue = ToAngle(hashCode);

            // 固定饱和度和亮度，保证颜色鲜艳且可辨识
            double saturation = 0.75;
            double lightness = 0.50;

            //基础颜色
            Color baseColor = HslToColor(hue, saturation, lightness);

            // 默认不透明
            byte alpha = 255;

            if (parameter != null)
            {
                double opacity;

                if (parameter is double d)
                {
                    opacity = d;
                }
                else if (double.TryParse(parameter.ToString(), NumberStyles.Float,CultureInfo.InvariantCulture, out opacity))
                {
                    int a = 1;
                    // 特性语法传入的字符串 "0.1" 走这里
                }
                else
                {
                    opacity = 1.0; // 解析失败，保持不透明
                }

                opacity = Math.Max(0.0, Math.Min(1.0, opacity));
                alpha = (byte)Math.Round(opacity * 255);
            }

            var c=  Color.FromArgb(alpha, baseColor.R, baseColor.G, baseColor.B);
            return new SolidColorBrush(c);

        }
        return new SolidColorBrush(Colors.Black);

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private static Color HslToColor(double h, double s, double l)
    {
        double c = (1 - Math.Abs(2 * l - 1)) * s;
        double hPrime = h / 60.0;
        double x = c * (1 - Math.Abs(hPrime % 2 - 1));
        double m = l - c / 2;

        double r1, g1, b1;

        if (hPrime < 1) { r1 = c; g1 = x; b1 = 0; }
        else if (hPrime < 2) { r1 = x; g1 = c; b1 = 0; }
        else if (hPrime < 3) { r1 = 0; g1 = c; b1 = x; }
        else if (hPrime < 4) { r1 = 0; g1 = x; b1 = c; }
        else if (hPrime < 5) { r1 = x; g1 = 0; b1 = c; }
        else { r1 = c; g1 = 0; b1 = x; }

        byte r = (byte)((r1 + m) * 255);
        byte g = (byte)((g1 + m) * 255);
        byte b = (byte)((b1 + m) * 255);

     //   Debug.WriteLine($"HSL: {h}, {s}, {l} -> RGB: {r}, {g}, {b}");
        return Color.FromRgb(r, g, b);

    }

    //位反转排列
    //数字转角度 1->0  2->180 3->90 4->270 5->45 6->135 7->225 ->8->315
    private static double ToAngle(int angle)
    {

        // 支持任意 2 的幂次长度：2, 4, 8, 16, 32, 64 ...
        int n = angle;

        // 计算位数：找到能覆盖 n 的最小 2 的幂次
        int bits = 0;
        int temp = n;
        while (temp > 0)
        {
            temp >>= 1;
            bits++;
        }
        // bits 至少为 1（当 n=0 时，即 angle=1）
        if (bits == 0) bits = 1;

        // 反转 bits 位二进制
        int reversed = 0;
        for (int i = 0; i < bits; i++)
        {
            reversed = (reversed << 1) | (n & 1);
            n >>= 1;
        }

        // 总份数 = 2^bits
        int total = 1 << bits;
        return reversed * (360.0 / total);

    }

}
