using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Rhino.Etl.Core;

namespace ImportData
{
    static class PlaceFactory
    {
        public static Place FromRow(Row row, int id)
        {
            var place = new Place
            {
                Id = id,
                EnglishName = (string) row.ValueOfColumnContaining("English"),
                Type = (string) row.ValueOfColumnContaining("Type"),
                Parish = (string) row.ValueOfColumnContaining("Parish"),
                Keverang = (string) row.ValueOfColumnContaining("Keverang"),
                GridReference = (string) row.ValueOfColumnContaining("Grid"),
                CornishName = (string) row.ValueOfColumnContaining("Cornish"),
                Administration = (string) row.ValueOfColumnContaining("Administration"),
                Notes = (string) row.ValueOfColumnContaining("Notes"),
                Etymology = (string) row.ValueOfColumnContaining("Etymology"),
                HistoricForms = (string) row.ValueOfColumnContaining("Historic")
            };

            return place;
        } 
    }
}
