using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Etl.Core;

namespace ImportData
{
    static class RowExtensions
    {
        public static object ValueOfColumnContaining(this Row row, string columnNamePartial)
        {
            var columnName = row.Columns.FirstOrDefault(name => name.Contains(columnNamePartial));
            return !string.IsNullOrEmpty(columnName) ? row[columnName] : null;
        }
    }
}
