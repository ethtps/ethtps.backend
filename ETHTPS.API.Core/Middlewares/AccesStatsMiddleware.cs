﻿using System.Diagnostics;

using Coravel.Queuing.Interfaces;

using ETHTPS.Data.Integrations.MSSQL;
using ETHTPS.Services.BackgroundTasks.Recurring.Aggregated;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ETHTPS.API.Core.Middlewares
{
    public sealed class AccesStatsMiddleware
    {
        private readonly RequestDelegate _next;
        private static int _MAX_QUEUE_SIZE = 100;
        private static int _CURRENT_QUEUE_SIZE = 0;
        private static List<AggregatedEnpointStat> _queueParameters = new();
        private static object _lockObj = new();

        private static int _reqLastPeriod = 0;
        private static int _period = 1000;
        private static int RequestsLastPeriod => Interlocked
            .Increment(ref _reqLastPeriod);
        private static bool _resetRunning = false;

        public AccesStatsMiddleware(RequestDelegate next)
        {
            _next = next;
            if (!_resetRunning)
            {
                ResetRequestCounterEverySecond();
                _resetRunning = true;
            }
        }

        public async Task InvokeAsync(HttpContext context, ILogger<AccesStatsMiddleware> logger, IQueue
            queue)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            await _next(context);
            /*
            try
            {
            }
            catch (Exception e)
            {
                logger.LogError($"{e.GetType()} exception caught by middleware");
#if DEVELOPMENT
                logger.LogError(JsonConvert.SerializeObject(e));
#endif
                context.Response.StatusCode = 400;
                throw;
            }
            finally
            {
                stopwatch.Stop();
            }
            */
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()} | {Math.Round((double)RequestsLastPeriod / (_period / 1000), 2)} hreq/s | {context.Connection.RemoteIpAddress?.MapToIPv4().ToString()} | {context.Request.Method} {context.Request.Path} | {context.Response.StatusCode} | {stopwatch.Elapsed.TotalMilliseconds}ms");

            var payload = new AggregatedEnpointStat()
            {
                Path = context.Request.Path,
                AverageRequestTimeMs = (int)stopwatch.Elapsed.TotalMilliseconds
            };

            lock (_lockObj)
            {
                _queueParameters.Add(payload);
            }
            _CURRENT_QUEUE_SIZE++;
            if (_CURRENT_QUEUE_SIZE >= _MAX_QUEUE_SIZE)
            {
                logger.LogTrace("Enqueuing stats..."); queue.QueueInvocableWithPayload<AggregatedEndpointStatsBuilder, IList<AggregatedEnpointStat>>(_queueParameters);
                queue.QueueBroadcast(new BuildAggregatedStatsEvent());
                _CURRENT_QUEUE_SIZE = 0;
            }
        }

        private static async void ResetRequestCounterEverySecond()
        {
            while (true)
            {
                Interlocked.Add(ref _reqLastPeriod, -_reqLastPeriod);
                await Task.Delay(_period);
            }
        }
    }
}
