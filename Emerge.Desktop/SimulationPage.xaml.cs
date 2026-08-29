using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.UI;
using Emerge.Core.Simulation;
using Emerge.Rendering;

namespace Emerge.Desktop;

public sealed partial class SimulationPage : Page
{
    private bool _isRunning = true;

    public SimulationPage()
    {
        this.InitializeComponent();
        TxtSeed.Text = MainWindow.CurrentConfig.Seed.ToString();
        NumPopulation.Value = MainWindow.CurrentConfig.InitialPopulation;
    }

    private void Canvas_CreateResources(CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
    {
    }

    private void Canvas_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
    {
        if (_isRunning)
        {
            MainWindow.SimulationInstance.Tick();
        }

        var session = args.DrawingSession;
        session.Clear(Color.FromArgb(255, 18, 18, 18));

        foreach (var cmd in WorldRenderer.Render(MainWindow.SimulationInstance.World))
        {
            session.FillCircle(
                (float)cmd.X,
                (float)cmd.Y,
                (float)cmd.Radius,
                Color.FromArgb(255, cmd.R, cmd.G, cmd.B)
            );
        }
    }

    private void BtnStart_Click(object sender, RoutedEventArgs e) => _isRunning = true;
    private void BtnPause_Click(object sender, RoutedEventArgs e) => _isRunning = false;

    private void BtnReset_Click(object sender, RoutedEventArgs e)
    {
        int seed = int.TryParse(TxtSeed.Text, out var parsedSeed) ? parsedSeed : (int)System.DateTime.Now.Ticks;
        int population = (int)NumPopulation.Value;

        var newConfig = new SimulationConfig
        {
            Seed = seed,
            WorldWidth = 800,
            WorldHeight = 600,
            InitialPopulation = population
        };

        MainWindow.ResetSimulation(newConfig);
        _isRunning = true;
    }
}