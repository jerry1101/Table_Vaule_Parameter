using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
            var list = new List<Value>();
            using (var context = new PLContext())
            {
                var inputParameter = new SqlParameter("item_store_nextmonday_outtodate", SqlDbType.Structured);
                inputParameter.Value = table;
                inputParameter.TypeName = "dbo.TwoIntKeysAndTwoDateValues";
                //list = context.PLQuantities.FromSql("SELECT TOP 1 [store_key],[item_key],[quantity] FROM [presentation_level]").ToList();
                list = context.PLQuantities.FromSql<Value>
                    ($"EXEC apx_getMaxPresentationLevelsAtStoreItemLevel @item_store_nextmonday_outtodate", inputParameter)
                    .ToList();
                //list=context.Database.ExecuteSqlCommand("EXEC apx_getMaxPresentationLevelsAtStoreItemLevel @item_store_nextmonday_outtodate", inputParameter);


            }
            foreach (var item in list)
            {
                Console.WriteLine(item.item_key);
                Console.WriteLine(item.quantity);
            }



            //using (var conn = new SqlConnection("Server=DESKTOP-TQH0UDM\\MSSQLSERVER01;Database=testtvg;Integrated Security=True; "))
            //{
            //    conn.Open();
            //    using (var cmd = new SqlCommand("apx_getMaxPresentationLevelsAtStoreItemLevel", conn))
            //    {

            //        cmd.CommandType = CommandType.StoredProcedure;

            //        var inputParameter = cmd.Parameters.AddWithValue("@item_store_nextmonday_outtodate", table);
            //        inputParameter.SqlDbType = SqlDbType.Structured;
            //        inputParameter.TypeName = "dbo.TwoIntKeysAndTwoDateValues";

            //        var reader = cmd.ExecuteReader();

            //        var list = SPUtility.DataReaderMapToList<Value>(reader);

            //        list.ForEach(item => Console.Write(item.ToString() + ","));
            //    }
            //    conn.Close();


            //}


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
                //Console.WriteLine(dr["quantity"].GetType().ToString());
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
        [Key]
        public int item_key { get; set; }
        [Key]
        public int store_key { get; set; }
        public short quantity { get; set; }
    }




    public class PLContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-TQH0UDM\\MSSQLSERVER01;Database=testtvg;Integrated Security=True; ");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Value>()
                .HasKey(c => new { c.item_key, c.store_key });
        }



        public DbSet<Value> PLQuantities { get; set; }


    }


}