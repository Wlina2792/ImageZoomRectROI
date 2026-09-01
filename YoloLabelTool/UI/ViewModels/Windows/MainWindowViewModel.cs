using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using YoloLabelTool.LabelModel.Models;
using YoloLabelTool.LabelModel.Services;
using YoloLabelTool.UI.Views.Dialogs;

namespace YoloLabelTool.UI.ViewModels.Windows;

public partial class MainWindowViewModel : ObservableObject, IDisposable
{
    
    [ObservableProperty] private LabelTypeManagementService labelTypeManagementService = null!;

    [ObservableProperty] private ImageLabelManagementService imageLabelManagementService = null!;

    [ObservableProperty] private ImageLabelModel? selectImage;

    [ObservableProperty] private ImageLabelModel.Label? selectedLabel;

    [ObservableProperty] private bool isDrawing;

    [ObservableProperty] private bool autoSave;

    [ObservableProperty] private double? drawingPointX1_n;

    [ObservableProperty] private double? drawingPointY1_n;

    [ObservableProperty] private double? drawingPointX2_n;

    [ObservableProperty] private double? drawingPointY2_n;

    public ObservableCollection<LabelTypeMode> LabelTypeItems
        => LabelTypeManagementService.LabelTypes;

    public MainWindowViewModel(
        LabelTypeManagementService labelTypeManagementService,
        ImageLabelManagementService imageLabelManagementService)
    {
        this.LabelTypeManagementService = labelTypeManagementService;
        this.imageLabelManagementService = imageLabelManagementService;

        SubscribeToLabelTypeChanges();
    }

    public void Dispose()
    {
        UnsubscribeFromLabelTypeChanges();
        GC.SuppressFinalize(this);
    }

    partial void OnSelectImageChanged(ImageLabelModel? oldValue, ImageLabelModel newValue)
    {
        // 保存旧图片（如果启用自动保存）
        if (oldValue != null && AutoSave)
        {
            ImageLabelManagementService.SaveLabelsToFile(oldValue);
        }

        // 加载新图片
        if (newValue != null)
        {
            SelectedLabel = null;
            ImageLabelManagementService.LoadLabelsFromFile(newValue);
        }
    }

    [RelayCommand]
    private void AddLabelType()
        => LabelTypeManagementService.AddLabelType();

    [RelayCommand]
    private void AddImage()
    {
        var dialog = new OpenFileDialog
        {
            Multiselect = true,
            Filter = "图片文件|*.bmp;*.jpg;*.jpeg;*.png;*.gif",
            Title = "选择图片文件"
        };

        if (dialog.ShowDialog() != true)
            return;

        foreach (var path in dialog.FileNames)
        {
            ImageLabelManagementService.AddImage(path);
        }

        SelectImage ??= ImageLabelManagementService.ImageLabels.FirstOrDefault();
    }

    [RelayCommand]
    private void RemoveImageLabel(ImageLabelModel.Label label)
        => ImageLabelManagementService.RemoveLabel(SelectImage, label);


    public void AddImageLabel()
    {
        if (DrawingPointX1_n == null || DrawingPointX2_n == null ||
            DrawingPointY1_n == null || DrawingPointY2_n == null)
        {
            return;
        }

        var dialog = new ChooseLabelTypeDialog(LabelTypeManagementService.LabelTypes);
        if (dialog.ShowDialog() != true)
            return;

        int id = dialog.SelectedLLabelTypeMode.TypeId;
        double nx1 = DrawingPointX1_n.Value;
        double ny1 = DrawingPointY1_n.Value;
        double nx2 = DrawingPointX2_n.Value;
        double ny2 = DrawingPointY2_n.Value;

        SelectImage?.Labels.Add(new ImageLabelModel.Label(id, nx1, ny1, nx2, ny2));

        // 清理绘制状态
        DrawingPointX1_n = null;
        DrawingPointY1_n = null;
        DrawingPointX2_n = null;
        DrawingPointY2_n = null;
        IsDrawing = false;
    }


    /// <summary>
    /// 订阅 LabelTypes 集合及其子对象的变化
    /// </summary>
    private void SubscribeToLabelTypeChanges()
    {
        var collection = LabelTypeManagementService.LabelTypes;
        collection.CollectionChanged += OnLabelTypesCollectionChanged;

        foreach (var item in collection)
        {
            item.PropertyChanged += OnLabelTypeModePropertyChanged;
        }
    }

    /// <summary>
    /// 取消订阅，防止内存泄漏
    /// </summary>
    private void UnsubscribeFromLabelTypeChanges()
    {
        var collection = LabelTypeManagementService.LabelTypes;
        collection.CollectionChanged -= OnLabelTypesCollectionChanged;

        foreach (var item in collection)
        {
            item.PropertyChanged -= OnLabelTypeModePropertyChanged;
        }
    }

    private void OnLabelTypesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        // 新增项 → 订阅 PropertyChanged
        if (e.NewItems != null)
        {
            foreach (LabelTypeMode item in e.NewItems)
            {
                item.PropertyChanged += OnLabelTypeModePropertyChanged;
            }
        }

        // 移除项 → 取消订阅（防止内存泄漏）
        if (e.OldItems != null)
        {
            foreach (LabelTypeMode item in e.OldItems)
            {
                item.PropertyChanged -= OnLabelTypeModePropertyChanged;
            }
        }

        // 集合内容发生变化，强制通知 UI 刷新 LabelTypeItems
        OnPropertyChanged(nameof(LabelTypeItems));
    }

    private void OnLabelTypeModePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        // 仅当 TypeName 变化时才触发刷新
        if (e.PropertyName == nameof(LabelTypeMode.TypeName))
        {
            OnPropertyChanged(nameof(LabelTypeItems));
        }
    }
}