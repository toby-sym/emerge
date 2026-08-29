using Emerge.Core.Simulation;
using Emerge.Rendering;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Linq;

namespace Emerge_Desktop;

public sealed partial class MainPage : Page
{
    private readonly Simulation _simulation;
    private readonly DispatcherTimer _timer;

    public MainPage()
    {
        InitializeComponent();

        var config = new SimulationConfig
        {
            Seed = 48192837,
            WorldWidth = 800,
            WorldHeight = 600,
            InitialPopulation = 50
        };

        _simulation = new Simulation(config);

        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(50) // ~20 ticks/sec
        };
        _timer.Tick += Timer_Tick;
    }

    private void Timer_Tick(object? sender, object e)
    {
        if (!_simulation.IsRunning)
        {
            _timer.Stop();
            return;
        }

        _simulation.Tick();
        Redraw();
    }

    private void Redraw()
    {
        SimulationCanvas.Children.Clear();

        double scaleX = SimulationCanvas.ActualWidth / _simulation.World.Width;
        double scaleY = SimulationCanvas.ActualHeight / _simulation.World.Height;

        foreach (var cmd in WorldRenderer.Render(_simulation.World))
        {
            var ellipse = new Ellipse
            {
                Width = cmd.Radius * 2,
                Height = cmd.Radius * 2,
                Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(255, cmd.R, cmd.G, cmd.B))
            };

            Canvas.SetLeft(ellipse, cmd.X * scaleX - cmd.Radius);
            Canvas.SetTop(ellipse, cmd.Y * scaleY - cmd.Radius);

            SimulationCanvas.Children.Add(ellipse);
        }

        var pop = _simulation.World.Organisms;
        TickLabel.Text = $"Tick: {_simulation.TickCount}";
        PopulationLabel.Text = $"Population: {pop.Count}";
        AvgSpeedLabel.Text = $"Avg Speed: {(pop.Count > 0 ? pop.Average(o => o.Genome.Speed) : 0):F2}";
    }

    private void StartButton_Click(object sender, RoutedEventArgs e)
    {
        _timer.Start();
    }

    private void PauseButton_Click(object sender, RoutedEventArgs e)
    {
        _timer.Stop();
    }
}