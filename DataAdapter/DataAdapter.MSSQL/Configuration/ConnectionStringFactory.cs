using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAdapter.MSSQL.MSSQL.Configuration
{
    public class ConnectionStringFactory   //连接字符串工厂
    {
        private static IApplicationSettings _applicationSettings;

        static ConnectionStringFactory()
        {
            _applicationSettings = new WebConfigApplicationSettings();
        }

        /// <summary>
        /// 获得连接字符串
        /// </summary>
        public static string DataAdapterConnectionString
        {
            get { return _applicationSettings.DataAdapterConnectionString; }
        }
    }
}
