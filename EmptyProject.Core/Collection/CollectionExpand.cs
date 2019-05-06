using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EmptyProject.Core.Validation;
using System.Data;
using System.ComponentModel;
using System.Reflection;

namespace EmptyProject.Core.Collection
{
    public static class CollectionExpand
    {
        /// <summary>
        /// 转换为配置文件
        /// </summary>
        /// <param name="InputDic">字典</param>
        /// <param name="ConfigName">
        /// 转换的配置文件根标签
        /// 如果为空则不包含根标签
        /// 默认为空
        /// </param>
        /// <returns></returns>
        public static string ToConfig(this IDictionary<string, string> InputDic, string ConfigName = "")
        {
            if (InputDic == null || InputDic.Count == 0)
                return "";

            StringBuilder sb = new StringBuilder(500);
            if (!ConfigName.IsEmpty())
            {
                sb.Append("<");
                sb.Append(ConfigName);
                sb.Append(">");
            }
            foreach (KeyValuePair<string, string> Item in InputDic)
            {
                sb.Append("<");
                sb.Append(Item.Key);
                sb.Append(">");
                sb.Append(Item.Value);
                sb.Append("</");
                sb.Append(Item.Key);
                sb.Append(">");
            }
            if (!ConfigName.IsEmpty())
            {
                sb.Append("</");
                sb.Append(ConfigName);
                sb.Append(">");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 从集合中随机返回一条信息
        /// </summary>
        /// <typeparam name="T">集合中的对象数据类型</typeparam>
        /// <param name="InputCollection">从哪个集合返回随机数据</param>
        /// <returns></returns>
        public static T Random<T>(this ICollection<T> InputList)
        {
            if (InputList == null || InputList.Count == 0)
                return default(T);

            int Flag = 0;

            Flag = InputList.Count == 1 ? 0 : new Random().Next(0, InputList.Count - 1);

            int Count = 0;

            foreach (T Item in InputList)
            {
                if (Count == Flag)
                    return Item;

                Count++;
            }

            return default(T);
        }

        /// <summary>
        /// IEnumerable的循环扩展
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (T item in items)
            {
                action(item);
            }
        }

        public static List<T> Distinct<T>(IList<T> list)
        {
            List<T> list1 = new List<T>();
            foreach (T obj in list)
            {
                if (!list1.Contains(obj))
                    list1.Add(obj);
            }
            return list1;
        }

        /// <summary>
        /// IList转换DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IList<T> list)
        {
            return ConvertTo<T>(list);
        }
        /// <summary>
        /// IList转换DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ConvertTo<T>(IList<T> list)
        {
            DataTable table = CreateTable<T>();
            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (T item in list)
            {
                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    object val = prop.GetValue(item);
                    if (val != null)
                        row[prop.Name] = val;
                }

                table.Rows.Add(row);
            }

            return table;
        }

        public static IList<T> ConvertTo<T>(IList<DataRow> rows)
        {
            IList<T> list = null;

            if (rows != null)
            {
                list = new List<T>();

                foreach (DataRow row in rows)
                {
                    T item = CreateItem<T>(row);
                    list.Add(item);
                }
            }

            return list;
        }

        /// <summary>
        /// DataTable转换List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static IList<T> ToList<T>(this DataTable table)
        {
            return ConvertTo<T>(table);
        }

        public static IList<T> ConvertTo<T>(DataTable table)
        {
            if (table == null)
            {
                return null;
            }

            List<DataRow> rows = new List<DataRow>();

            foreach (DataRow row in table.Rows)
            {
                rows.Add(row);
            }

            return ConvertTo<T>(rows);
        }

        public static T CreateItem<T>(DataRow row)
        {
            T obj = default(T);
            if (row != null)
            {
                obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in row.Table.Columns)
                {
                    PropertyInfo prop = obj.GetType().GetProperty(column.ColumnName);
                    try
                    {
                        object value = row[column.ColumnName];
                        prop.SetValue(obj, value, null);
                    }
                    catch
                    {
                        // You can log something here
                        //throw;
                    }
                }
            }

            return obj;
        }

        public static DataTable CreateTable<T>()
        {
            Type entityType = typeof(T);
            DataTable table = new DataTable(entityType.Name);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (PropertyDescriptor prop in properties)
            {
                if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    NullableConverter converter = new NullableConverter(prop.PropertyType);
                    table.Columns.Add(prop.Name, converter.UnderlyingType);
                }
                else
                    table.Columns.Add(prop.Name, prop.PropertyType);
            }

            return table;
        }
    }
}
