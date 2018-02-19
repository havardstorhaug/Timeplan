using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timeplan.BL
{
    public partial class TidsInndeling
    {
        public static TidsInndeling GetById(int id)
        {
            var db = new TimeplanEntities();
            return GetById(id, db);
        }

        public static TidsInndeling GetById(int id, TimeplanEntities db)
        {
            return db.TidsInndelings.FirstOrDefault(tidsInndeling => tidsInndeling.Id == id);
        }
    }
}
