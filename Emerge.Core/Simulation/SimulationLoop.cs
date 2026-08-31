namespace Emerge.Core.Simulation;

public sealed class SimulationLoop
{
    private readonly Simulation _simulation;
    private readonly int _tickIntervalMilliseconds;
    private CancellationTokenSource? _loopCancellationTokenSource;
    private Task? _loopTask;

    public SimulationLoop(Simulation simulation, int tickIntervalMilliseconds = 50)
    {
        _simulation = simulation;
        _tickIntervalMilliseconds = tickIntervalMilliseconds;
    }

    public bool IsRunning { get; private set; }
    public int TickCount => _simulation.TickCount;

    public void Start()
    {
        if (IsRunning)
        {
            return;
        }

        _simulation.Start();
        IsRunning = true;
        _loopCancellationTokenSource = new CancellationTokenSource();
        _loopTask = RunLoopAsync(_loopCancellationTokenSource.Token);
    }

    public void Stop()
    {
        if (!IsRunning)
        {
            return;
        }

        IsRunning = false;
        _simulation.Stop();
        _loopCancellationTokenSource?.Cancel();
    }

    private async Task RunLoopAsync(CancellationToken cancellationToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(_tickIntervalMilliseconds));

        try
        {
            while (!cancellationToken.IsCancellationRequested && _simulation.IsRunning)
            {
                await timer.WaitForNextTickAsync(cancellationToken);
                _simulation.Tick();
            }
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            IsRunning = false;
            _simulation.Stop();
            _loopCancellationTokenSource?.Dispose();
            _loopCancellationTokenSource = null;
            _loopTask = null;
        }
    }
}
