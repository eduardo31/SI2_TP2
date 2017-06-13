using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoAventura.Tools
{
    public static class TablePrinter
    {
        public static IEnumerable<string> Getcolumns(this IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; ++i)
            {
                yield return reader.GetName(i);
            }
        }

        public static void PrintTable(List<string> list)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < list.Count; ++i)
            {
                result.Append(list[i]);
                if (i != list.Count - 1)
                {
                    result.Append(" - ");
                }
            }
            Console.WriteLine(result);
        }
    }
}
