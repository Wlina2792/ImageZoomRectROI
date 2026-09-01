using Material.Icons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;
using YoloLabelTool.LabelModel.Models;

namespace YoloLabelTool.UI.Converts;

public class FindTypeIdConvert: IMultiValueConverter
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
            return DependencyProperty.UnsetValue;
        }

        int id = System.Convert.ToInt32(values[0]);   // 比 (int) 强转更稳妥

        if (!(values[1] is ObservableCollection<LabelTypeMode> labelTypes))
        {
            throw new ArgumentException();
        }
        bool isfind = labelTypes.Any(e => e.TypeId == id);

        if (isfind)
        {
            return MaterialIconKind.AspectRatio;
        }
        else
        {
            return MaterialIconKind.Warning;
        }

           
        
    }

    object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
