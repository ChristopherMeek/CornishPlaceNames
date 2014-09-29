using System.Linq.Expressions;
using Rhino.Etl.Core;

namespace ImportData
{
    class PlaceNameImportProcess : EtlProcess
    {
        protected override void Initialize()
        {
            this.Register(new LoadPlaces("data\\data.xlsx"));
            this.Register(new CuratePlaces());
            this.Register(new InsertPlaces("dev"));
        }
    }
}