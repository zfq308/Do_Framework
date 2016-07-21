using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoFramework.sysCore.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DoFramework.sysCore.Helper.Tests
{
    [TestClass()]
    public class DataTableCompareTests
    {
        /// <summary>
        /// 账面数据 Accounting
        /// </summary>
        /// <returns></returns>
        DataTable setDataAccounting()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("empno", typeof(string));
            dt.Columns.Add("empname", typeof(string));
            dt.Columns.Add("sex", typeof(bool));
            dt.Columns.Add("wage", typeof(decimal));
            dt.Columns.Add("birthday", typeof(DateTime));
            dt.Rows.Add(1, "L0001", "涂聚文", false, 4500, "1970-03-04");
            dt.Rows.Add(2, "L0002", "刘杰", false, 4300, "1972-04-04");
            dt.Rows.Add(3, "L0003", "宋承宪", false, 4500, "1974-04-04");
            dt.Rows.Add(4, "L0005", "宁夏", false, 4500, "1973-04-04");
            dt.Rows.Add(6, "L0009", "江东", true, 5500, "1975-04-04");
            dt.Rows.Add(6, "L0010", "李燕云", true, 9500, "1976-04-04");
            dt.Rows.Add(7, "L0020", "赵雅芝", false, 14500, "1977-04-04");

            return dt;
        }
        /// <summary>
        /// 盘点数据 Inventory
        /// </summary>
        /// <returns></returns>
        DataTable setDataInventory()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("empno", typeof(string));
            dt.Columns.Add("empname", typeof(string));
            dt.Columns.Add("sex", typeof(bool));
            dt.Columns.Add("wage", typeof(decimal));
            dt.Columns.Add("birthday", typeof(DateTime));
            dt.Rows.Add(10, "L0001", "涂聚文", false, 4500, "1970-03-04");
            dt.Rows.Add(11, "L0002", "刘杰", false, 4300, "1972-04-04");
            dt.Rows.Add(12, "L0009", "江东", true, 5500, "1973-04-04");
            dt.Rows.Add(13, "L0010", "李燕云", true, 9500, "1974-04-04");
            dt.Rows.Add(14, "L0020", "赵雅芝", false, 14500, "1975-04-04");
            dt.Rows.Add(15, "L0032", "徐若萱", false, 4300, "1976-04-04");
            dt.Rows.Add(16, "L0056", "保芝林", true, 4200, "1977-04-04");
            dt.Rows.Add(17, "L0042", "何燕华", false, 4100, "1978-04-04");
            dt.Rows.Add(18, "L0052", "黄花菜", false, 4400, "1979-04-04");
            dt.Rows.Add(19, "L0012", "艾薇儿", true, 5500, "1982-04-04");
            dt.Rows.Add(20, "L0018", "傅艺伟", false, 6500, "1932-04-04");
            dt.Rows.Add(21, "L0028", "李世民", false, 9500, "1992-04-04");
            return dt;
        }

        [TestMethod()]
        public void CompareLinQDataTableTest()
        {

            DataTable datadiff1 = new DataTable();
            DataTable datadiff2 = new DataTable();
            DataTable dataOverage = new DataTable();//盘盈
            DataTable dataInventoryLoss = new DataTable();//盘亏

            DataTable datatable1 = setDataAccounting();
            DataTable datatable2 = setDataInventory();

            #region
            //var qry1 = datatable1.AsEnumerable().Select(a => new { MobileNo = a["empno"].ToString() });
            //var qry2 = datatable2.AsEnumerable().Select(b => new { MobileNo = b["empno"].ToString() });
            //var exceptAB = qry1.Except(qry2);
            ////
            //DataTable dtMisMatch = (from a in datatable1.AsEnumerable()
            //                        join ab in exceptAB on a["empno"].ToString() equals ab.MobileNo
            //                        select a).CopyToDataTable();

            ////detect row deletes - a row is in datatable1 except missing from datatable2
            //var exceptAB1 = qry1.Except(qry2);
            // dataInventoryLoss= (from a in datatable1.AsEnumerable()
            //               join ab in exceptAB1 on a["empno"].ToString() equals ab.MobileNo
            //               select a).CopyToDataTable();
            ////detect row inserts - a row is in datatable2 except missing from datatable1
            //var exceptAB2 = qry2.Except(qry1);
            // dataOverage = (from a in datatable2.AsEnumerable()
            //                     join ab in exceptAB2 on a["empno"].ToString() equals ab.MobileNo
            //                     select a).CopyToDataTable();

            #endregion

            DataTableCompare.CompareLinQDataTable(setDataAccounting(), setDataInventory(), "empno", out dataOverage, out dataInventoryLoss);

        }

        [TestMethod()]
        public void CompareDataTableTest()
        {
            DataTable datadiff1 = new DataTable();
            DataTable datadiff2 = new DataTable();
            DataTable dataOverage = new DataTable();//盘盈
            DataTable dataInventoryLoss = new DataTable();//盘亏

            DataTable datatable1 = setDataAccounting();
            DataTable datatable2 = setDataInventory();

            DataTableCompare.CompareDataTable(setDataAccounting(), setDataInventory(), "empno", "id", out dataOverage, out datadiff1, out datadiff2, out dataInventoryLoss);
        }
    }
}