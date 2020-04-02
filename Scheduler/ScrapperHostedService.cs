using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BusinessLogic.Services;
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
    private OfferService _offerService;
    private ScrappingManager _scrapper;
    private UrlBuilder _urlBuilder;
    private Timer _timer;

    public ScrapperHostedService(
        ILogger<ScrapperHostedService> logger,  
        OfferService offerService, 
        UrlBuilder urlBuilder, 
        ScrappingManager scrapper)
    {
        _logger = logger;
        _offerService = offerService;
        _scrapper = scrapper;
        _urlBuilder = urlBuilder;
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
        
        var tasks = GetUrlList().Select(SaveOffersFromPage);
        await Task.WhenAll(tasks);
        
        _logger.LogInformation(
            "Timed Hosted Service is working. Count: {Count}", count);
    }

    private IEnumerable<string> GetUrlList()
    {
        return new List<string>(){
            _urlBuilder.ForWroclaw().ForFlat().Build(),
            _urlBuilder.ForWarsaw().ForFlat().Build(),
        };
    }

    private async Task SaveOffersFromPage(string url)
    {
        var offers = _scrapper.GetOffers(url);
        
        if(offers.ToList().Count > 0) 
        {
            await _offerService.InsertManyAsync(offers);   
        }
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