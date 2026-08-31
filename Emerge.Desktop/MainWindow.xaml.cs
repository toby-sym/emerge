using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.UI;
using Emerge.Core.Simulation;
using Emerge.Rendering;

namespace Emerge.Desktop;

public sealed partial class MainWindow : Window
{
    public static Simulation SimulationInstance { get; private set; } = null!;
    public static SimulationLoop SimulationLoop { get; private set; } = null!;
    public static SimulationConfig CurrentConfig { get; set; }

    public MainWindow()
    {
        this.InitializeComponent();

        this.ExtendsContentIntoTitleBar = true;
        this.SetTitleBar(AppTitleBar);

        CurrentConfig = new SimulationConfig
        {
            Seed = (int)DateTime.Now.Ticks,
            WorldWidth = 800,
            WorldHeight = 600,
            InitialPopulation = 100
        };

        SimulationInstance = new Simulation(CurrentConfig);
        SimulationLoop = new SimulationLoop(SimulationInstance, 50);
        SimulationLoop.Start();
        NavView.SelectedItem = SimNavItm;
    }

    private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.IsSettingsSelected) return;

        var selectedItem = (NavigationViewItem)args.SelectedItem;
        string? tag = selectedItem?.Tag?.ToString();

        Type pageType = tag switch
        {
            "Simulation" => typeof(SimulationPage),
            "Analytics" => typeof(AnalyticsPage),
            "Genetics" => typeof(GeneticsPage),
            _ => typeof(SimulationPage)
        };

        if (ContentFrame.CurrentSourcePageType != pageType)
        {
            ContentFrame.Navigate(pageType);
        }
    }

    public static void ResetSimulation(SimulationConfig newConfig)
    {
        SimulationLoop?.Stop();

        CurrentConfig = newConfig;
        SimulationInstance = new Simulation(CurrentConfig);
        SimulationLoop = new SimulationLoop(SimulationInstance, 50);
        SimulationLoop.Start();
    }

    public static void StartSimulationLoop() => SimulationLoop?.Start();

    public static void PauseSimulationLoop() => SimulationLoop?.Stop();
}