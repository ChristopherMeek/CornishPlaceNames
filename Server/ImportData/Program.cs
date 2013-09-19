using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportData
{
    class Program
    {
        static void Main(string[] args)
        {
            var properties = new NameValueCollection();
            properties["showDateTime"] = "true";
            Common.Logging.LogManager.Adapter = new Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter(properties);      
            
            var process = new PlaceNameImportProcess();
            process.Execute();

            Console.WriteLine("----------- DONE -----------");
        }
    }
}
