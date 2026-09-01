using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace YoloLabelTool.LabelModel.Models;

public partial class ImageLabelModel:ObservableObject
{
    [ObservableProperty] private string _imagePath;

    [ObservableProperty] private ObservableCollection<Label> labels=new();

    public partial class Label: ObservableObject
    {
        [ObservableProperty] private int typeId;
        [ObservableProperty] private double nx1;
        [ObservableProperty] private double ny1;
        [ObservableProperty] private double nx2;
        [ObservableProperty] private double ny2;
        public Label(int Id, double nx1, double ny1, double nx2, double ny2)
        {
            TypeId = Id; Nx1 = nx1; Ny1 = ny1;Nx2=nx2; Ny2=ny2;
        }

    }
   

}
