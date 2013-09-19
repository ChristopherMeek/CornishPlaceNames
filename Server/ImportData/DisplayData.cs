using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Etl.Core;
using Rhino.Etl.Core.Operations;

namespace ImportData
{
    class DisplayData : AbstractOperation
    {
        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            foreach (var row in rows)
            {
                Console.WriteLine(row.ToString());
                yield return row;
            }
        }
    }
}
