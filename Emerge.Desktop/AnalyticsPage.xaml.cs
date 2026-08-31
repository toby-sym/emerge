using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Windows.UI;

namespace Emerge.Desktop;

public sealed partial class AnalyticsPage : Page
{
    private readonly DispatcherTimer _timer;
    private readonly Queue<TelemetrySnapshot> _history = new();
    private const int MaxHistoryPoints = 100;

    private record struct TelemetrySnapshot(long Tick, int Population, int Food);

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
        int popCount;
        int foodCount;

        lock (world.SyncRoot)
        {
            popCount = world.Organisms.Count;
            foodCount = world.Food.Count;
        }

        long currentTick = MainWindow.SimulationInstance.TickCount;

        TxtPopulationCount.Text = popCount.ToString();
        TxtTickCount.Text = currentTick.ToString(); 
        TxtSeedInfo.Text = MainWindow.CurrentConfig.Seed.ToString();

        _history.Enqueue(new TelemetrySnapshot(currentTick, popCount, foodCount));
        while (_history.Count > MaxHistoryPoints)
        {
            _history.Dequeue();
        }

        ChartCanvas.Invalidate();
    }
}

private void ChartCanvas_Draw(CanvasControl sender, CanvasDrawEventArgs args)
{
    var ds = args.DrawingSession;
    float width = (float)sender.ActualWidth;
    float height = (float)sender.ActualHeight;

    if (_history.Count < 2 || width <= 0 || height <= 0)
        return;

    int maxVal = Math.Max(10, _history.Max(p => Math.Max(p.Population, p.Food)));
    float padding = 20f;
    float chartWidth = width - (padding * 2);
    float chartHeight = height - (padding * 2);

    // Draw horizontal background grid lines
    Color gridColor = Color.FromArgb(40, 128, 128, 128);
    for (int i = 0; i <= 4; i++)
    {
        float y = padding + (chartHeight / 4 * i);
        ds.DrawLine(padding, y, width - padding, y, gridColor, 1f);
    }

    var points = _history.ToArray();
    var popPath = new Vector2[points.Length];
    var foodPath = new Vector2[points.Length];

    for (int i = 0; i < points.Length; i++)
    {
        float x = padding + ((float)i / (MaxHistoryPoints - 1)) * chartWidth;
        
        float popY = height - padding - ((float)points[i].Population / maxVal) * chartHeight;
        float foodY = height - padding - ((float)points[i].Food / maxVal) * chartHeight;

        popPath[i] = new Vector2(x, popY);
        foodPath[i] = new Vector2(x, foodY);
    }

    // Render trend lines
    DrawPolyline(ds, popPath, Color.FromArgb(255, 0, 120, 212), 2.5f);  // Blue (Population)
    DrawPolyline(ds, foodPath, Color.FromArgb(255, 16, 124, 65), 2.5f); // Green (Food)
}

    // Add this helper method inside AnalyticsPage
    private static void DrawPolyline(Microsoft.Graphics.Canvas.CanvasDrawingSession ds, Vector2[] points, Color color, float strokeWidth)
    {
        for (int i = 0; i < points.Length - 1; i++)
        {
            ds.DrawLine(points[i], points[i + 1], color, strokeWidth);
        }
    }
    

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        _timer.Stop();
        base.OnNavigatedFrom(e);
    }
}