using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Rhino.Etl.Core;
using Rhino.Etl.Core.ConventionOperations;
using Rhino.Etl.Core.Operations;
using Simple.Data;

namespace ImportData
{
    class InsertPlaces : AbstractCommandOperation
    {
        private readonly string _connectionString;

        public InsertPlaces(string connectionStringName) : base(connectionStringName)
        {
            this._connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        }

        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            var db = Database.OpenConnection(_connectionString);
            db.Places.DeleteAll();
            foreach (var row in rows)
            {
                var place = row.ToObject<Place>();
                db.Places.Insert(place);
                yield return row;
            }
        }
    }
}
