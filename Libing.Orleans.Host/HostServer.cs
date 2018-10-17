using Libing.Common;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Libing.Orleans.Host
{
    public class HostServer
    {
        ISiloHost siloHost = null;
        public void OnStart()
        {
            #region 变量
            var siloPort = SysConfig.GetAppConfig("OLSiloPort");
            var gatewayPort = SysConfig.GetAppConfig("OLGatewayPort");
            var environment = SysConfig.GetAppConfig("OLEnvironment");

            var clusterId = SysConfig.GetAppConfig("OLClusterId");
            var serviceId = SysConfig.GetAppConfig("OLServiceId");
            var logLevel = SysConfig.GetAppConfig("OLLogLevel");
            var logPath = SysConfig.GetAppConfig("OLlogPath");
            var DBConnStr = SysConfig.DBConnStr;

            #endregion

            ISiloHostBuilder builder = new SiloHostBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = clusterId;
                    options.ServiceId = serviceId;
                })
                .UseEnvironment(environment)
                .Configure<EndpointOptions>(options =>
                {
                    options.AdvertisedIPAddress = IPAddress.Parse("192.168.2.132");
                    options.GatewayPort = int.Parse(gatewayPort);
                    options.SiloPort = int.Parse(siloPort);
                })
                .UseAdoNetClustering(options =>
                {
                    options.ConnectionString = DBConnStr;
                    options.Invariant = "System.Data.SqlClient";
                })
                .ConfigureLogging(logging =>
                {
                    logging.AddFile(logPath + $"\\OrleansSilos -{ DateTime.Now.ToString("yyyy.MM.dd")}.txt");
                    logging.SetMinimumLevel((LogLevel)Enum.Parse(typeof(LogLevel), logLevel));
                })
                .ConfigureApplicationParts(parts =>
                {
                    Assembly assembly = typeof(Libing.Service.UserService).Assembly;
                    parts.AddApplicationPart(assembly).WithReferences();
                });

            try
            {
                Console.WriteLine("Build SiloHost.....");
                siloHost = builder.Build();
                Console.WriteLine("Async Sart SiloHost.....");
                siloHost.StartAsync().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine("创建服务异常:" + ex.ToString());
            }


        }
        public void OnStop()
        {
            siloHost?.Dispose();
        }

        private static IPAddress GetLocalIp()
        {
            var allIp = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            if (allIp == null || allIp.Count() == 0)
            {
                return null;
            }
            foreach (var ip in allIp)
            {
                if (ip.ToString().StartsWith("192.168."))
                {
                    return ip;
                }
            }
            return null;
        }
    }
}
