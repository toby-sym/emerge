using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Emerge.Desktop;

public sealed partial class AnalyticsPage : Page
{
    private readonly DispatcherTimer _timer;

    public AnalyticsPage()
    {
        this.InitializeComponent();

        _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
        _timer.Tick += (s, e) => UpdateMetrics();
        _timer.Start();

        UpdateMetrics();
    }

    private void UpdateMetrics()
    {
        var world = MainWindow.SimulationInstance?.World;
        if (world != null)
        {
            TxtPopulationCount.Text = world.Organisms?.Count.ToString() ?? "N/A";
            TxtTickCount.Text = MainWindow.SimulationInstance.TickCount.ToString(); 
            TxtSeedInfo.Text = MainWindow.CurrentConfig.Seed.ToString();
        }
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        _timer.Stop();
        base.OnNavigatedFrom(e);
    }
}