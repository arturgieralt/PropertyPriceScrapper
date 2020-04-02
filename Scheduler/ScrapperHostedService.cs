using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Scrapper;

namespace Scheduler
{

    public class ScrapperHostedService : IHostedService, IDisposable
{
    private int executionCount = 0;
    private readonly ILogger<ScrapperHostedService> _logger;
    private IServiceProvider _serviceProvider;
    private ScrappingManager _scrapper;
    private Timer _timer;

    public ScrapperHostedService(ILogger<ScrapperHostedService> logger,  IServiceProvider serviceProvider,  ScrappingManager scrapper)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _scrapper = scrapper;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service running.");

        _timer = new Timer(async state => await DoWork(state), null, TimeSpan.Zero, 
            TimeSpan.FromHours(24));

        return Task.CompletedTask;
    }

    private async Task DoWork(object state)
    {
        var count = Interlocked.Increment(ref executionCount);

        var offers = _scrapper.GetOffers("https://www.otodom.pl/sprzedaz/mieszkanie/wroclaw/?search%5Bcreated_since%5D=1&search%5Bregion_id%5D=1&search%5Bsubregion_id%5D=381&search%5Bcity_id%5D=39&nrAdsPerPage=72");
        Console.WriteLine(offers.ToList().Count);
            
        // using( var scope = _serviceProvider.CreateScope()) {
        //     var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        // }
        
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
}