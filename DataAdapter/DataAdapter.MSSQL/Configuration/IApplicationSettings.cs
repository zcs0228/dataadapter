﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAdapter.MSSQL.MSSQL.Configuration
{
    interface IApplicationSettings
    {
        string DataAdapterConnectionString { get; }
    }
}
