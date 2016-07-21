using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoFramework.sysCore.Helper
{
    /// <summary>
    /// http://www.cnblogs.com/geovindu/p/4692644.html
    /// </summary>
    public class DataTableCompare
    {
        /// <summary>
        /// 比较两个DataTableCompare数据（结构相同）
        /// 涂聚文
        /// http://www.codeproject.com/Tips/344792/Compare-two-datatable-using-LINQ-Query
        /// </summary>
        /// <param name="datatable1"></param>
        /// <param name="datatable2"></param>
        /// <param name="keyField"></param>
        /// <param name="dataOverage"></param>
        /// <param name="dataInventoryLoss"></param>
        public static void CompareLinQDataTable(DataTable datatable1, DataTable datatable2, string keyField, out DataTable dataOverage, out DataTable dataInventoryLoss)
        {
            var qry1 = datatable1.AsEnumerable().Select(a => new { IdNo = a[keyField].ToString() });
            var qry2 = datatable2.AsEnumerable().Select(b => new { IdNo = b[keyField].ToString() });
            //detect row deletes - a row is in datatable1 except missing from datatable2
            var exceptAB1 = qry1.Except(qry2);
            dataInventoryLoss = (from a in datatable1.AsEnumerable()
                                 join ab in exceptAB1 on a[keyField].ToString() equals ab.IdNo
                                 select a).CopyToDataTable();
            //detect row inserts - a row is in datatable2 except missing from datatable1
            var exceptAB2 = qry2.Except(qry1);
            dataOverage = (from a in datatable2.AsEnumerable()
                           join ab in exceptAB2 on a[keyField].ToString() equals ab.IdNo
                           select a).CopyToDataTable();

        }

        /// <summary>
        /// 比较两个DataTableCompare数据（结构相同）
        /// 来源于:http://www.cnblogs.com/houlinbo/archive/2010/02/10/1667189.html
        /// </summary>
        /// <param name="dt1">来自数据库的DataTable</param>
        /// <param name="dt2">来自文件的DataTable</param>
        /// <param name="keyField">要比较的关键字段名</param>
        /// <param name="keyid">不需要比较的字段名id</param>
        /// <param name="dtRetAdd">新增数据（dt2中的数据）</param>
        /// <param name="dtRetDif1">不同的数据（数据库中的数据）</param>
        /// <param name="dtRetDif2">不同的数据（dt2中的数据,修改过的）</param>
        /// <param name="dtRetDel">删除的数据（dt2中的数据）</param>
        public static void CompareDataTable(DataTable dt1, DataTable dt2, string keyField, string keyid, out DataTable dtRetAdd, out DataTable dtRetDif1, out DataTable dtRetDif2, out DataTable dtRetDel)
        {
            //为三个表拷贝表结构
            dtRetDel = dt1.Clone();
            dtRetAdd = dtRetDel.Clone();
            dtRetDif1 = dtRetDel.Clone();
            dtRetDif2 = dtRetDel.Clone();

            int colCount = dt1.Columns.Count;

            DataView dv1 = dt1.DefaultView;
            DataView dv2 = dt2.DefaultView;

            //先以第一个表为参照，看第二个表是修改了还是删除了
            foreach (DataRowView dr1 in dv1)
            {
                dv2.RowFilter = keyField + " = '" + dr1[keyField].ToString() + "'";
                if (dv2.Count > 0)
                {
                    if (!CompareUpdate(dr1, dv2[0]))//比较是否有不同的
                    {
                        dtRetDif1.Rows.Add(dr1.Row.ItemArray);//修改前
                        dtRetDif2.Rows.Add(dv2[0].Row.ItemArray);//修改后
                        dtRetDif2.Rows[dtRetDif2.Rows.Count - 1][keyid] = dr1.Row[keyid];//将ID赋给来自文件的表，因为它的ID全部==0
                        continue;
                    }
                }
                else
                {
                    //已经被删除的
                    dtRetDel.Rows.Add(dr1.Row.ItemArray);
                }
            }

            //以第一个表为参照，看记录是否是新增的
            dv2.RowFilter = "";//清空条件
            foreach (DataRowView dr2 in dv2)
            {
                dv1.RowFilter = keyField + " = '" + dr2[keyField].ToString() + "'";
                if (dv1.Count == 0)
                {
                    //新增的
                    dtRetAdd.Rows.Add(dr2.Row.ItemArray);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dr1"></param>
        /// <param name="dr2"></param>
        /// <returns></returns>
        private static bool CompareUpdate(DataRowView dr1, DataRowView dr2)
        {
            //行里只要有一项不一样，整个行就不一样,无需比较其它
            object val1;
            object val2;
            for (int i = 1; i < dr1.Row.ItemArray.Length; i++)
            {
                val1 = dr1[i];
                val2 = dr2[i];
                if (!val1.Equals(val2))
                {
                    return false;
                }
            }
            return true;
        }

    }


    /// <summary>
    ///datatable relational operators
    ///http://weblogs.sqlteam.com/davidm/archive/2004/01/21/753.aspx
    /// </summary>
    public class SQLOps
    {
        #region Join

        /// <summary>
        ///
        /// </summary>
        /// <param name="First"></param>
        /// <param name="Second"></param>
        /// <param name="FJC"></param>
        /// <param name="SJC"></param>
        /// <returns></returns>
        public static DataTable Join(DataTable First, DataTable Second, DataColumn[] FJC, DataColumn[] SJC)
        {
            //Create Empty Table
            DataTable table = new DataTable("Join");

            // Use a DataSet to leverage DataRelation
            using (DataSet ds = new DataSet())
            {
                //Add Copy of Tables
                ds.Tables.AddRange(new DataTable[] { First.Copy(), Second.Copy() });
                //Identify Joining Columns from First
                DataColumn[] parentcolumns = new DataColumn[FJC.Length];
                for (int intLoop = 0; intLoop < parentcolumns.Length; intLoop++)
                {
                    parentcolumns[intLoop] = ds.Tables[0].Columns[FJC[intLoop].ColumnName];
                }

                //Identify Joining Columns from Second
                DataColumn[] childcolumns = new DataColumn[SJC.Length];
                for (int intLoop = 0; intLoop < childcolumns.Length; intLoop++)
                {
                    childcolumns[intLoop] = ds.Tables[1].Columns[SJC[intLoop].ColumnName];
                }
                //Create DataRelation
                DataRelation r = new DataRelation(string.Empty, parentcolumns, childcolumns, false);
                ds.Relations.Add(r);
                //Create Columns for JOIN table
                for (int intLoop = 0; intLoop < First.Columns.Count; intLoop++)
                {
                    table.Columns.Add(First.Columns[intLoop].ColumnName, First.Columns[intLoop].DataType);
                }
                for (int intLoop = 0; intLoop < Second.Columns.Count; intLoop++)
                {
                    //Beware Duplicates
                    if (!table.Columns.Contains(Second.Columns[intLoop].ColumnName))
                        table.Columns.Add(Second.Columns[intLoop].ColumnName, Second.Columns[intLoop].DataType);
                    else
                        table.Columns.Add(Second.Columns[intLoop].ColumnName + "_Second", Second.Columns[intLoop].DataType);
                }
                //Loop through First table
                table.BeginLoadData();
                foreach (DataRow firstrow in ds.Tables[0].Rows)
                {
                    //Get "joined" rows
                    DataRow[] childrows = firstrow.GetChildRows(r);
                    if (childrows != null && childrows.Length > 0)
                    {
                        object[] parentarray = firstrow.ItemArray;
                        foreach (DataRow secondrow in childrows)
                        {
                            object[] secondarray = secondrow.ItemArray;
                            object[] joinarray = new object[parentarray.Length + secondarray.Length];
                            Array.Copy(parentarray, 0, joinarray, 0, parentarray.Length);
                            Array.Copy(secondarray, 0, joinarray, parentarray.Length, secondarray.Length);
                            table.LoadDataRow(joinarray, true);
                        }
                    }
                }
                table.EndLoadData();
            }
            return table;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="First"></param>
        /// <param name="Second"></param>
        /// <param name="FJC"></param>
        /// <param name="SJC"></param>
        /// <returns></returns>
        public static DataTable Join(DataTable First, DataTable Second, DataColumn FJC, DataColumn SJC)
        {

            return Join(First, Second, new DataColumn[] { FJC }, new DataColumn[] { SJC });

        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="First"></param>
        /// <param name="Second"></param>
        /// <param name="FJC"></param>
        /// <param name="SJC"></param>
        /// <returns></returns>
        public static DataTable Join(DataTable First, DataTable Second, string FJC, string SJC)
        {
            return Join(First, Second, new DataColumn[] { First.Columns[FJC] }, new DataColumn[] { First.Columns[SJC] });
        }
        #endregion

        #region Distinct

        /// <summary>
        ///
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="Columns"></param>
        /// <returns></returns>
        public static DataTable Distinct(DataTable Table, DataColumn[] Columns)
        {

            //Empty table

            DataTable table = new DataTable("Distinct");

            //Sort variable

            string sort = string.Empty;



            //Add Columns & Build Sort expression

            for (int i = 0; i < Columns.Length; i++)
            {

                table.Columns.Add(Columns[i].ColumnName, Columns[i].DataType);

                sort += Columns[i].ColumnName + ",";

            }

            //Select all rows and sort

            DataRow[] sortedrows = Table.Select(string.Empty, sort.Substring(0, sort.Length - 1));



            object[] currentrow = null;

            object[] previousrow = null;



            table.BeginLoadData();

            foreach (DataRow row in sortedrows)
            {

                //Current row

                currentrow = new object[Columns.Length];

                for (int i = 0; i < Columns.Length; i++)
                {

                    currentrow[i] = row[Columns[i].ColumnName];

                }



                //Match Current row to previous row

                if (!SQLOps.RowEqual(previousrow, currentrow))

                    table.LoadDataRow(currentrow, true);



                //Previous row

                previousrow = new object[Columns.Length];

                for (int i = 0; i < Columns.Length; i++)
                {

                    previousrow[i] = row[Columns[i].ColumnName];

                }



            }

            table.EndLoadData();

            return table;



        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="Column"></param>
        /// <returns></returns>
        public static DataTable Distinct(DataTable Table, DataColumn Column)
        {

            return Distinct(Table, new DataColumn[] { Column });

        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="Column"></param>
        /// <returns></returns>
        public static DataTable Distinct(DataTable Table, string Column)
        {
            return Distinct(Table, Table.Columns[Column]);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="Columns"></param>
        /// <returns></returns>
        public static DataTable Distinct(DataTable Table, params string[] Columns)
        {

            DataColumn[] columns = new DataColumn[Columns.Length];

            for (int i = 0; i < Columns.Length; i++)
            {

                columns[i] = Table.Columns[Columns[i]];



            }

            return Distinct(Table, columns);

        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Table"></param>
        /// <returns></returns>
        public static DataTable Distinct(DataTable Table)
        {

            DataColumn[] columns = new DataColumn[Table.Columns.Count];

            for (int i = 0; i < Table.Columns.Count; i++)
            {

                columns[i] = Table.Columns[i];



            }

            return Distinct(Table, columns);

        }
        #endregion

        #region UnitesDataTable

        /// <summary>
        /// 结构不同
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="DTName"></param>
        /// <returns></returns>
        public static DataTable UnitesDataTable(DataTable dt1, DataTable dt2, string DTName)
        {
            DataTable dt3 = dt1.Clone();
            for (int i = 0; i < dt2.Columns.Count; i++)
            {
                dt3.Columns.Add(dt2.Columns[i].ColumnName);
            }
            object[] obj = new object[dt3.Columns.Count];

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                dt1.Rows[i].ItemArray.CopyTo(obj, 0);
                dt3.Rows.Add(obj);
            }

            if (dt1.Rows.Count >= dt2.Rows.Count)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    for (int j = 0; j < dt2.Columns.Count; j++)
                    {
                        dt3.Rows[i][j + dt1.Columns.Count] = dt2.Rows[i][j].ToString();
                    }
                }
            }
            else
            {
                DataRow dr3;
                for (int i = 0; i < dt2.Rows.Count - dt1.Rows.Count; i++)
                {
                    dr3 = dt3.NewRow();
                    dt3.Rows.Add(dr3);
                }
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    for (int j = 0; j < dt2.Columns.Count; j++)
                    {
                        dt3.Rows[i][j + dt1.Columns.Count] = dt2.Rows[i][j].ToString();
                    }
                }
            }
            dt3.TableName = DTName; //设置DT的名字
            return dt3;
        }

        /// <summary>
        /// 结构不同
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public static DataTable UnitesDataTable(DataTable dt1, DataTable dt2)
        {
            DataTable dt3 = dt1.Clone();
            for (int i = 0; i < dt2.Columns.Count; i++)
            {
                dt3.Columns.Add(dt2.Columns[i].ColumnName);
            }
            object[] obj = new object[dt3.Columns.Count];

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                dt1.Rows[i].ItemArray.CopyTo(obj, 0);
                dt3.Rows.Add(obj);
            }

            if (dt1.Rows.Count >= dt2.Rows.Count)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    for (int j = 0; j < dt2.Columns.Count; j++)
                    {
                        dt3.Rows[i][j + dt1.Columns.Count] = dt2.Rows[i][j].ToString();
                    }
                }
            }
            else
            {
                DataRow dr3;
                for (int i = 0; i < dt2.Rows.Count - dt1.Rows.Count; i++)
                {
                    dr3 = dt3.NewRow();
                    dt3.Rows.Add(dr3);
                }
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    for (int j = 0; j < dt2.Columns.Count; j++)
                    {
                        dt3.Rows[i][j + dt1.Columns.Count] = dt2.Rows[i][j].ToString();
                    }
                }
            }
            //dt3.TableName = DTName; //设置DT的名字
            return dt3;
        }

        #endregion

        /// <summary>
        ///
        /// </summary>
        /// <param name="First"></param>
        /// <param name="Second"></param>
        /// <returns></returns>
        public static DataTable Union(DataTable First, DataTable Second)
        {
            //Result table
            DataTable table = new DataTable("Union");
            //Build new columns
            DataColumn[] newcolumns = new DataColumn[First.Columns.Count];
            for (int i = 0; i < First.Columns.Count; i++)
            {
                newcolumns[i] = new DataColumn(First.Columns[i].ColumnName, First.Columns[i].DataType);
            }
            //add new columns to result table
            table.Columns.AddRange(newcolumns);
            table.BeginLoadData();
            //Load data from first table
            foreach (DataRow row in First.Rows)
            {
                table.LoadDataRow(row.ItemArray, true);
            }
            //Load data from second table
            foreach (DataRow row in Second.Rows)
            {
                table.LoadDataRow(row.ItemArray, true);
            }
            table.EndLoadData();
            return table;
        }

        /// <summary>
        /// 多个 结构相同的DataTable合并
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllEntrysDataTable(List<DataTable> GetEntrysDataTable)
        {
            DataTable newDataTable = GetEntrysDataTable[0].Clone();
            object[] obj = new object[newDataTable.Columns.Count];
            for (int i = 0; i < GetEntrysDataTable.Count; i++)//entryGroups.GetEntryGroupCount()
            {
                for (int j = 0; j < GetEntrysDataTable[i].Rows.Count; j++)
                {
                    GetEntrysDataTable[i].Rows[j].ItemArray.CopyTo(obj, 0);
                    newDataTable.Rows.Add(obj);
                }
            }
            return newDataTable;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="First"></param>
        /// <param name="Second"></param>
        /// <returns></returns>
        public static DataTable Intersect(DataTable First, DataTable Second)
        {
            //Get reference to Columns in First
            DataColumn[] firstcolumns = new DataColumn[First.Columns.Count];
            for (int i = 0; i < firstcolumns.Length; i++)
            {
                firstcolumns[i] = First.Columns[i];
            }
            //Get reference to Columns in Second
            DataColumn[] secondcolumns = new DataColumn[Second.Columns.Count];
            for (int i = 0; i < secondcolumns.Length; i++)
            {
                secondcolumns[i] = Second.Columns[i];
            }
            //JOIN ON all columns
            DataTable table = SQLOps.Join(First, Second, firstcolumns, secondcolumns);
            table.TableName = "Intersect";
            return table;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Values"></param>
        /// <param name="OtherValues"></param>
        /// <returns></returns>
        public static bool RowEqual(object[] Values, object[] OtherValues)
        {
            if (Values == null)
                return false;
            for (int i = 0; i < Values.Length; i++)
            {
                if (!Values[i].Equals(OtherValues[i]))
                    return false;
            }
            return true;
        }
    }
}
