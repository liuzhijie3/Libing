using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libing.Common
{
    public class SysConfig
    {

        /// <summary>
        /// 商城数据库字符串
        /// </summary>
        public static readonly string DBConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnStr"]?.ConnectionString;



        public static string GetAppConfig(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return "";
            }
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }


    }
}
