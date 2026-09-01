using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace YoloLabelTool.LabelModel.Models;

public partial class LabelTypeMode:ObservableObject
{
    [ObservableProperty] private int typeId=-1;
    [ObservableProperty] private string typeName="?类别";



}
