using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using YoloLabelTool.LabelModel.Services;
using YoloLabelTool.UI.ViewModels.Windows;
using YoloLabelTool.UI.Views.Windows;

namespace YoloLabelTool;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public new static App Current => (App)Application.Current;
    public IServiceProvider ServiceProvider { get; } = ConfigureServices();

    private static ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection()
            .AddSingleton<MainWindow>()
            .AddSingleton<MainWindowViewModel>()
            .AddSingleton<LabelTypeManagementService>()
            .AddSingleton<ImageLabelManagementService>()
            ;
           
        return services.BuildServiceProvider();

    }


    protected override async void OnStartup(StartupEventArgs e)
    {

        base.OnStartup(e);

        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();

        mainWindow.Show();

    }

}
