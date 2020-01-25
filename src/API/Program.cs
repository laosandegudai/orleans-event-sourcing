﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Hosting;
using Orleans;
using Orleans.Configuration;
using Microsoft.Extensions.Hosting;
using Orleans.Clustering.Kubernetes;
using Grains;
using Orleans.Statistics;
using System;
using System.Net;

namespace API
{
    public class Program
    {
        const int siloPort = 11111;
        const int gatewayPort = 30000;
        
        //https://stackoverflow.com/questions/54841844/orleans-direct-client-in-asp-net-core-project/54842916#54842916
        private static void ConfigureOrleans(ISiloBuilder builder)
        {
            // get injected pod ip address 
            var podIPAddress = Environment.GetEnvironmentVariable("POD_IP");
            builder.Configure<ClusterOptions>(options => 
            {
                options.ClusterId = "testcluster";
                options.ServiceId = "SampleApp";
            })
            .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Parse(podIPAddress))
            .ConfigureEndpoints(siloPort: siloPort, gatewayPort: gatewayPort)
            .UseKubeMembership(opt =>
            {
                opt.DropResourcesOnInit = true;
            })
            .AddMemoryGrainStorageAsDefault()
            .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(ValueGrain).Assembly).WithReferences())
            .ConfigureLogging(logging => logging.AddConsole())
            .UseLinuxEnvironmentStatistics()
            .UseDashboard(x =>
             {
                x.HostSelf = false;
                x.BasePath = "/dashboard";
                x.ScriptPath = "/api/dashboard";
                x.CounterUpdateIntervalMs = 10000;
            });
        }
        
        private static void ConfigureLocalOrleans(ISiloBuilder builder)
        {
            builder.Configure<ClusterOptions>(options => 
            {
                options.ClusterId = "testcluster";
                options.ServiceId = "SampleApp";
            })
            .UseLocalhostClustering()
            .ConfigureEndpoints(new Random(1).Next(10001, 10100), new Random(1).Next(20001, 20100))
            .ConfigureEndpoints(siloPort: siloPort, gatewayPort: gatewayPort)
            .AddMemoryGrainStorageAsDefault()
            .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(ValueGrain).Assembly).WithReferences())
            .ConfigureLogging(logging => logging.AddConsole())
            .UseDashboard(x =>
             {
                x.HostSelf = false;
                x.BasePath = "/dashboard";
                x.ScriptPath = "/api/dashboard";
                x.CounterUpdateIntervalMs = 10000;
             });
        }
        
        public static void Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.UseStartup<Startup>();
                })
                .UseOrleans(ConfigureOrleans)
                .Build();
            host.Run();
        }
    }
}
