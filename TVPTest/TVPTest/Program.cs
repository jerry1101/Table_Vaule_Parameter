using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;

namespace TVPTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputs = new List<Tuple<int, int, DateTime, DateTime>>();
            inputs.Add(Tuple.Create(9, 101, new DateTime(2018, 11, 1), new DateTime(2018, 12, 12)));
            inputs.Add(Tuple.Create(11, 101, new DateTime(2018, 11, 1), new DateTime(2018, 12, 12)));
            //Creating a data table with the same structure of the TVP 
            DataTable table = new DataTable();
            table.Columns.Add("KeyOne", typeof(int));
            table.Columns.Add("KeyTwo", typeof(int));
            table.Columns.Add("ValueOne", typeof(System.DateTime));
            table.Columns.Add("ValueTwo", typeof(System.DateTime));

            SPUtility.AddRowsToDataTable(table, inputs);


            using (var conn = new SqlConnection("Server=DESKTOP-TQH0UDM\\MSSQLSERVER01;Database=testtvg;Integrated Security=True; "))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("apx_getMaxPresentationLevelsAtStoreItemLevel", conn))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    var inputParameter = cmd.Parameters.AddWithValue("@item_store_nextmonday_outtodate", table);
                    inputParameter.SqlDbType = SqlDbType.Structured;
                    inputParameter.TypeName = "dbo.TwoIntKeysAndTwoDateValues";

                    var reader = cmd.ExecuteReader();
                    var list = SPUtility.DataReaderMapToList<Value>(reader);

                    list.ForEach(item => Console.Write(item.ToString() + ","));
                }
                conn.Close();


            }


        }
    }
    public class SPUtility
    {
        public static List<T> DataReaderMapToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (var prop in obj.GetType().GetProperties())
                {
                    if (!object.Equals(dr[prop.Name], DBNull.Value))
                    {
                        prop.SetValue(obj, dr[prop.Name], null);
                    }
                }
                list.Add(obj);
            }
            return list;
        }

        public static void AddRowsToDataTable(DataTable dt, List<Tuple<int, int, DateTime, DateTime>> list)
        {
            foreach (var item in list)
            {
                dt.Rows.Add(item.Item1, item.Item2, item.Item3, item.Item4);
            }

        }

    }

    public class Value
    {
        public int item_key { get; set; }
        public int store_key { get; set; }
        public int quantity { get; set; }
    }


}