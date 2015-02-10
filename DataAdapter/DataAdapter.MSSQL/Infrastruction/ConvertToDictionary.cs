using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAdapter.MSSQL.Infrastruction
{
    /// <summary>
    /// 将存储数据库字段数据的类转换为字典
    /// </summary>
    public class ConvertToDictionary
    {
        public static IDictionary<string, object> ConvertTo<T>(T valueClass)
        {
            IDictionary<string, object> results = new Dictionary<string, object>();
            if (valueClass is DataRow)
            {
                results = TranslateDataRow(valueClass as DataRow);
            }
            else
            {
                Type typeValue = valueClass.GetType();
                foreach (PropertyInfo item in typeValue.GetProperties())
                {
                    string name = item.Name;
                    object value = item.GetValue(valueClass, null);

                    results.Add(name, value);
                }
            }

            return results;
        }

        private static Dictionary<string, object> TranslateDataRow(DataRow dr)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            DataTable dt = dr.Table;
            foreach (DataColumn item in dt.Columns)
            {
                result.Add(item.ColumnName, dr[item]);
            }

            return result;
        }
    }
}
