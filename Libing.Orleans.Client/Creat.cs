using Libing.Common;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libing.Orleans.Client
{
    public class Creat
    {

        public static Libing.IService.IUserService User()
        {
            return _client.GetGrain<Libing.IService.IUserService>(0);
        }



        #region 系统配置


        static readonly IClusterClient _client;
        static Creat()
        {
            _client = GetClient();
        }

        static IClusterClient GetClient()
        {
            var clusterId = SysConfig.GetAppConfig("OLClusterId") ?? "ClusterId";
            var serviceId = SysConfig.GetAppConfig("OLServiceId") ?? "ServiceId";
            var logLevel = SysConfig.GetAppConfig("OLLogLevel");
            var logPath = SysConfig.GetAppConfig("OLlogPath");


            IClientBuilder builder = new ClientBuilder()
                .UseAdoNetClustering(options =>
                {
                    options.ConnectionString = SysConfig.DBConnStr ?? "server=192.168.2.15;user id=sa;password=123456;database=app_bookmall;";
                    options.Invariant = "System.Data.SqlClient";
                })
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = clusterId;
                    options.ServiceId = serviceId;
                })
                // .AddOutgoingGrainCallFilter<LoggingCallFilter>()
                //.AddOutgoingGrainCallFilter(async context =>
                //{
                //    var dtStart = DateTime.Now.Ticks;
                //    await context.Invoke();
                //    var dtEnd = DateTime.Now.Ticks;
                //    var timeInterval = (dtEnd - long.Parse(dtStart.ToString())) / 10;
                //    if (OnEvent != null)
                //    {
                //        OnEvent(timeInterval);
                //    }


                //})
                //  .UsePerfCounterEnvironmentStatistics()
                //.Configure<ClientMessagingOptions>(option =>
                //{
                //    option.ClientSenderBuckets = 100;
                //})
                //.Configure<LoadSheddingOptions>(op =>
                //{
                //    op.LoadSheddingEnabled = true;
                //    op.LoadSheddingLimit = 90;
                //})
                //.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IProduct).Assembly).WithReferences())
                //.ConfigureLogging(logging =>logging.SetMinimumLevel(LogLevel.Information).AddProvider())
                .ConfigureLogging(logging =>
                {
                    if (!Directory.Exists(logPath))
                    {
                        Directory.CreateDirectory(logPath);
                    }
                    logging.AddFile(logPath + $"\\OrleansSilos-{ DateTime.Now.ToString("yyyy.MM.dd")}.txt");
                    logging.SetMinimumLevel((LogLevel)Enum.Parse(typeof(LogLevel), logLevel));
                });

            //.Configure<GatewayOptions>(option=> {
            //    option.GatewayListRefreshPeriod = TimeSpan.FromSeconds(10);
            //})
            // .ConfigureServices(svc => svc.AddSingleton<ICategoryProperty, CategoryPropertyGrain>())

            IClusterClient client = builder.Build();


            try
            {
                client.Connect(s =>
                {
                    return Task.FromResult(false);
                }).Wait();

            }
            catch (Exception ex)
            {

            }

            return client;
        }

        #endregion
    }
}
