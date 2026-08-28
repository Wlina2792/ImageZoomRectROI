using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ImageZoomRectROI
{
    public class point_convert : IMultiValueConverter
    {
        private ZoomImage _zoomImage;
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // values[0] = ZoomImage 控件对象本身
            // values[1] = OffsetX  (触发器)
            // values[2] = OffsetY  (触发器)
            // values[3] = ZoomRatio(触发器)
            // values[4] = VM 归一化坐标值

            if (values[0] is ZoomImage zoomImage && values[4] is double normalizedValue)
            {
                _zoomImage = zoomImage;
                if(parameter?.ToString() == "X")
                {
                    var imagePoint = _zoomImage.GetContainerPoint(new Point( normalizedValue, 0));
                    return imagePoint.X;
                }
                else if (parameter?.ToString() == "Y")
                {
                    var imagePoint = _zoomImage.GetContainerPoint(new Point(0, normalizedValue));
                    return imagePoint.Y;
                }
                else
                {
                    throw new ArgumentException();
                }

               
            }
            return 0.0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (value is double containerValue)
            {
                double imagePoint;
                // _zoomImage 在 Convert 中已被赋值
                if ( parameter?.ToString() == "X")
                {
                    imagePoint = _zoomImage.GetImagePoint(new Point(containerValue,0)).X;
                }
                else if( parameter?.ToString() == "Y")
                {
                    imagePoint = _zoomImage.GetImagePoint(new Point(0,containerValue)).Y;
                }
                else
                {
                    throw new InvalidOperationException();
                }


                // 返回 5 个元素，与 MultiBinding 的绑定顺序一一对应
                // 触发器绑定返回 DoNothing，仅归一化坐标绑定接收回写值
                return new object[]
                {
                    Binding.DoNothing,  // values[0] ZoomImage 对象
                    Binding.DoNothing,  // values[1] OffsetX
                    Binding.DoNothing,  // values[2] OffsetY
                    Binding.DoNothing,  // values[3] ZoomRatio
                    imagePoint          // values[4] 归一化坐标 → 写回 VM
                };
            }
            return new object[]
            {
                Binding.DoNothing,
                Binding.DoNothing,
                Binding.DoNothing,
                Binding.DoNothing,
                Binding.DoNothing
            };
        }

      
    }
}