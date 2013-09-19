using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;
using FileHelpers.DataLink;
using Rhino.Etl.Core;
using Rhino.Etl.Core.Files;
using Rhino.Etl.Core.Operations;

namespace ImportData
{
    class LoadPlaces : AbstractOperation
    {
        private readonly string _filePath;

        public LoadPlaces(string filePath)
        {
            string combine = Path.Combine(Environment.CurrentDirectory,filePath);
            _filePath = combine;
        }

        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            using (var connection = new OleDbConnection(ConnectionString))
            {
                using (var command = new OleDbCommand("SELECT * FROM [Cornwall$]", connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader != null && reader.Read())
                        {
                            yield return Row.FromReader(reader);
                        }
                    }
                }
            }
        }

        private string ConnectionString
        {
            get
            {
                return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + _filePath + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES\"";
            }
        }
    }

    
}
