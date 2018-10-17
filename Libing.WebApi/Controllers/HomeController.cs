using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Libing.WebApi.Controllers
{
    public class HomeController : ApiController
    {
        public async Task<string> Get(string name = "匿名")
        {
            string r = name;
            Libing.IService.IUserService user = Libing.Orleans.Client.Creat.User();
            r = await user.Hello(name);

            return "我 ( " + DateTime.Now.ToString("HHmmssfff") + " ) 知道了 " + r;
        }
    }
}
