using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using YoloLabelTool.LabelModel.Models;

namespace YoloLabelTool.UI.Views.Dialogs;

public partial class ChooseLabelTypeDialog : Window, INotifyPropertyChanged
{
    public ChooseLabelTypeDialog(ObservableCollection<LabelTypeMode> labelTypeModes)
    {
        InitializeComponent();
        LabelTypeModes = labelTypeModes;
        this.DataContext = this;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public  ObservableCollection<LabelTypeMode> LabelTypeModes {  get;  set; }

    private LabelTypeMode? _selectedLabeiType;
    public LabelTypeMode? SelectedLLabelTypeMode
    {
        get => _selectedLabeiType;
        set
        {
            // 取消旧对象的订阅
            if (_selectedLabeiType != null)
                _selectedLabeiType.PropertyChanged -= OnSelectedLabelTypePropertyChanged;

            _selectedLabeiType = value;

            // 订阅新对象的属性变化
            if (_selectedLabeiType != null)
                _selectedLabeiType.PropertyChanged += OnSelectedLabelTypePropertyChanged;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedLLabelTypeMode)));
        }
    }

    private void OnSelectedLabelTypePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs($"{nameof(SelectedLLabelTypeMode)}.{e.PropertyName}"));
    }

    private void OK_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedLLabelTypeMode == null)
        {
            MessageBox.Show($"未选择标签"); 
            return;
        }
        this.DialogResult = true;
    }

    private void NG_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = false;
    }
}