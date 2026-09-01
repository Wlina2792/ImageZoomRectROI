using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using YoloLabelTool.LabelModel.Models;

namespace YoloLabelTool.LabelModel.Services;

public partial class LabelTypeManagementService : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<LabelTypeMode> labelTypes = new();

    [ObservableProperty]
    private int lastId = -1;

    public void AddLabelType(string name = "新类别")
    {
        LabelTypes.Add(new LabelTypeMode() { TypeId = ++LastId, TypeName = name });
        OnPropertyChanged(nameof(LabelTypes));
    }

    public void RemoveLabelType(LabelTypeMode labelTypeMode)
    {
        this.LabelTypes.Remove(labelTypeMode);
        OnPropertyChanged(nameof(LabelTypes));
    }





}