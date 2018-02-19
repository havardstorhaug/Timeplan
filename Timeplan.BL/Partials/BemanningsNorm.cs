using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Timeplan.BL
{
    public partial class BemanningsNorm
    {
        public void Update(string navn, decimal antall)
        {
            if (Navn != navn)
            {
                Navn = navn;
                IsChanged = true;
            }

            if (Antall != antall)
            {
                Antall = antall;
                IsChanged = true;
            }

        }

        public BemanningsNorm Save()
        {
            var db = new TimeplanEntities();

            var bemanningsNorm = Id > 0 ? GetById(Id, db) : new BemanningsNorm();

            if (bemanningsNorm != null)
            {
                bemanningsNorm.Copy(this, db);

                if (Id <= 0)
                {
                    db.BemanningsNorms.Add(bemanningsNorm);
                }

                db.SaveChanges();

                Id = bemanningsNorm.Id;
            }

            IsChanged = false;

            return bemanningsNorm;
        }

        public void Delete()
        {
            if (Id > 0)
            {
                var db = new TimeplanEntities();

                var bemanningsNorm = GetById(Id, db);

                if (bemanningsNorm != null)
                {
                    db.BemanningsNorms.Remove(bemanningsNorm);

                    db.SaveChanges();
                }
            }
        }

        private void Copy(BemanningsNorm copyFrom, TimeplanEntities db)
        {
            Navn = copyFrom.Navn;
            Antall = copyFrom.Antall;
        }

        public static IEnumerable<BemanningsNorm> GetAll()
        {
            var db = new TimeplanEntities();
            return db.BemanningsNorms;
        }

        public static BemanningsNorm GetById(int id)
        {
            var db = new TimeplanEntities();
            return GetById(id, db);
        }

        public static BemanningsNorm GetById(int id, TimeplanEntities db)
        {
            return db.BemanningsNorms.FirstOrDefault(bemanningsNorm => bemanningsNorm.Id == id);
        }
        
        public bool IsChanged { get; set; }
    
    }
}
