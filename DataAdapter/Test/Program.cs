using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAdapter.MSSQL;
using DataAdapter.MSSQL.Infrastruction;
using System.Data.SqlClient;
using DataAdapter.MSSQL.Translators;
using System.Collections;
using System.Data;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string conn = "Data Source=QH-20140814XCYI;Initial Catalog=Test;Integrated Security=True";
            SqlServerDataFactory dataFactory = new SqlServerDataFactory(conn);
            DataTable dttest = new DataTable();
            DataColumn dc = new DataColumn("d", typeof(string));
            dttest.Columns.Add(dc);

            DataColumn dc1 = new DataColumn("c", typeof(int));
            dttest.Columns.Add(dc1);

            DataRow dr = dttest.NewRow();
            dr["c"] = 2;
            dr["d"] = "123";
            dttest.Rows.Add(dr);
            DataRow dr1 = dttest.NewRow();
            dr1["c"] = 3;
            dr1["d"] = "123";
            dttest.Rows.Add(dr1);
            //dataFactory.Insert("B", dttest);

            string[] key = { "c" };
            dataFactory.Update("B", dttest, key);
            Console.ReadKey();


            //////////////////////////////////////delete
            Delete delete = new Delete("tableName");
            delete.AddCriterions("a", "zhangxin", "1", CriteriaOperator.MoreThan);
            delete.AddCriterions("a", "zhangdi", 2, CriteriaOperator.LessThan);
            delete.AddCriterions("b", "2", CriteriaOperator.Equal);
            delete.AddSqlOperator(SqlOperator.OR);

            SqlCommand deleteCmd = new SqlCommand();
            DeleteTranslator.TranslateIntoDelete(delete, deleteCmd);
            Console.WriteLine(deleteCmd.CommandText);
            Console.WriteLine(deleteCmd.Parameters.Count);
            //Console.ReadKey();
            //////////////////////////////////////////////////

            Test test = new Test
            {
                a = "1",
                b = "2",
                c = "3"
            };
            Insert<Test> insert = new Insert<Test>("tableName", test);
            insert.AddExcludeField("a");
            SqlCommand insertCmd = new SqlCommand();
            SaveTranslator.TranslateIntoInsert<Test>(insert, insertCmd);
            Console.WriteLine(insertCmd.CommandText);
            Console.WriteLine(insertCmd.Parameters.Count);
            Console.ReadKey();
            ////////////////////////////////////////////////////////////

            Test test1 = new Test
            {
                a = "1",
                b = "2",
                c = "3"
            };
            Update<Test> update = new Update<Test>("taleName", test1);
            update.AddCriterion("a", "a", "1", CriteriaOperator.Equal);
            update.AddCriterion("b", "b", "2", CriteriaOperator.Equal);
            update.AddExcludeField("c");
            update.AddSqlOperator(SqlOperator.AND);
            SqlCommand updateCmd = new SqlCommand();
            SaveTranslator.TranslateIntoUpdate<Test>(update, updateCmd);
            Console.WriteLine(updateCmd.CommandText);
            Console.WriteLine(updateCmd.Parameters.Count);
            Console.ReadKey();

            ////////////////////////////////////////////////////////////

            Query query = new Query("tableName");
            query.AddCriterion("a.b", "1", CriteriaOperator.Equal);
            //query.AddCriterion("b", "b", "2", CriteriaOperator.Equal);
            query.AddCriterion("c", "____", CriteriaOperator.Like);
            query.AddOrderByClause(new OrderByClause("a", false));
            query.AddOrderByClause(new OrderByClause("b", true));
            //query.AddSqlOperator(SqlOperator.AND);
            SqlCommand queryCmd = new SqlCommand();
            QueryTranslator.TranslateIntoSelect(query, queryCmd);
            Console.WriteLine(queryCmd.CommandText);
            Console.WriteLine(queryCmd.Parameters.Count);
            //Console.ReadKey();

            //////////////////////////////////////////////////////////////

            ComplexQuery cmquery = new ComplexQuery();
            
            IList<NeedField> list = new List<NeedField>();
            NeedField needField = new NeedField
            {
                TableName = "tableName1",
                FieldName = "a",
                VariableName = ""
            };
            list.Add(needField);
            cmquery.NeedFields = list;
            cmquery.AddNeedField("tableName2", "b");
            cmquery.AddNeedField("tableName3", "c", "test");
            IDictionary<string, string> testjoin = new Dictionary<string, string>();
            testjoin.Add("tableName1", "c1");
            testjoin.Add("tableName2", "c2");
            testjoin.Add("tableName3", "c3");


            //cmquery.AddJoinCriterion("assss", JoinType.FULL_JOIN);
            cmquery.AddJoinCriterion(testjoin, JoinType.INNER_JOIN);



            //cmquery.IsDictinct = true;
            //cmquery.TopNumber = 9;
            cmquery.AddCriterion("a", "a", "1", CriteriaOperator.Equal);
            cmquery.AddCriterion("b", "b", "2", CriteriaOperator.Equal);
            cmquery.AddCriterion("c", "c", "3", CriteriaOperator.Like);
            List<OrderByClause> orders = new List<OrderByClause>();
            orders.Add(new OrderByClause("a", true));
            orders.Add(new OrderByClause("b", false));
            cmquery.OrderByClauses = orders;
            SqlCommand cmqueryCmd = new SqlCommand();
            ComplexQueryTranslator.TranslateIntoComplexQuery(cmquery, cmqueryCmd);
            Console.WriteLine(cmqueryCmd.CommandText);
            Console.WriteLine(cmqueryCmd.Parameters.Count);
            //Console.ReadKey();


            ///////////////////////////////////////////////////////////
            ComplexQuery testquery = new ComplexQuery();
            Console.WriteLine(testquery.TopNumber);
            Console.WriteLine(DateTime.Today);
            Console.WriteLine(DateTime.Now.AddDays(-1).Date);
            //Console.ReadKey();

            ///////////////////////////////////////////////////////

            //Query testQuery = new Query("B");
            //SqlServerDataFactory dataFactory = new SqlServerDataFactory("Data Source=QH-20140814XCYI;Initial Catalog=Test;Integrated Security=True");
            //IEnumerable<BModle> result = dataFactory.Query<BModle>(testQuery);
            //foreach (var item in result)
            //{
            //    //BModle a = (BModle)item;
            //    Console.WriteLine(item.d);
            //    Console.WriteLine(item.c);
            //}
            //Console.WriteLine(result.Count());
            //Console.ReadKey();

            //JoinCriterion joinCri = new JoinCriterion();
            //Console.WriteLine("++" + joinCri.DefaultJoinFieldName + "++");
            //Console.ReadKey();

            //Criterion testcri = new Criterion("name","",1, CriteriaOperator.Equal);

            //Console.WriteLine(testcri.ParameterName);
            //Console.ReadKey();

            //SqlServerDataFactory dataFactory = new SqlServerDataFactory("");
            //SqlParameter a = new SqlParameter();
            //SqlParameter b = new SqlParameter("", typeof(int));
            //dataFactory.Query("", a, b);
            //dataFactory.Query("");
            //SqlParameter[] arry = { new SqlParameter(), new SqlParameter("", typeof(int)) };
            //dataFactory.Query("", arry);


            ////////////////////////////////////////////////////////////////////////////////
            DataTable dt = new DataTable();
            dt.Columns.Add("a", typeof(int));
            dt.Columns.Add("b", typeof(string));
            DataRow newRow = dt.NewRow();
            newRow[0] = 1;
            newRow[1] = "aaaa";
            dt.Rows.Add(newRow);

            Insert<DataRow> rowInsert = new Insert<DataRow>("myTableName", dt.Rows[0]);
            SaveTranslator.TranslateIntoInsert(rowInsert, insertCmd);
            Console.WriteLine(insertCmd.CommandText);
            Console.WriteLine(insertCmd.Parameters.Count);
            Console.ReadKey();
            
        }
    }
}
