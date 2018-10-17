using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libing.Service
{
    public class UserService :Orleans.Grain, Libing.IService.IUserService
    {
        public Task<string> Hello(string name)
        {
            string r = String.Format("我 {0} 收到了( {1} )", DateTime.Now.ToString("HHmmss"), name);
            return Task.FromResult<string>(r);
        }
    }
}
