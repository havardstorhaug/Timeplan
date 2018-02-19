using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timeplan.Database.Creator;

namespace Timeplan.SetupDb
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new DatabaseCreator();

            if (db.DatabaseExists())
            {
                Console.WriteLine("Database already exists!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Creating database");
            if (args.Length > 0)
            {
                db.CreateProdDb();
            }
            else
            {
                db.CreateTestDevDb();
            }

            Console.WriteLine("Finished!");
            Console.ReadKey();
        }
    }
}
