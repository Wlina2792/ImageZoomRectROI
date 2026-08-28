using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ImageZoomRectROI;


public partial class Roilabel:ObservableObject
{
    [ObservableProperty] private double p1x;
    [ObservableProperty] private double p1y;
    [ObservableProperty] private double p2x;
    [ObservableProperty] private double p2y;

}


public partial class MainWindowVM:ObservableObject
{
    [ObservableProperty] ObservableCollection<Roilabel> roilabels=new();
    
    public MainWindowVM() 
    {
         Roilabels.Add(new Roilabel() { P1x = 0.0625, P1y = 0.0625, P2x = 0.125, P2y = 0.125 });
        // Roilabels.Add(new Roilabel() { X = 25, Y = 0.25, H = 0.25, W = 0.3125 });
        //    Roilabels.Add(new Roilabel() { X = 100, Y = 100, H = 100, W = 100 });
        //   Roilabels.Add(new Roilabel() { X = 250, Y = 250, H = 150, W = 150 });
      //  Roilabels.Add(new Roilabel() {P1x = 76, P1y = 76, P2x = 152, P2y = 152 });
      //  Roilabels.Add(new Roilabel() { P1x = 228, P1y = 228, P2x = 380, P2y = 380 });
    }

}
