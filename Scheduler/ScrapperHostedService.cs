using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BusinessLogic.Models;
using BusinessLogic.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Scheduler.ScheduledTask;
using Scrapper;

namespace Scheduler
{

    public class ScrapperHostedService : IHostedService, IDisposable
{
    private readonly IList<ScrappingRequest> _taskList = new List<ScrappingRequest>(){
        new ScrappingRequest(){
            City = City.Wroclaw,
            OfferType = OfferType.Flat
        },
        new ScrappingRequest(){
            City = City.Warsaw,
            OfferType = OfferType.Flat
        }
    };

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
        
        var tasks = _taskList.Select(SaveOffersFromPage);
        await Task.WhenAll(tasks);
        
        _logger.LogInformation(
            "Timed Hosted Service is working. Count: {Count}", count);
    }

    private async Task SaveOffersFromPage(ScrappingRequest request)
    {
        var url = _urlBuilder.ForCity(request.City).ForType(request.OfferType).Build();
        var offers = _scrapper.GetOffers(url, request.City, request.OfferType);
        
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