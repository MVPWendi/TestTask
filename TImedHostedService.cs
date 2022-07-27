using TestTask;

public class TimedHostedService : IHostedService, IDisposable
{
    private int executionCount = 0;
    private readonly ILogger<TimedHostedService> _logger;
    private Timer? _timer = null;

    public TimedHostedService(ILogger<TimedHostedService> logger)
    {
        _logger = logger;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service running.");
        
        _timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromSeconds(5));

        return Task.CompletedTask;
    }

    private void DoWork(object? state)
    {
        var count = Interlocked.Increment(ref executionCount);
        _logger.LogInformation($"DATE: {DateTime.Now}");
        foreach (var candidate in Database.GetCandidatesWhoDontDoTask())
        {
            _logger.LogInformation($"CAND: {candidate.DateToCompleteTask}");
            if ((DateTime.Now - candidate.DateToCompleteTask).TotalSeconds > 0)
            {
                _logger.LogInformation("Set Expire");
                Database.SetExpiredTask(candidate);
                HTTP.SendHTTP(candidate.PhoneNumber, "истекло время выполнения задания");
            }
        }
        _logger.LogInformation(
            "Timed Hosted Service is working. Count: {Count}", count);
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}