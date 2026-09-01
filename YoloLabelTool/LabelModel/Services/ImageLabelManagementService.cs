using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using YoloLabelTool.LabelModel.Models;

namespace YoloLabelTool.LabelModel.Services;

public partial class ImageLabelManagementService:ObservableObject
{
    [ObservableProperty] private ObservableCollection<ImageLabelModel> imageLabels=new();

    public void AddImage(string Path)=>this.ImageLabels.Add(new ImageLabelModel() { ImagePath = Path });

    public void RemoveImage(ImageLabelModel imageLabelModel)=>this.ImageLabels.Remove(imageLabelModel);

    public void AddImageLabel(ImageLabelModel imageLabelModel, ImageLabelModel.Label label) => imageLabelModel.Labels.Add(label);

    public void RemoveLabel(ImageLabelModel imageLabelModel,ImageLabelModel.Label label)=>imageLabelModel.Labels.Remove(label);

    public void LoadLabelsFromFile(ImageLabelModel image)
    {
        if (image == null) return;

        string txtPath = Path.ChangeExtension(image.ImagePath, ".txt");
        if (File.Exists(txtPath))
        {
            image.Labels.Clear();
            var lines = File.ReadAllLines(txtPath);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Trim().Split(' ');
                if (parts.Length >= 5)
                {
                    int id = int.Parse(parts[0]);
                    double cx_n = double.Parse(parts[1]);
                    double cy_n = double.Parse(parts[2]);
                    double w_n = double.Parse(parts[3]);
                    double h_n = double.Parse(parts[4]);

                    image.Labels.Add(new ImageLabelModel.Label(
                        id,
                        cx_n - w_n / 2.0,
                        cy_n - h_n / 2.0,
                        cx_n + w_n / 2.0,
                        cy_n + h_n / 2.0
                    ));
                }
            }
        }
        else
        {
            image.Labels.Clear();
        }
    }

    public void SaveLabelsToFile(ImageLabelModel image)
    {
        if (image == null) return;

        string txtPath = Path.ChangeExtension(image.ImagePath, ".txt");
        using (StreamWriter sw = new StreamWriter(txtPath))
        {
            foreach (var label in image.Labels)
            {
                double cx = (label.Nx1 + label.Nx2) / 2.0;
                double cy = (label.Ny1 + label.Ny2) / 2.0;
                double w = label.Nx2 - label.Nx1;
                double h = label.Ny2 - label.Ny1;
                sw.WriteLine($"{label.TypeId} {cx} {cy} {w} {h}");
            }
        }
    }

}
