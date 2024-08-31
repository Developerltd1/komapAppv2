using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace komaxApp.Utility.ExtensionMethod
{
    public class Conversion
    {
        // Helper method to safely get values from the array
        public static string GetValue(string[] dataParts, int index)
        {
            // Check if the index is within bounds and if the value is not null or empty
            if (index < dataParts.Length && !string.IsNullOrEmpty(dataParts[index]))
            {
                return dataParts[index];
            }
            // If out of bounds or invalid, return null
            return null;
        }
        public void CopyProperties<TSource, TDestination>(TSource source, TDestination destination)
        {
            if (source == null || destination == null)
                throw new ArgumentNullException("Source or/and Destination objects are null");

            var sourceProperties = typeof(TSource).GetProperties();
            var destinationProperties = typeof(TDestination).GetProperties();

            foreach (var sourceProperty in sourceProperties)
            {
                if (sourceProperty.CanRead)
                {
                    var destinationProperty = destinationProperties
                        .FirstOrDefault(p => p.Name == sourceProperty.Name && p.PropertyType == sourceProperty.PropertyType);
                    if (destinationProperty != null && destinationProperty.CanWrite)
                    {
                        var value = sourceProperty.GetValue(source, null);
                        destinationProperty.SetValue(destination, value, null);
                    }
                }
            }
        }



        public DataTable ListToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }
}
