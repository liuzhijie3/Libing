using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Libing.Orleans.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<HostServer>(s =>
                {
                    s.ConstructUsing(name => new HostServer()); //指定服务用的初始化方法 必须得有这句 不然报错                    
                    s.WhenStarted(tc =>
                    {
                        tc.OnStart();
                    });
                    s.WhenStopped(tc =>
                    {
                        tc.OnStop();
                    });

                });
                x.RunAsLocalSystem().StartAutomatically(); //服务的运行方式
                //服务出错处理
                x.SetDescription("liuzhijie Test");
                x.SetDisplayName("Liu_Service");
                x.SetServiceName("Liu_Service");

                //设置服务恢复设置
                x.EnableServiceRecovery(r =>
                {
                    r.RestartService(3);
                });

                //x.AddCommandLineSwitch("throwonstart", v => throwOnStart = v);
                //x.AddCommandLineSwitch("throwonstop", v => throwOnStop = v);
                //x.AddCommandLineSwitch("throwunhandled", v => throwUnhandled = v);
                x.EnablePauseAndContinue();//支持暂停和继续
                x.EnableShutdown(); //支持关闭

            });
        }
    }
}
