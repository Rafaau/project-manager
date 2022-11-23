﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ProjectManager.Core.Services;
public class BackgroundWorkerService : IHostedService
{
  readonly ILogger<BackgroundWorkerService> _logger;

  public BackgroundWorkerService(ILogger<BackgroundWorkerService> logger)
  {
    _logger = logger; 
  }
  public Task StartAsync(CancellationToken cancellationToken)
  {
    _logger.LogInformation("Service started.");
    return Task.CompletedTask;
  }

  public Task StopAsync(CancellationToken cancellationToken)
  {
    _logger.LogInformation("Service stopped.");
    return Task.CompletedTask;
  }

  //protected async override Task ExecuteAsync(CancellationToken stoppingToken)
  //{
  //  while (!stoppingToken.IsCancellationRequested)
  //  {
  //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
  //    await Task.Delay(1000, stoppingToken);
  //  }
  //}
}
