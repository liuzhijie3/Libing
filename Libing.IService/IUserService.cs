using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libing.IService
{
    public interface IUserService : IBaseService
    {
        Task<string> Hello(string name);
    }
}
