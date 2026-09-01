using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;
using YoloLabelTool.LabelModel.Models;
using YoloLabelTool.LabelModel.Services;

namespace YoloLabelTool.UI.Converts;

public class LabelNameConvert : IMultiValueConverter
{
    private static bool IsUnavailable(object value)
    {
        return value == null

            || value == DependencyProperty.UnsetValue
            || value == Binding.DoNothing;
    }

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values == null || values.Length < 2 ||
            IsUnavailable(values[0]) || IsUnavailable(values[1]))
        {
            return "未知类别";
        }

        int id = System.Convert.ToInt32(values[0]);   // 比 (int) 强转更稳妥

        if (!(values[1] is ObservableCollection<LabelTypeMode> labelTypes))
        {
            return "未知类别";
        }

        var imageLabel = labelTypes.FirstOrDefault(e => e.TypeId == id);
        if (imageLabel == null)
        {
            return $"非包含ID-{id}";
        }
        else if (imageLabel.TypeName.IsWhiteSpace())
        {
            return $"未命名ID-{id}";
        }
        else
        {
            return imageLabel.TypeName;
        }
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
