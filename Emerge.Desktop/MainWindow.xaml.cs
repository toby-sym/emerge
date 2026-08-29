using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.UI;
using Emerge.Core.Simulation;
using Emerge.Rendering;
using Emerge.Desktop;

namespace Emerge.Desktop;

public sealed partial class MainWindow : Window
{
    private readonly Simulation _simulation;

    public MainWindow()
    {
        this.InitializeComponent();

        this.SystemBackdrop = new MicaBackdrop();
        this.ExtendsContentIntoTitleBar = true;
        this.SetTitleBar(AppTitleBar);

        // Fix 1: Initialize required SimulationConfig properties
        var config = new SimulationConfig
        {
            Seed = 48192837,
            WorldWidth = 800,
            WorldHeight = 600,
            InitialPopulation = 100 
        };
        _simulation = new Simulation(config);
    }

    private void Canvas_CreateResources(CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
    {
    }

    private void Canvas_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
    {
        _simulation.Tick();

        var session = args.DrawingSession;
        session.Clear(Color.FromArgb(255, 18, 18, 18));

        foreach (var cmd in WorldRenderer.Render(_simulation.World))
        {
            session.FillCircle(
                (float)cmd.X,
                (float)cmd.Y,
                (float)cmd.Radius,
                Color.FromArgb(255, cmd.R, cmd.G, cmd.B)
            );
        }
    }
}