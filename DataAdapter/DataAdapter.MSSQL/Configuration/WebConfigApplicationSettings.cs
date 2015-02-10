using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace DataAdapter.MSSQL.MSSQL.Configuration
{
    public class WebConfigApplicationSettings : IApplicationSettings
    {
        /// <summary>
        /// 读取Web.Config文件中连接字符串
        /// </summary>
        public string DataAdapterConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["DataAdapterConnectionString"].ToString(); }
        }
    }
}
