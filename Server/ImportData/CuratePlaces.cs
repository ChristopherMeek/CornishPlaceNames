using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Etl.Core;
using Rhino.Etl.Core.Operations;

namespace ImportData
{
    class CuratePlaces : AbstractOperation
    {
        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            var rowId = 0;

            foreach (var row in rows)
                yield return Row.FromObject(PlaceFactory.FromRow(row, rowId++));
        }
    }
}
